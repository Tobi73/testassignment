using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TenderPlanAssignment;
using MongoDB.Bson;
using MongoDB.Driver;
using TenderPlanAssignment.BLL;
using MongoDB.Bson.Serialization;

namespace TenderPlanAssignmentTest.Test
{
    [TestFixture]
    public class ConvertersTest
    {

        StringArgsToPhoneDictionaryDocument testDocumentConverter;
        StringArgsToFilepathForDBDump testFilePathConverter;
        DocumentToStringConverter testDocumentToString;

        [OneTimeSetUp]
        public void TestSetUp()
        {
            testDocumentConverter = new StringArgsToPhoneDictionaryDocument();
            testFilePathConverter = new StringArgsToFilepathForDBDump();
            testDocumentToString = new DocumentToStringConverter();
        }

        [Test]
        public void shouldConvertToString()
        {
            BsonDocument input = new BsonDocument
            {
                {"FullName", "Зайцев Андрей Олегович"},
                {"PhoneNumber", "9278029511"},
                {"Region", "Москва"}
            };
            PhoneDictionaryEntry entry = BsonSerializer.Deserialize<PhoneDictionaryEntry>(input);
            String properInput = "ФИО:Зайцев Андрей Олегович" + Environment.NewLine
                + "Телефон:(927)-802-95-11" + Environment.NewLine
                + "Регион:Москва" + Environment.NewLine;
            Assert.AreEqual(properInput, testDocumentToString.ConvertDocument(entry));
        }

        [Test]
        public void shouldConvertProperly()
        {
            string operation = "add";
            string properPersonData = "Зайцев Андрей Олегович 9278029511";
            string[] arguments = new string[2];
            arguments[0] = operation;
            arguments[1] = properPersonData;
            Assert.AreEqual(true, testDocumentConverter.EnsureArgumentsAreValid(arguments));
        }

        [Test]
        public void shouldReturnProperPhoneEntry()
        {
            string operation = "add";
            string properPersonData = "Зайцев Андрей Олегович 9278029511";
            string[] arguments = new string[2];
            PhoneDictionaryEntry properEntry = new PhoneDictionaryEntry
            {
                FullName = "Зайцев Андрей Олегович",
                PhoneNumber = "9278029511"
            };
            arguments[0] = operation;
            arguments[1] = properPersonData;
            PhoneDictionaryEntry testedEntry = testDocumentConverter.FormObjectFromArguments(arguments);
            Assert.AreEqual(4, properPersonData.Split(' ').Length);
            Assert.AreEqual(properEntry.FullName, testedEntry.FullName);
            Assert.AreEqual(properEntry.PhoneNumber, testedEntry.PhoneNumber);
        }

    }
}
