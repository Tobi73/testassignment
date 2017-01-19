using System;
using System.Text.RegularExpressions;

namespace TenderPlanAssignment
{

    /// <summary>
    /// Класс, который проверяет на правильность введеный путь к файлу для экспорта
    /// </summary>
    public class StringArgsToFilepathForDBDump : IArgumentHandler<String>
    {

        /// <summary>
        /// Проверка на правильность путя к файлу для экспорта
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Возврат строки с путем к файлу
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
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
