using System;
using System.Text;
using TenderPlanAssignment.Interfaces;

namespace TenderPlanAssignment.BLL
{
    public class DocumentToStringConverter : IDocumentConverter<String, PhoneDictionaryEntry>
    {
        public string ConvertDocument(PhoneDictionaryEntry document)
        {
            return getFullnameLine(document.FullName)
                + getPhoneNumberLine(document.PhoneNumber)
                + getRegionLine(document.Region);
        }

        private string getFullnameLine(string fullname)
        {
            StringBuilder returnValue = new StringBuilder();
            returnValue.Append("ФИО:");
            returnValue.Append(fullname);
            returnValue.Append(Environment.NewLine);
            return returnValue.ToString();
        }

        private string getRegionLine(string region)
        {
            StringBuilder returnValue = new StringBuilder();
            returnValue.Append("Регион:");
            returnValue.Append(region);
            returnValue.Append(Environment.NewLine);
            return returnValue.ToString();
        }

        private string getPhoneNumberLine(string phoneNumber)
        {
            return "Телефон:" + getPhoneRegion(phoneNumber.Substring(0, 3))
                + getPhoneMainPart(phoneNumber.Substring(3))
                + Environment.NewLine;
        }

        private string getPhoneRegion(string region)
        {
            return "(" + region + ")";
        }

        private string getPhoneMainPart(string mainPartOfNumber)
        {
            return "-" + mainPartOfNumber.Substring(0, 3) + "-"
                + mainPartOfNumber.Substring(3, 2)
                + "-"
                + mainPartOfNumber.Substring(5, 2);
        }



    }
}
