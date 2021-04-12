using ConsoleRPG.GameComponents;
using ConsoleRPG.Mobs;
using System;

namespace ConsoleRPG
{
    internal class Game
    {
        public static void Start(Player player)
        {
            if (player is null) player = GameManager.CreatePlayer();

            Start:
            Console.Clear();
            Console.WriteLine($"Select an action for {player.name}\n\n1. Explore\n\n2. Go to the market\n\n3. Check inventory\n\n4. Check status\n\n-1. Exit game\n");
            switch (Console.ReadLine())
            {
                case "-1":
                    break;

                case "1":
                    Event.RandomEvent(player);
                    if (GameManager.playerIsDead) break;
                    else goto Start;

                case "2":
                    Market.Menu(player);
                    goto Start;

                case "3":
                    player.Inventory();
                    goto Start;

                case "4":
                    player.Status();
                    goto Start;

                default:
                    Console.WriteLine("Invalid value !");
                    Console.ReadLine();
                    goto Start;
            }
        }

        public static void Continue()
        {
        Start:
            Console.Clear();
            if (GameManager.savedPlayers.Count != 0)
            {
                Console.WriteLine("Select a save to continue:\n");
                for (int i = 0; i < GameManager.savedPlayers.Count; i++) Console.WriteLine($"{i + 1}. {GameManager.savedPlayers[i].name}\n");
                string option = Console.ReadLine();
                if (!option.Equals("-1"))
                {
                    try { Start(GameManager.savedPlayers[int.Parse(option) - 1]); }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid value !");
                        Console.ReadLine();
                        goto Start;
                    }
                }
            }
            else
            {
                Console.WriteLine("No save detected !");
                Console.ReadLine();
            }
        }

        public static void Configs()
        {
        Start:
            Console.Clear();
            Console.WriteLine("1. Change difficulty\n\n2. Change game color\n\n3. Exit\n");
            switch (Console.ReadLine())
            {
                case "1":
                    GameManager.ChangeDifficulty();
                    goto Start;

                case "2":
                    GameManager.ChangeColor();
                    goto Start;

                case "3":
                    break;

                default:
                    Console.WriteLine("Invalid value !");
                    Console.ReadLine();
                    goto Start;
            }
        }
    }
}