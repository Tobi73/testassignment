using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TenderPlanAssignment
{
    public class DatabaseOperationHandler
    {

        private string client;

        public DatabaseOperationHandler(string client)
        {
            this.client = client;
        }

        public void searchDocument(String filter)
        {
            foreach(String attribute in PhoneDictionaryEntry.getAttributes())
            {
                BsonDocument documentFilter = new BsonDocument{
                    {attribute, filter }
                };
                displayDocuments(documentFilter).GetAwaiter().GetResult();
            }
        }

        public async Task deleteDocument(PhoneDictionaryEntry entryForDeletion)
        {
            IMongoCollection<PhoneDictionaryEntry> collection;
            try
            {
                collection = getCollectionFromDatabase("PersonEntry");
                await collection.DeleteOneAsync(entryForDeletion.ToBsonDocument());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task insertNewDocument(PhoneDictionaryEntry newEntry)
        {
            IMongoCollection<PhoneDictionaryEntry> collection;
            try
            {
                collection = getCollectionFromDatabase("PersonEntry");
                await collection.InsertOneAsync(newEntry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task insertNewDocument(PhoneDictionaryEntry newEntry, IMongoCollection<PhoneDictionaryEntry> collection)
        {
            await collection.InsertOneAsync(newEntry);
        }

        public async Task displayDocuments(BsonDocument filter)
        {
            IMongoCollection<PhoneDictionaryEntry> collection;
            try
            {
                if (filter == null)
                {
                    filter = new BsonDocument();
                }
                collection = getCollectionFromDatabase("PersonEntry");
                using (IAsyncCursor<PhoneDictionaryEntry> cursor = await collection.FindAsync(filter))
                {
                    while (await cursor.MoveNextAsync())
                    {
                        IEnumerable<PhoneDictionaryEntry> documentCollection = cursor.Current;
                        foreach (PhoneDictionaryEntry entry in documentCollection)
                        {
                            string documentToDisplay = "ФИО:" + entry.FirstName + " " + entry.Surname + " " + entry.Patronymic + Environment.NewLine
                                + "Номер телефона:" + entry.PhoneNumber + Environment.NewLine
                                + "Регион:" + entry.Region + Environment.NewLine;
                            Console.WriteLine(documentToDisplay);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task exportDocuments(String filepath)
        {
            IMongoCollection<PhoneDictionaryEntry> collection;
            try
            {
                collection = getCollectionFromDatabase("PersonEntry");
                using (IAsyncCursor<PhoneDictionaryEntry> cursor = await collection.FindAsync(new BsonDocument()))
                {
                    while (await cursor.MoveNextAsync())
                    {
                        using (StreamWriter filewriter = new StreamWriter(filepath, true, Encoding.UTF8))
                        {
                            IEnumerable<PhoneDictionaryEntry> documentCollection = cursor.Current;
                            foreach (PhoneDictionaryEntry entry in documentCollection)
                            {
                                try
                                {
                                    writeDocumentInFile(entry, filewriter);
                                }
                                catch (Exception e)
                                {
                                    throw e;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task importDocuments(String filepath)
        {
            IMongoCollection<PhoneDictionaryEntry> collection;
            try
            {
                collection = getCollectionFromDatabase("PersonEntry");
                using (StreamReader filereader = new StreamReader(filepath))
                {
                    while (!filereader.EndOfStream)
                    {
                        PhoneDictionaryEntry formedDocument = formDocumentFromFile(filereader);
                        insertNewDocument(formedDocument).GetAwaiter().GetResult();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void writeDocumentInFile(PhoneDictionaryEntry entry, StreamWriter filewriter)
        {
            string newDocument = entry.FirstName + ";" + entry.Surname + ";" + entry.Patronymic + ";" + entry.PhoneNumber + ";" + entry.Region;
            filewriter.WriteLine(newDocument);
        }

        private PhoneDictionaryEntry formDocumentFromFile(StreamReader filereader)
        {
            string entryInString = filereader.ReadLine();
            string[] documentAttributes = new string[5];
            Array.Copy(entryInString.Split(';'), documentAttributes, 5);
            PhoneDictionaryEntry newEntry = new PhoneDictionaryEntry
            {
                FirstName = documentAttributes[0],
                Surname = documentAttributes[1],
                Patronymic = documentAttributes[2],
                PhoneNumber = documentAttributes[3],
            };
            if(documentAttributes[4] != null && !String.IsNullOrEmpty(documentAttributes[4]))
            {
                newEntry.Region = documentAttributes[4];
            }
            return newEntry;
        }

        private IMongoCollection<PhoneDictionaryEntry> getCollectionFromDatabase(string collectionName)
        {
            MongoClient connection = new MongoClient(client);
            IMongoDatabase database = connection.GetDatabase("PhoneDictionary");
            return database.GetCollection<PhoneDictionaryEntry>(collectionName);
        }

    }
}
