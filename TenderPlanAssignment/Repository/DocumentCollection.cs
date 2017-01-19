using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TenderPlanAssignment
{
    /// <summary>
    /// Класс для взаимодействия с коллекцией из базы данных.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DocumentCollection<T>
    {

        private IMongoDatabase database;
        private IndexKeysDefinition<T> indexParams;
        private String collectionName;

        public DocumentCollection(IMongoDatabase database, String collectionName)
        {
            this.database = database;
            this.collectionName = collectionName;
        }

        public DocumentCollection(IMongoDatabase database, String collectionName, IndexKeysDefinition<T> indexParams)
        {
            this.database = database;
            this.collectionName = collectionName;
            this.indexParams = indexParams;
        }

        /// <summary>
        /// Поиск документа в коллекции по фильтру.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<T> FindDocument(FilterDefinition<T> filter)
        {
            IMongoCollection<T> collection = GetCollectionFromDatabase();
            return collection.Find<T>(filter).ToEnumerable<T>();
        }

        /// <summary>
        /// Удаление документа из коллекции.
        /// </summary>
        /// <param name="entryForDeletion"></param>
        public void DeleteDocument(T entryForDeletion)
        {
            IMongoCollection<T> collection = GetCollectionFromDatabase();
            collection.DeleteOne(entryForDeletion.ToBsonDocument());
        }

        /// <summary>
        /// Добавление документа в коллекцию.
        /// </summary>
        /// <param name="newEntry"></param>
        public void InsertDocument(T newEntry)
        {
            IMongoCollection<T> collection = GetCollectionFromDatabase();
            collection.InsertOne(newEntry);
        }

        /// <summary>
        /// Получение всех документов из коллекции.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAllData()
        {
            IMongoCollection<T> collection = GetCollectionFromDatabase();
            using (var cursor = collection.Find<T>(FilterDefinition<T>.Empty).ToCursor())
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

        /// <summary>
        /// Получение коллекции из базы данных.
        /// </summary>
        /// <returns></returns>
        public IMongoCollection<T> GetCollectionFromDatabase()
        {
            var collection = database.GetCollection<T>(collectionName);
            if (indexParams != null)
            {
                collection.Indexes.CreateOne(indexParams);
            }
            return collection;
        }

    }
}
