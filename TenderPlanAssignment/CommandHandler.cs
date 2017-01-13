using MongoDB.Driver;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderPlanAssignment
{
    public class CommandHandler
    {

        DatabaseOperationHandler databaseHandler;

        public CommandHandler(DatabaseOperationHandler handler)
        {
            this.databaseHandler = handler;
        }

        public void processCommand(String[] input)
        {
            try
            {
                switch (input[0])
                {
                    case "add":
                        StringArgsToPhoneDictionaryDocument converter = new StringArgsToPhoneDictionaryDocument();
                        PhoneDictionaryEntry newEntry = converter.formObjectFromArguments(input);
                        databaseHandler.insertNewDocument(newEntry).GetAwaiter().GetResult();
                        break;
                    case "search":
                        databaseHandler.searchDocument(input[1]);
                        break;
                    case "remove":
                        break;
                    case "restore":
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

    }
}
