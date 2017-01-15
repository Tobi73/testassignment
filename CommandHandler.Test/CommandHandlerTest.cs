using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TenderPlanAssignment;
using MongoDB.Bson;


namespace TenderPlanAssignmentTest.Test
{
    [TestFixture]
    public class CommandHandlerTest
    {

        DatabaseOperationHandler testDataHandler;
        CommandProcessor testCommandHandler;
        StringArgsToPhoneDictionaryDocument testDocumentConverter;
        StringArgsToFilepathForDBDump testFilePathConverter;

        [OneTimeSetUp]
        public void TestSetUp()
        {
            testDataHandler = new DatabaseOperationHandler("mongodb://localhost");
            testCommandHandler = new CommandProcessor(testDataHandler);
            testDocumentConverter = new StringArgsToPhoneDictionaryDocument();
            testFilePathConverter = new StringArgsToFilepathForDBDump();
        }

        [Test]
        public void shouldConvertProperly()
        {
            string operation = "add";
            string properPersonData = "Зайцев Андрей Олегович 9278029511";
            string[] arguments = new string[2];
            arguments[0] = operation;
            arguments[1] = properPersonData;
            Assert.AreEqual(true, testDocumentConverter.ensureArgumentsAreValid(arguments));
        }

        [Test]
        public void shouldReturnProperPhoneEntry()
        {
            string operation = "add";
            string properPersonData = "Зайцев Андрей Олегович 9278029511";
            string[] arguments = new string[2];
            PhoneDictionaryEntry properEntry = new PhoneDictionaryEntry
            {
                FirstName = "Зайцев",
                Surname = "Андрей",
                Patronymic = "Олегович",
                PhoneNumber = "9278029511"
            };
            arguments[0] = operation;
            arguments[1] = properPersonData;
            PhoneDictionaryEntry testedEntry = testDocumentConverter.formObjectFromArguments(arguments);
            Assert.AreEqual(4, properPersonData.Split(' ').Length);
            Assert.AreEqual(properEntry.FirstName, testedEntry.FirstName);
            Assert.AreEqual(properEntry.Surname, testedEntry.Surname);
            Assert.AreEqual(properEntry.Patronymic, testedEntry.Patronymic);
            Assert.AreEqual(properEntry.PhoneNumber, testedEntry.PhoneNumber);
        }


    }
}
