using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ExtractionCompta.Repositories;

namespace ExtractionCompta
{
    class Program
    {
        static void Main(string[] args) 
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Extraction Comptable. Version " + Assembly.GetExecutingAssembly().GetName().Version);
                Console.WriteLine("Usage : [ExcelSource] [CcvDestination]");
                return;
            }
          
            var source = new ExcelComptaSourceRepository(args[0]);
            var destination = new CsvComptaDestinationRepository(args[1]);

            Console.WriteLine($"Extraction de {args[0]} vers {args[1]}...");
            new ExtractCompta(source, destination).Execute();
            Console.WriteLine("Extraction terminée");
        }
    }
}
