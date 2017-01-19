using System;
using System.IO;
using System.Collections.Generic;
using TenderPlanAssignment.BLL;
using TenderPlanAssignment.Repository;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TenderPlanAssignment
{
    public class CommandProcessor
    {

        private DatabaseUnit databaseHandler;
        private IndexKeysDefinition<PhoneDictionaryEntry> indexer;

        public IndexKeysDefinition<PhoneDictionaryEntry> Indexer
        {
            set
            {
                indexer = value;
            }
            get
            {
                return indexer;
            }
        }

        public CommandProcessor(DatabaseUnit handler)
        {
            this.databaseHandler = handler;
            IndexKeysDefinitionBuilder<PhoneDictionaryEntry> test = Builders<PhoneDictionaryEntry>.IndexKeys;
            IndexKeysDefinition<PhoneDictionaryEntry> keys = test.Text("FullName").Text("Phone").Text("Region");
            indexer = keys;
        }

        public IEnumerable<String> FindDocuments(string[] input)
        {
            if (!InputIsValid(input) || input.Length != 2)
            {
                throw new Exception("Wrong number of arguments!");
            }
            DocumentCollection collection = databaseHandler.GetDocumentCollection("PersonEntry", indexer);
            var regex = input[1];
            FilterDefinition<PhoneDictionaryEntry> filter = getRegexFilters(regex);
            var converter = new DocumentToStringConverter();
            foreach (var entry in collection.FindDocument(filter))
            {
                yield return converter.ConvertDocument(entry);
            }
        }

        public IEnumerable<String> GetAllDocuments(string[] input)
        {
            if (!InputIsValid(input) || input.Length != 1)
            {
                throw new Exception("Wrong number of arguments!");
            }
            DocumentCollection collection = databaseHandler.GetDocumentCollection("PersonEntry", indexer);
            var converter = new DocumentToStringConverter();
            foreach (var entry in collection.GetAllData())
            {
                yield return converter.ConvertDocument(entry);
            }
        }

        public void AddDocument(string[] input)
        {
            if (!InputIsValid(input) || input.Length != 2)
            {
                throw new Exception("Wrong number of arguments!");
            }
            var entryForInsertConverter = new StringArgsToPhoneDictionaryDocument();
            PhoneDictionaryEntry newEntry = entryForInsertConverter.FormObjectFromArguments(input);
            DocumentCollection collection = databaseHandler.GetDocumentCollection("PersonEntry", indexer);
            collection.InsertDocument(newEntry);
        }

        public void RemoveDocument(string[] input)
        {
            if (!InputIsValid(input) || input.Length != 2)
            {
                throw new Exception("Wrong number of arguments!");
            }
            var entryForDeletionConverter = new StringArgsToPhoneDictionaryDocument();
            PhoneDictionaryEntry entryForDeletion = entryForDeletionConverter.FormObjectFromArguments(input);
            DocumentCollection collection = databaseHandler.GetDocumentCollection("PersonEntry", indexer);
            collection.DeleteDocument(entryForDeletion);
        }

        public void ExportDocuments(String[] input)
        {
            if (!InputIsValid(input) || input.Length != 2)
            {
                throw new Exception("Wrong number of arguments!");
            }
            var checker = new StringArgsToFilepathForDBDump();
            var filepath = checker.FormObjectFromArguments(input);
            var importer = new DocumentImporter();
            DocumentCollection collection = databaseHandler.GetDocumentCollection("PersonEntry", indexer);
            foreach (var entry in importer.ImportDocuments(filepath))
            {
                collection.InsertDocument(entry);
            }
        }

        public void ImportDocuments(String[] input)
        {
            if (!InputIsValid(input))
            {
                throw new Exception("Wrong number of arguments!");
            }
            var filepath = "export/dumpfile.csv";
            if (input.Length == 2)
            {
                var checker = new StringArgsToFilepathForDBDump();
                filepath = checker.FormObjectFromArguments(input);
            }
            else
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/export");
            }
            var exporter = new DocumentExporter();
            DocumentCollection collection = databaseHandler.GetDocumentCollection("PersonEntry", indexer);
            exporter.ExportDocuments(filepath, collection.GetAllData());
        }

        private bool InputIsValid(String[] input)
        {
            if (input.Length > 2 || input.Length == 0)
            {
                return false;
            }
            return true;
        }

        private FilterDefinition<PhoneDictionaryEntry> getRegexFilters(String regex)
        {
            var filters = new List<FilterDefinition<PhoneDictionaryEntry>>();
            foreach (String attribute in PhoneDictionaryEntry.getAttributes())
            {
                filters.Add(Builders<PhoneDictionaryEntry>.Filter.Regex(attribute, new BsonRegularExpression(regex)));
            }
            return Builders<PhoneDictionaryEntry>.Filter.Or(filters);
        }

    }
}
