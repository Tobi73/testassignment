using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TenderPlanAssignment
{
    public class DocumentCollection
    {

        private IMongoDatabase database;
        private IndexKeysDefinition<PhoneDictionaryEntry> indexParams;
        private String collectionName;

        public DocumentCollection(IMongoDatabase database, String collectionName)
        {
            this.database = database;
            this.collectionName = collectionName;
        }

        public DocumentCollection(IMongoDatabase database, String collectionName, IndexKeysDefinition<PhoneDictionaryEntry> indexParams)
        {
            this.database = database;
            this.collectionName = collectionName;
            this.indexParams = indexParams;
        }

        public IEnumerable<PhoneDictionaryEntry> FindDocument(FilterDefinition<PhoneDictionaryEntry> filter)
        {
            IMongoCollection<PhoneDictionaryEntry> collection = GetCollectionFromDatabase();
            return collection.Find<PhoneDictionaryEntry>(filter).ToEnumerable<PhoneDictionaryEntry>();
        }

        public void DeleteDocument(PhoneDictionaryEntry entryForDeletion)
        {
            IMongoCollection<PhoneDictionaryEntry> collection = GetCollectionFromDatabase();
            collection.DeleteOne(entryForDeletion.ToBsonDocument());
        }

        public void InsertDocument(PhoneDictionaryEntry newEntry)
        {
            IMongoCollection<PhoneDictionaryEntry> collection = GetCollectionFromDatabase();
            collection.InsertOne(newEntry);
        }

        public IEnumerable<PhoneDictionaryEntry> GetAllData()
        {
            IMongoCollection<PhoneDictionaryEntry> collection = GetCollectionFromDatabase();
            using (var cursor = collection.Find<PhoneDictionaryEntry>(FilterDefinition<PhoneDictionaryEntry>.Empty).ToCursor())
            {
                while (cursor.MoveNext())
                {
                    var batchOfDocuments = cursor.Current;
                    foreach (var document in batchOfDocuments)
                    {
                        yield return document;
                    }
                }
            }
        }

        public IMongoCollection<PhoneDictionaryEntry> GetCollectionFromDatabase()
        {
            var collection = database.GetCollection<PhoneDictionaryEntry>(collectionName);
            if (indexParams != null)
            {
                collection.Indexes.CreateOne(indexParams);
            }
            return collection;
        }

    }
}
