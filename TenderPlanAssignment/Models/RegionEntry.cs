using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace TenderPlanAssignment.Models
{
    /// <summary>
    /// Регион и, соответствующий ему, код
    /// </summary>
    public class RegionEntry
    {
        public ObjectId Id
        {
            get;
            set;
        }

        public String Region
        {
            get;
            set;
        }

        public int Code
        {
            get;
            set;
        }



    }
}
