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

        DatabaseOperationHandler handler;

        public CommandHandler(DatabaseOperationHandler handler)
        {
            this.handler = handler;
        }

        public void processCommand(String[] input)
        {
            try
            {
                switch (input[0])
                {
                    case "add":
                        handler.insertNewDocument(input).GetAwaiter().GetResult();
                        break;
                    case "search":
                        handler.searchDocument(input[1]);
                        break;
                    case "remove":
                        break;
                    case "restore":
                        break;
                    case "dump":
                        handler.exportDocuments(input[1]).GetAwaiter().GetResult();
                        break;
                    case "show":
                        handler.displayDocuments(null).GetAwaiter().GetResult();
                        break;
                    default:
                        break;
                }
            }
            catch (MongoException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (TimeoutException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
