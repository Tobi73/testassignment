using System;
using TenderPlanAssignment.Repository;

namespace TenderPlanAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            var database = new DatabaseUnit("PhoneDictionary");
            var handler = new CommandProcessor(database);
            try
            {
                switch (args[0])
                {
                    case "add":
                        handler.AddDocument(args);
                        break;
                    case "search":
                        foreach (String entry in handler.FindDocuments(args))
                        {
                            Console.WriteLine(entry);
                        }
                        break;
                    case "remove":
                        handler.RemoveDocument(args);
                        break;
                    case "export":
                        handler.ExportDocuments(args);
                        break;
                    case "import":
                        handler.ImportDocuments(args);
                        break;
                    case "show":
                        foreach (String entry in handler.GetAllDocuments(args))
                        {
                            Console.WriteLine(entry);
                        }
                        break;
                    default:
                        Console.WriteLine("Wrong number of arguments!");
                        break;
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("Wrong number of arguments!");
                Console.WriteLine(e.StackTrace);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
