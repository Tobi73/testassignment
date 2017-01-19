using System;
using MongoDB.Driver;
using System.Configuration;

namespace TenderPlanAssignment.Repository
{
    /// <summary>
    /// Класс для работы с базой данных
    /// </summary>
    public class DatabaseUnit
    {

        IMongoDatabase database;

        public DatabaseUnit(String connectionString, String databaseName)
        {
            var connection = new MongoClient(connectionString);
            this.database = connection.GetDatabase(databaseName);
        }

        public DatabaseUnit(String databaseName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;
            var connection = new MongoClient(connectionString);
            this.database = connection.GetDatabase(databaseName);
        }

        /// <summary>
        /// Получение нового объекта класса DocumentCollection,
        /// для работы с указанной коллекцией.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public DocumentCollection<T> GetDocumentCollection<T>(String collectionName)
        {
            return new DocumentCollection<T>(database, collectionName);
        }

        /// <summary>
        /// Получение нового объекта класса DocumentCollection,
        /// для работы с указанной коллекцией.
        /// Вторым аргументом является способ индексации,
        /// который будет применен к полученной коллекции.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="indexParams"></param>
        /// <returns></returns>
        public DocumentCollection<T> GetDocumentCollection<T>(String collectionName, IndexKeysDefinition<T> indexParams)
        {
            return new DocumentCollection<T>(database, collectionName, indexParams);
        }


    }
}
