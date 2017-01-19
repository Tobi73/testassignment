using System;
using MongoDB.Driver;
using System.Configuration;

namespace TenderPlanAssignment.Repository
{
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

        public DocumentCollection GetDocumentCollection(String collectionName)
        {
            return new DocumentCollection(database, collectionName);
        }

        public DocumentCollection GetDocumentCollection(String collectionName, IndexKeysDefinition<PhoneDictionaryEntry> indexParams)
        {
            return new DocumentCollection(database, collectionName, indexParams);
        }


    }
}
