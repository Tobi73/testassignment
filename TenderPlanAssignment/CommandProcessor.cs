using MongoDB.Driver;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderPlanAssignment
{
    public class CommandProcessor
    {

        DatabaseOperationHandler databaseHandler;

        public CommandProcessor(DatabaseOperationHandler handler)
        {
            this.databaseHandler = handler;
        }

        public void processCommand(String[] input)
        {
            if (!inputIsValid(input))
            {
                Console.WriteLine("Wrong number of arguments!");
                return;
            }
            try
            {
                switch (input[0])
                {
                    case "add":
                        StringArgsToPhoneDictionaryDocument entryForInsertConverter = new StringArgsToPhoneDictionaryDocument();
                        PhoneDictionaryEntry newEntry = entryForInsertConverter.formObjectFromArguments(input);
                        databaseHandler.insertNewDocument(newEntry).GetAwaiter().GetResult();
                        break;
                    case "search":
                        databaseHandler.searchDocument(input[1]);
                        break;
                    case "remove":
                        StringArgsToPhoneDictionaryDocument entryForDeletionConverter = new StringArgsToPhoneDictionaryDocument();
                        PhoneDictionaryEntry entryForDeletion = entryForDeletionConverter.formObjectFromArguments(input);
                        databaseHandler.deleteDocument(entryForDeletion).GetAwaiter().GetResult();
                        break;
                    case "restore":
                        if (input.Length == 2)
                        {
                            StringArgsToFilepathForDBDump checker = new StringArgsToFilepathForDBDump();
                            string filepath = checker.formObjectFromArguments(input);
                            databaseHandler.importDocuments(filepath).GetAwaiter().GetResult();
                        }
                        else
                        {
                            Console.WriteLine("Filepath to .csv file is missing!");
                        }
                        break;
                    case "dump":
                        if(input.Length == 2)
                        {
                            StringArgsToFilepathForDBDump checker = new StringArgsToFilepathForDBDump();
                            string filepath = checker.formObjectFromArguments(input);
                            databaseHandler.exportDocuments(filepath).GetAwaiter().GetResult();
                        } else
                        {
                            Directory.CreateDirectory(Directory.GetCurrentDirectory() +"/export");
                            databaseHandler.exportDocuments("export/dumpfile.csv").GetAwaiter().GetResult();
                        }
                        break;
                    case "show":
                        databaseHandler.displayDocuments(null).GetAwaiter().GetResult();
                        break;
                    default:
                        Console.WriteLine("Wrong number of arguments!");
                        break;
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("Wrong number of arguments!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private bool inputIsValid(String[] input)
        {
            if (input.Length > 2 || input.Length == 0)
            {
                return false;
            }
            return true;
        }

    }
}
