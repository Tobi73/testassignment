using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace TenderPlanAssignment
{
    /// <summary>
    /// Запись в телефонной книге
    /// </summary>
    public class PhoneDictionaryEntry
    {

        public ObjectId Id
        {
            get;
            set;
        }

        public String FullName
        {
            get;
            set;
        }

        public String PhoneNumber
        {
            get;
            set;
        }

        public String Region
        {
            get;
            set;
        }

        /// <summary>
        /// Получение имен всех атрибутов объекта
        /// </summary>
        /// <returns></returns>
        public static List<String> getAttributes()
        {
            var attributes = new List<String>();
            attributes.Add("FullName");
            attributes.Add("PhoneNumber");
            attributes.Add("Region");
            return attributes;
        }
    }
}
