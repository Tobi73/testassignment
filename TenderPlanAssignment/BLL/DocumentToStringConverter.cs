using System;
using System.Text;
using TenderPlanAssignment.Interfaces;

namespace TenderPlanAssignment.BLL
{

    /// <summary>
    /// Класс, который преобразовывает документ коллекции в, приемлимую для демонстрации, строку.
    /// </summary>
    public class DocumentToStringConverter : IDocumentConverter<String, PhoneDictionaryEntry>
    {

        /// <summary>
        /// Главный public метод, который конвертирует документ в строку.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public string ConvertDocument(PhoneDictionaryEntry document)
        {
            return getFullnameLine(document.FullName)
                + getPhoneNumberLine(document.PhoneNumber)
                + getRegionLine(document.Region);
        }

        /// <summary>
        /// Получение строки с ФИО.
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        private string getFullnameLine(string fullname)
        {
            return "ФИО:" + fullname + Environment.NewLine;
        }

        /// <summary>
        /// Получение строки с регионом.
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        private string getRegionLine(string region)
        {
            return "Регион:" + region + Environment.NewLine;
        }

        /// <summary>
        /// Основной метод для формирования строки с номером телефона.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        private string getPhoneNumberLine(string phoneNumber)
        {
            return "Телефон:" + getPhoneRegion(phoneNumber.Substring(0, 3))
                + getPhoneMainPart(phoneNumber.Substring(3))
                + Environment.NewLine;
        }

        /// <summary>
        /// Получение кода региона.
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        private string getPhoneRegion(string region)
        {
            return "(" + region + ")";
        }


        /// <summary>
        /// Получение основной части номера телефона.
        /// </summary>
        /// <param name="mainPartOfNumber"></param>
        /// <returns></returns>
        private string getPhoneMainPart(string mainPartOfNumber)
        {
            return "-" + mainPartOfNumber.Substring(0, 3) + "-"
                + mainPartOfNumber.Substring(3, 2)
                + "-"
                + mainPartOfNumber.Substring(5, 2);
        }



    }
}
