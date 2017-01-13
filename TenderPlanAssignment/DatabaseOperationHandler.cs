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

        public async Task insertNewDocument(string[] args)
        {
            IMongoCollection<PhoneDictionaryEntry> collection = null;
            try
            {
                collection = getCollectionFromDatabase();
            }
            catch (Exception e)
            {
                throw e;
            }
            PhoneDictionaryEntry newEntry = new PhoneDictionaryEntry
            {
                FirstName = args[1],
                Surname = args[2],
                Patronymic = args[3],
                PhoneNumber = args[4],
                Region = args[5]
            };
            await collection.InsertOneAsync(newEntry);
        }

        public async Task displayDocuments(BsonDocument filter)
        {
            IMongoCollection<PhoneDictionaryEntry> collection = null;
            try
            {
                collection = getCollectionFromDatabase();
            }
            catch (Exception e)
            {
                throw e;
            }
            if (filter == null)
            {
                filter = new BsonDocument();
            }
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

        public async Task exportDocuments(String filepath)
        {
            IMongoCollection<PhoneDictionaryEntry> collection = null;
            try
            {
                collection = getCollectionFromDatabase();
            }
            catch (Exception e)
            {
                throw e;
            }
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

        private void writeDocumentInFile(PhoneDictionaryEntry entry, StreamWriter filewriter)
        {
            string newDocument = entry.FirstName + ";" + entry.Surname + ";" + entry.Patronymic + ";" + entry.PhoneNumber + ";" + entry.Region;
            filewriter.WriteLine(newDocument);
        }

        private IMongoCollection<PhoneDictionaryEntry> getCollectionFromDatabase()
        {
            MongoClient connection = new MongoClient(client);
            IMongoDatabase database = connection.GetDatabase("PhoneDictionary");
            return database.GetCollection<PhoneDictionaryEntry>("PersonEntry");
        }

    }
}
