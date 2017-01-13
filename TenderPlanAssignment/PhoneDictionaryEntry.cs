using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TenderPlanAssignment
{
    public class PhoneDictionaryEntry
    {

        public ObjectId Id
        {
            get;
            set;
        }

        public String FirstName
        {
            get;
            set;
        }
        
        public String Surname
        {
            get;
            set;
        }

        public String Patronymic
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

        public static List<String> getAttributes()
        {
            List<String> attributes = new List<String>();
            attributes.Add("FirstName");
            attributes.Add("Surname");
            attributes.Add("Patronymic");
            attributes.Add("PhoneNumber");
            attributes.Add("Region");
            return attributes;
        }
    }
}
