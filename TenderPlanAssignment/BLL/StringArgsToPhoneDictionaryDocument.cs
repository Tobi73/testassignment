using System;
using System.Text.RegularExpressions;

namespace TenderPlanAssignment
{
    public class StringArgsToPhoneDictionaryDocument : IArgumentHandler<PhoneDictionaryEntry>
    {
        public bool EnsureArgumentsAreValid(string[] arguments)
        {
            var stringToCheck = arguments[1];
            var mask = new Regex(@"^\D+\s\D+\s\D+\s(\d){10}$");
            if (!mask.IsMatch(stringToCheck))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public PhoneDictionaryEntry FormObjectFromArguments(string[] arguments)
        {
            if (arguments.Length != 2)
            {
                throw new Exception("Wrong number of arguments!");
            }
            if (!EnsureArgumentsAreValid(arguments))
            {
                throw new Exception("Wrong format of arguments!");
            }
            var fieldsOfDocument = arguments[1].Split(' ');
            var newEntry = new PhoneDictionaryEntry
            {
                FullName = fieldsOfDocument[0].Trim() + " " + fieldsOfDocument[1].Trim() + " " + fieldsOfDocument[2].Trim(),
                PhoneNumber = fieldsOfDocument[3].Trim()
            };
            return newEntry;

        }
    }
}
