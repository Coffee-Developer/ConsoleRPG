using ConsoleRPG.Mobs;
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
            switch (Helpers.ClearDisplayRead($"Wellcome to {storeName}, {player.name} !\n\n1. Buy items\n\n2. Sell items\n\n3. Exit\n"))
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
                    Helpers.DisplayRead("Invalid value !");
                    goto Start;
            }
        }

        private static void Buy(Player player)
        {
        Start:
            if (itemsOnSale.Count != 0)
            {
                string option = Helpers.DisplayItemsInList($"{player.name} coins: {player.coins}\n", itemsOnSale, item => $"{item.name}\nPrice: {item.price}\nDescription: {item.description}\n");
                if (option != "-1")
                {
                    Helpers.ValidateOption(() =>
                    {
                        var selectedItem = itemsOnSale[int.Parse(option) - 1];
                        if (player.coins - selectedItem.price < 0) Helpers.DisplayRead("You have no money, come back here when you have more");
                        else
                        {
                            player.coins -= selectedItem.price;
                            player.inventory.Add(selectedItem);
                            itemsOnSale.Remove(selectedItem);
                            GameManager.boughtItems++;
                        }
                    });
                    goto Start;
                }
            }
            else Helpers.ClearDisplayRead("Woah !\nYou bought everything");
        }

        private static void Sell(Player player)
        {
        Start:
            if (player.inventory.Count != 0)
            {
                string option = Helpers.DisplayItemsInList($"{player.name} coins: {player.coins}\n", player.inventory, item => $"{item.name}\nPrice: {item.price}\nDescription: {item.description}\n");
                if (option != "-1")
                {
                    Helpers.ValidateOption(() =>
                    {
                        var selectedItem = player.inventory[int.Parse(option) - 1];
                        player.coins += selectedItem.price;
                        itemsOnSale.Add(selectedItem);
                        player.inventory.Remove(selectedItem);
                        GameManager.soldItems++;
                    });
                    goto Start;
                }
            }
            else Helpers.ClearDisplayRead("Woah !\nYou have no items");
        }
    }
}