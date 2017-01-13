using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TenderPlanAssignment
{
    public class StringArgsToPhoneDictionaryDocument : IArgumentHandler<PhoneDictionaryEntry>
    {
        public bool ensureArgumentsAreValid(string[] arguments)
        {
            string stringToCheck = arguments[1];
            Regex mask = new Regex(@"^\D+\s\D+\s\D+\s(\d){10}$");
            if (!mask.IsMatch(stringToCheck))
            {
                return false;
            } else
            {
                return true;
            }
        }

        public PhoneDictionaryEntry formObjectFromArguments(string[] arguments)
        {
            if(arguments.Length != 2)
            {
                throw new Exception("Wrong number of arguments!");
            }
            if (!ensureArgumentsAreValid(arguments))
            {
                throw new Exception("Wrong format of arguments!");
            }
            string[] fieldsOfDocument = arguments[1].Split(' ');
            PhoneDictionaryEntry newEntry = new PhoneDictionaryEntry{
                FirstName = fieldsOfDocument[0].Trim(),
                Surname = fieldsOfDocument[1].Trim(),
                Patronymic = fieldsOfDocument[2].Trim(),
                PhoneNumber = fieldsOfDocument[3].Trim(),
            };
            return newEntry;

        }
    }
}
