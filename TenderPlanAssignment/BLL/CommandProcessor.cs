using System;
using System.IO;
using System.Collections.Generic;
using TenderPlanAssignment.BLL;
using TenderPlanAssignment.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using TenderPlanAssignment.Models;

namespace TenderPlanAssignment
{

    /// <summary>
    /// Класс для обработки основных операций программы.
    /// У большинства методов в качестве аргументов выступает массив строк,
    /// который формирует пользователь при использовании программы
    /// </summary>

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

        /// <summary>
        /// Поиск записей в базе по введеной строке с помощью регулярного выражения.
        /// Записи возвращаются в виде обработанной строки.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public IEnumerable<String> FindDocuments(string[] input)
        {
            if (!InputIsValid(input) || input.Length != 2)
            {
                throw new Exception("Wrong number of arguments!");
            }
            DocumentCollection<PhoneDictionaryEntry> collection = databaseHandler.GetDocumentCollection("PersonEntry", indexer);
            var regex = input[1];
            FilterDefinition<PhoneDictionaryEntry> filter = GetRegexFilters(regex);
            var converter = new DocumentToStringConverter();
            foreach (var entry in collection.FindDocument(filter))
            {
                yield return converter.ConvertDocument(entry);
            }
        }

        /// <summary>
        /// Получение всех записей из базы данных.
        /// Записи возвращаются в виде обработанной строки.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public IEnumerable<String> GetAllDocuments(string[] input)
        {
            if (!InputIsValid(input) || input.Length != 1)
            {
                throw new Exception("Wrong number of arguments!");
            }
            DocumentCollection<PhoneDictionaryEntry> collection = databaseHandler.GetDocumentCollection("PersonEntry", indexer);
            var converter = new DocumentToStringConverter();
            foreach (var entry in collection.GetAllData())
            {
                yield return converter.ConvertDocument(entry);
            }
        }

        /// <summary>
        /// Добавление новой записи в базу.
        /// Вторым аргументом должна быть строка в формате "Фамилия Имя Отчество Телефон".
        /// Телефон должен состоять ровно из 10 цифр.
        /// </summary>
        /// <param name="input"></param>
        public void AddDocument(string[] input)
        {
            if (!InputIsValid(input) || input.Length != 2)
            {
                throw new Exception("Wrong number of arguments!");
            }
            var entryForInsertConverter = new StringArgsToPhoneDictionaryDocument();
            PhoneDictionaryEntry newEntry = entryForInsertConverter.FormObjectFromArguments(input);
            newEntry.Region = TryDetermineRegion(newEntry);
            DocumentCollection<PhoneDictionaryEntry> collection = databaseHandler.GetDocumentCollection("PersonEntry", indexer);
            collection.InsertDocument(newEntry);
        }

        /// <summary>
        /// Удаление записи из базы.
        /// Вторым аргументом должна быть строка в формате "Фамилия Имя Отчество Телефон".
        /// Телефон должен состоять ровно из 10 цифр.
        /// </summary>
        /// <param name="input"></param>
        public void RemoveDocument(string[] input)
        {
            if (!InputIsValid(input) || input.Length != 2)
            {
                throw new Exception("Wrong number of arguments!");
            }
            var entryForDeletionConverter = new StringArgsToPhoneDictionaryDocument();
            PhoneDictionaryEntry entryForDeletion = entryForDeletionConverter.FormObjectFromArguments(input);
            DocumentCollection<PhoneDictionaryEntry> collection = databaseHandler.GetDocumentCollection("PersonEntry", indexer);
            collection.DeleteDocument(entryForDeletion);
        }

        /// <summary>
        /// Добавление записей в базу из файла.
        /// Вторым аргументом должен являться путь к файлу в формате .csv.
        /// </summary>
        /// <param name="input"></param>
        public void ImportDocuments(String[] input)
        {
            if (!InputIsValid(input) || input.Length != 2)
            {
                throw new Exception("Wrong number of arguments!");
            }
            var checker = new StringArgsToFilepathForDBDump();
            var filepath = checker.FormObjectFromArguments(input);
            var importer = new DocumentImporter();
            DocumentCollection<PhoneDictionaryEntry> collection = databaseHandler.GetDocumentCollection("PersonEntry", indexer);
            foreach (var entry in importer.ImportDocuments(filepath))
            {
                collection.InsertDocument(entry);
            }
        }

        /// <summary>
        /// Экспорт данных из базы.
        /// Вторым аргументом может выступать путь к файлу в формате .csv,
        /// куда будут загружаться данные.
        /// Если второй аргумент отсутствует, то экспорт в проводится в
        /// файл export/dumpfile.csv.
        /// </summary>
        /// <param name="input"></param>
        public void ExportDocuments(String[] input)
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
            DocumentCollection<PhoneDictionaryEntry> collection = databaseHandler.GetDocumentCollection("PersonEntry", indexer);
            exporter.ExportDocuments(filepath, collection.GetAllData());
        }


        /// <summary>
        /// Проверка на верное количество введеных аргументов.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool InputIsValid(String[] input)
        {
            if (input.Length > 2 || input.Length == 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Получение фильтров по регулярному выражению по всем атрибутам документа
        /// коллекции PhoneDictionary.
        /// </summary>
        /// <param name="regex"></param>
        /// <returns></returns>
        private FilterDefinition<PhoneDictionaryEntry> GetRegexFilters(String regex)
        {
            var filters = new List<FilterDefinition<PhoneDictionaryEntry>>();
            foreach (String attribute in PhoneDictionaryEntry.getAttributes())
            {
                filters.Add(Builders<PhoneDictionaryEntry>.Filter.Regex(attribute, new BsonRegularExpression(regex)));
            }
            return Builders<PhoneDictionaryEntry>.Filter.Or(filters);
        }

        /// <summary>
        /// Определение региона пользователя по коду региона в номере телефона.
        /// При выполнении метода используется дополнительная коллекция Region,
        /// в которой находятся документы с кодами регионов.
        /// Если код региона отсутствует в коллекции Region, 
        /// то атрибут Region в новом документе остается пустым.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private String TryDetermineRegion(PhoneDictionaryEntry entry)
        {
            DocumentCollection<RegionEntry> collection = databaseHandler.GetDocumentCollection<RegionEntry>("Region");
            foreach (var document in collection.GetAllData())
            {
                if (ArePhoneRegionsEqual(entry.PhoneNumber, document.Code))
                {
                    return document.Region;
                }
            }
            return String.Empty;
        }


        /// <summary>
        /// Сравнение кода региона в новом документе
        /// с кодом региона из коллекции Region.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="regionCode"></param>
        /// <returns></returns>
        private bool ArePhoneRegionsEqual(String phoneNumber, int regionCode)
        {
            int regionCodeInNumber = Convert.ToInt32(phoneNumber.Substring(0, 3));
            if (regionCode.Equals(regionCodeInNumber))
            {
                return true;
            }
            else return false;
        }

    }
}
