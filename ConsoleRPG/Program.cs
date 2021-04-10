using System;

namespace ConsoleRPG
{
    internal class Program
    {
        private static void Main()
        {
        Start:
            Console.Clear();
            Console.WriteLine("==========================\nWELLCOME TO MY GAME\n==========================\n\n1. New game\n\n2. Continue\n\n3. Configs\n\n4. Exit\n");
            switch (Console.ReadLine())
            {
                case "1":
                    Game.Start(null);
                    goto Start;

                case "2":
                    Game.Continue();
                    goto Start;

                case "3":
                    Game.Configs();
                    goto Start;

                case "4":
                    break;

                default:
                    Console.WriteLine("Invalid value !");
                    Console.ReadLine();
                    goto Start;
            }
        }
    }
}