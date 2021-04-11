using ConsoleRPG.Mobs;
using System;
using System.Collections.Generic;

namespace ConsoleRPG.GameComponents
{
    internal enum Items
    { Xp_potion, HP_potion, Xp_flask, HP_flask, Elixir, Booster, Estus_flask, Vigorite, Lerite, Mermel, Latus_potion }

    internal class Item
    {
        public string name, description;
        public int effectLife, effectXp, effectStrength, effectResistence, effectMana, effectSpeed, price;

        public void Effect(Player player)
        {
            player.life += effectLife;
            player.Xp += effectXp;
            player.strengthPoints += effectStrength;
            player.resistencePoints += effectResistence;
            player.manaPoints += effectMana;
            player.speedPoints += effectSpeed;
        }

        public static List<Item> RandomList(int min, int max)
        {
            int qtdItems = GameManager.rand.Next(min, max);
            var items = new List<Item>();
            for (int i = 0; i < qtdItems; i++) items.Add(Random());
            return items;
        }

        private Item(string name, string description, int effectLife, int effectXp, int effectStrength, int effectResistence, int effectMana, int effectSpeed, int price)
        {
            this.name = name;
            this.description = description;
            this.effectXp = effectXp;
            this.effectLife = effectLife;
            this.effectStrength = effectStrength;
            this.effectResistence = effectResistence;
            this.effectMana = effectMana;
            this.effectSpeed = effectSpeed;
            this.price = price;
        }

        private static Item Random()
        {
            Items item = (Items)new Random().Next(11);
            string description = "";
            int effectLife = 0, effectXp = 0, effectStrength = 0, effectResistence = 0, effectMana = 0, effectSpeed = 0, price = 0;

            switch (item)
            {
                case Items.Xp_potion:
                    description = "";
                    price = 20;
                    effectXp += 25;
                    break;

                case Items.HP_potion:
                    description = "";
                    price = 25;
                    effectLife += 20;
                    break;

                case Items.Xp_flask:
                    description = "";
                    price = 10;
                    effectXp += 15;
                    break;

                case Items.HP_flask:
                    description = "";
                    price = 15;
                    effectLife += 10;
                    break;

                case Items.Elixir:
                    description = "";
                    price = 35;
                    effectMana++;
                    break;

                case Items.Booster:
                    description = "";
                    price = 35;
                    effectSpeed++;
                    break;

                case Items.Estus_flask:
                    description = "";
                    price = 35;
                    effectResistence++;
                    break;

                case Items.Vigorite:
                    description = "";
                    price = 35;
                    effectStrength++;
                    break;

                case Items.Lerite:
                    description = "";
                    price = 40;
                    effectLife += 10;
                    effectResistence++;
                    break;

                case Items.Mermel:
                    description = "";
                    price = 40;
                    effectSpeed++;
                    effectStrength++;
                    break;

                case Items.Latus_potion:
                    description = "";
                    price = 40;
                    effectMana++;
                    effectXp += 10;
                    break;
            }

            return new Item(item.ToString().Replace("_", " "), description, effectLife, effectXp, effectStrength, effectResistence, effectMana, effectSpeed, price);
        }
    }
}