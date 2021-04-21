using ConsoleRPG.Mobs;
using System;

namespace ConsoleRPG.GameComponents
{
    internal enum Items
    { Xp_potion, HP_potion, Xp_flask, HP_flask, Elixir, Booster, Estus_flask, Vigorite, Lerite, Mermel, Latus_potion }

    internal class Item
    {
        public string name, description;
        public int effectLife, effectXp, effectStrength, effectResistence, effectMana, effectSpeed, price;

        public Item(string name, string description, int effectLife, int effectXp, int effectStrength, int effectResistence, int effectMana, int effectSpeed, int price)
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

        public void Effect(Player player)
        {
            if (effectLife != 0) Console.WriteLine($"{player.name}: Life: {player.life} => {player.life += effectLife}\n");

            if (effectXp != 0) Console.WriteLine($"{player.name}: Xp: {player.Xp} => {player.Xp += effectXp}\n");

            if (effectStrength != 0) Console.WriteLine($"{player.name}: Strength points: {player.strengthPoints} => {player.strengthPoints += effectStrength}\n");

            if (effectResistence != 0) Console.WriteLine($"{player.name}: Resistence points: {player.resistencePoints} => {player.resistencePoints += effectResistence}\n");

            if (effectMana != 0) Console.WriteLine($"{player.name}: Resistence points: {player.manaPoints} => {player.manaPoints += effectMana}\n");

            if (effectSpeed != 0) Console.WriteLine($"{player.name}: Resistence points: {player.speedPoints} => {player.speedPoints += effectSpeed}\n");
            Console.ReadLine();
        }
    }
}
