using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderPlanAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length > 2 || args.Length == 0)
            {
                Console.WriteLine("Wrong number of arguments!");
                return;
            }
            DatabaseOperationHandler connection = new DatabaseOperationHandler("mongodb://localhost");
            CommandHandler handler = new CommandHandler(connection);
            handler.processCommand(args);
            Console.ReadKey();
        }
    }
}
