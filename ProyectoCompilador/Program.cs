using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCompilador
{
    class Program
    {
        static void Main(string[] args)
        {
            Scanner scanner = new Scanner(@"C:\Users\Marco\Desktop\codigo.txt");
            scanner.InitScanner();
            List<Token> tokens = scanner.tokens;   
            /**
             * Crear Tabla de simbolos
             */
            if (!scanner.Success)
            {
                Console.WriteLine("Pulsa cualquier enter para terminar...");
                Console.ReadLine();
                return;
            }
            Parser parser = new Parser(@"C:\Users\Marco\Desktop\parser.txt");
            parser.SetAutomata();
            parser.SetGrammar(@"C:\Users\Marco\Desktop\grammar.txt");
            parser.IniciaAnalisis(tokens);
            Console.WriteLine(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())));
            Console.ReadLine();
        }
    }
}
