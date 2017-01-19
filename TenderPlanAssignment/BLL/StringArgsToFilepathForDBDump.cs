using System;
using System.Text.RegularExpressions;

namespace TenderPlanAssignment
{
    public class StringArgsToFilepathForDBDump : IArgumentHandler<String>
    {

        public bool EnsureArgumentsAreValid(string[] arguments)
        {
            var stringToCheck = arguments[1];
            var mask = new Regex(@".+[.]csv$");
            if (!mask.IsMatch(stringToCheck))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string FormObjectFromArguments(string[] arguments)
        {
            if (!EnsureArgumentsAreValid(arguments))
            {
                throw new Exception("Wrong format of arguments!");
            }
            return arguments[1];
        }
    }
}
