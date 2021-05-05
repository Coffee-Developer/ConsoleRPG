using ConsoleRPG.Mobs;
using System;
using System.Collections.Generic;

namespace ConsoleRPG.GameComponents
{
    internal static class Market
    {
        private static List<Item> itemsOnSale = new();

        private static readonly string[] firstNames = { "Gregory's", "Borvis", "Renan's", "Poloris", "Krager's", "Viuma's" }, secondNames = { " Gregocery", " Store", " Storage", " Bar" };

        public static void Menu(Player player)
        {
            itemsOnSale = GameManager.GenerateItemList(1, 6);
            string storeName = firstNames[GameManager.rand.Next(firstNames.Length)] + secondNames[GameManager.rand.Next(secondNames.Length)];

        Start:
            Console.Clear();
            Console.WriteLine($"Wellcome to {storeName}, {player.name} !\n\n1. Buy items\n\n2. Sell items\n\n3. Exit\n");
            switch (Console.ReadLine())
            {
                case "1":
                    Buy(player);
                    goto Start;

                case "2":
                    Sell(player);
                    goto Start;

                case "3":
                    break;

                default:
                    Console.WriteLine("Invalid value !");
                    Console.ReadLine();
                    goto Start;
            }
        }

        private static void Buy(Player player)
        {
        Start:
            Console.Clear();
            if (itemsOnSale.Count != 0)
            {
                DisplayInfo(player.name, player.coins, itemsOnSale);
                string option = Console.ReadLine();
                if (option != "-1")
                {
                    try
                    {
                        var selectedItem = itemsOnSale[int.Parse(option) - 1];
                        if (player.coins - selectedItem.price < 0)
                        {
                            Console.WriteLine("You have no money, come back here when you have more");
                            Console.ReadLine();
                        }
                        else
                        {
                            player.coins -= selectedItem.price;
                            player.inventory.Add(selectedItem);
                            itemsOnSale.Remove(selectedItem);
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid value !");
                        Console.ReadLine();
                    }
                    goto Start;
                }
            }
            else
            {
                Console.WriteLine("Woah !\nYou bought everything");
                Console.ReadLine();
            }
        }

        private static void Sell(Player player)
        {
        Start:
            Console.Clear();
            if (player.inventory.Count != 0)
            {
                DisplayInfo(player.name, player.coins, player.inventory);
                string option = Console.ReadLine();
                if (option != "-1")
                {
                    try
                    {
                        var selectedItem = player.inventory[int.Parse(option) - 1];
                        player.coins += selectedItem.price;
                        itemsOnSale.Add(selectedItem);
                        player.inventory.Remove(selectedItem);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid value !");
                        Console.ReadLine();
                    }
                    goto Start;
                }
            }
            else
            {
                Console.WriteLine("Woah !\nYou have no items");
                Console.ReadLine();
            }
        }

        private static void DisplayInfo(string playerName, int playerCoins, List<Item> items)
        {
            Console.WriteLine($"{playerName} coins: {playerCoins}\n");
            for (int i = 0; i < items.Count; i++) Console.WriteLine($"{i + 1}. {items[i].name}\nPrice: {items[i].price}\nDescription: {items[i].description}\n");
            Console.WriteLine("-1. Exit\n");
        }
    }
}