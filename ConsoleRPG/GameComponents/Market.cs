using ConsoleRPG.Mobs;
using System;
using System.Collections.Generic;

namespace ConsoleRPG.GameComponents
{
    internal class Market
    {
        private static string[] firstNames = { "Gregory's", "Borvis", "Renan's", "Poloris", "Krager's", "Viuma's" }, secondNames = { " Gregocery", " Store", " Storage", " Bar" };

        public static void Menu(Player player)
        {
            string storeName = firstNames[GameManager.rand.Next(firstNames.Length)] + secondNames[GameManager.rand.Next(secondNames.Length)];
            var itemsOnSale = Item.RandomList(1, 6);

        Start:
            Console.Clear();
            Console.WriteLine($"Wellcome to {storeName}, {player.name} !\n\n1. Buy items\n\n2. Sell items\n\n3. Exit\n");
            switch (Console.ReadLine())
            {
                case "1":
                    Buy(player, itemsOnSale);
                    goto Start;

                case "2":
                    Sell(player, itemsOnSale);
                    goto Start;

                case "3":
                    break;

                default:
                    Console.WriteLine("Invalid value !");
                    Console.ReadLine();
                    goto Start;
            }
        }

        private static void Buy(Player player, List<Item> itemsOnSale)
        {
        Start:
            Console.Clear();
            if (itemsOnSale.Count != 0)
            {
                Console.WriteLine($"{player.name} coins: {player.coins}\n");
                for (int i = 0; i < itemsOnSale.Count; i++) Console.WriteLine($"{i + 1}. {itemsOnSale[i].name}\nPrice: {itemsOnSale[i].price}\nDescription: {itemsOnSale[i].description}\n");
                Console.WriteLine("-1. Exit\n");
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

        private static void Sell(Player player, List<Item> itemsOnSale)
        {
        Start:
            Console.Clear();
            if (player.inventory.Count != 0)
            {
                Console.WriteLine($"{player.name} coins: {player.coins}\n");
                for (int i = 0; i < player.inventory.Count; i++) Console.WriteLine($"{i + 1}. {player.inventory[i].name}\nDescription: {player.inventory[i].description}\n");
                Console.WriteLine("-1. Exit\n");
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
    }
}