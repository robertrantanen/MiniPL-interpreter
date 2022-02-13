using System;
using System.Collections.Generic;

//mcs *.cs -out:minipl.exe
//mono minipl.exe

namespace MiniPl
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("enter filename, or empty to exit");

                string text = Console.ReadLine();

                if (text == "")
                {
                    break;
                }

                if (System.IO.File.Exists(text))
                {

                    string file = System.IO.File.ReadAllText(text);

                    //Console.WriteLine(file);

                    Scanner scanner = new Scanner();
                    List<Token> tokens = scanner.scan(file);
                    Console.WriteLine(tokens.Count);

                    Console.WriteLine("");
                    foreach (var token in tokens)
                    {
                        Console.WriteLine(token.GetTokenType()+ " " + token.GetValue() + " " + token.GetLine().ToString());
                    }
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("File not found");
                    Console.WriteLine("");
                }
            }

        }
    }

}