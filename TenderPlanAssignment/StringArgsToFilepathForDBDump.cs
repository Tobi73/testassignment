using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TenderPlanAssignment
{
    public class StringArgsToFilepathForDBDump : IArgumentHandler<String>
    {

        public bool ensureArgumentsAreValid(string[] arguments)
        {
            string stringToCheck = arguments[1];
            Regex mask = new Regex(@".+[.]cvs$");
            if (!mask.IsMatch(stringToCheck))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string formObjectFromArguments(string[] arguments)
        {
            if (!ensureArgumentsAreValid(arguments))
            {
                throw new Exception("Wrong format of arguments!");
            }
            return arguments[1];
        }
    }
}
