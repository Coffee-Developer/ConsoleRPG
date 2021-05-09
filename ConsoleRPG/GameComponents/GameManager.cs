using ConsoleRPG.Mobs;
using System;
using System.Collections.Generic;

namespace ConsoleRPG.GameComponents
{
    internal static class GameManager
    {
        #region Properties

        public static Difficulties difficulty = Difficulties.Medium;

        public static bool playerIsDead;

        public static Random rand = new();

        public static List<Player> savedPlayers = new();

        public static int DifficultyFactor => (int)difficulty;

        #endregion Properties

        #region Methods

        public static void ChangeColor()
        {
        Start:
            Console.Clear();
            Console.WriteLine($"Current color: {Console.BackgroundColor}\n\nSelect a new color:\n");
            for (int i = 0; i < 12; i++) Console.WriteLine($"{i + 1}. {(ConsoleColor)i}\n");
            string option = Console.ReadLine().Trim();
            if (option != "-1") 
            {
                Helpers.ValidateOption(() => Console.BackgroundColor = (ConsoleColor)int.Parse(option) - 1);
                goto Start;
            }
        }

        public static void ChangeDifficulty()
        {
        Start:
            Console.Clear();
            Console.WriteLine($"Current difficulty: {difficulty}\n\nSelect a new difficulty:\n\n1. Easy\n\n2. Medium\n\n3. Hard\n");
            try { difficulty = (Difficulties)int.Parse(Console.ReadLine().Trim()); }
            catch (Exception)
            {
                Console.WriteLine("Invalid value !");
                Console.ReadLine();
                goto Start;
            }
        }

        public static Player CreatePlayer()
        {
        Start:
            string playerName = GetName();
            Classes playerClass = GetClass(playerName);
            (LightAttacks lightAttack, HeavyAttacks heavyAttack, int strengthPoints, int resistencePoints, int speedPoints, int manaPoints) = GetSkills(playerName, playerClass);

        End:
            switch (Helpers.ClearDisplayRead($"This is your character:\n\nName: {playerName}\nClass: {playerClass}\nLight attack: {lightAttack.ToString().Replace("_", " ")}\nHeavy attack: {heavyAttack.ToString().Replace("_", " ")}\nStrength: {strengthPoints}\nResistence: {resistencePoints}\nSpeed: {speedPoints}\nMana: {manaPoints}\n\n1. Continue |  2. Restart\n"))
            {
                case "1":
                    var player = new Player(playerClass, lightAttack, heavyAttack, 1, playerName, strengthPoints, resistencePoints, speedPoints, manaPoints);
                    savedPlayers.Add(player);
                    return player;

                case "2":
                    goto Start;

                default:
                    Helpers.DisplayRead("Invalid value !");
                    goto End;
            }
        }

        public static void EnemyKilled(Player player, Enemy enemy)
        {
            player.Xp += enemy.Xp;
            player.coins += enemy.coins;
            Helpers.DisplayRead($"{player.name} has killed the {enemy.name} !\n\nXp earned: {enemy.Xp}\n\nCoins: {enemy.coins}\n");
        }

        public static void GameOver(Player player, string enemyName)
        {
            playerIsDead = true;
            savedPlayers.Remove(player);
            Helpers.DisplayRead($"{player.name} was killed by {enemyName} !\n");
        }

        public static Enemy GenerateEnemy(Player player)
        {
            Enemys enemy = (Enemys)rand.Next(6);
            int strengthPoints = 1, resistencePoints = 1, speedPoints = 1, coins = 0, xp = 0;

            switch (enemy)
            {
                case Enemys.Zombie:
                    strengthPoints = CalcPoints(player.difficultyFactor, player.strengthPoints);
                    resistencePoints = CalcPoints(player.difficultyFactor, player.resistencePoints);
                    coins = 10;
                    xp = 20;
                    break;

                case Enemys.Skeleton:
                    strengthPoints = CalcPoints(player.difficultyFactor, player.strengthPoints);
                    speedPoints = CalcPoints(player.difficultyFactor, player.speedPoints);
                    coins = 10;
                    xp = 20;
                    break;

                case Enemys.Slime:
                    speedPoints = CalcPoints(player.difficultyFactor, player.speedPoints);
                    coins = 5;
                    xp = 10;
                    break;

                case Enemys.Dragon:
                    strengthPoints = CalcPoints(player.difficultyFactor, player.strengthPoints);
                    resistencePoints = CalcPoints(player.difficultyFactor, player.resistencePoints);
                    speedPoints = CalcPoints(player.difficultyFactor, player.speedPoints);
                    coins = 15;
                    xp = 25;
                    break;

                case Enemys.Burned:
                    strengthPoints = CalcPoints(player.difficultyFactor, player.strengthPoints);
                    coins = 5;
                    xp = 15;
                    break;

                case Enemys.Iceman:
                    resistencePoints = CalcPoints(player.difficultyFactor, player.resistencePoints);
                    coins = 10;
                    xp = 15;
                    break;
            }

            return new Enemy(xp, coins, enemy.ToString(), player.level, strengthPoints, resistencePoints, speedPoints);
        }

        public static List<Item> GenerateItemList(int min, int max)
        {
            int qtdItems = rand.Next(min, max);
            var items = new List<Item>();
            while (items.Count != qtdItems) items.Add(GenerateItem());
            return items;
        }

        public static void DeleteSave()
        {
        Start:
            if (savedPlayers.Count != 0)
            {
                string option = Helpers.DisplayItemsInList("Select a save to delete or type -1 to exit:\n", savedPlayers, player => $"{player.name}");
                if (!option.Equals("-1"))
                {
                    Helpers.ValidateOption(() => savedPlayers.Remove(savedPlayers[int.Parse(option) - 1]));
                    goto Start;
                }
            }
            else Helpers.ClearDisplayRead("No save detected !");
        }

        private static Item GenerateItem()
        {
            Items item = (Items)new Random().Next(11);
            string description = "";
            int effectLife = 0, effectXp = 0, effectStrength = 0, effectResistence = 0, effectMana = 0, effectSpeed = 0, price = 0;

            switch (item)
            {
                case Items.Xp_potion:
                    effectXp += 25;
                    description = $"Might make you gain a level. Who knows?\nAdds {effectXp} Xp";
                    price = 20;
                    break;

                case Items.HP_potion:
                    effectLife += 20;
                    description = $"Restores {effectLife} Health. Should always have one or two of these on hand.";
                    price = 25;
                    break;

                case Items.Xp_flask:
                    effectXp += 15;
                    description = $"Might make you gain a level. Who knows?\nAdds {effectXp} Xp";
                    price = 10;
                    break;

                case Items.HP_flask:
                    effectLife += 10;
                    description = $"Restores {effectLife} Health. Should always have two or four of these on hand.";
                    price = 15;
                    break;

                case Items.Elixir:
                    effectMana++;
                    description = $"A flask in the form of a 250 ml triangular shield, with a pale white liquid, aroma and flavors with a slightly leavened tone.\nAdds {effectMana} Mana points";
                    price = 35;
                    break;

                case Items.Booster:
                    effectSpeed++;
                    description = $"A kind of potion created by wizzards a long time ago.\nAdds {effectSpeed} speed points";
                    price = 35;
                    break;

                case Items.Estus_flask:
                    effectResistence++;
                    description = $"The Undead treasure these dull green flasks.\nAdds {effectResistence} resistence points";
                    price = 35;
                    break;

                case Items.Vigorite:
                    effectStrength++;
                    description = $"A bottle with 50mg of vigoritin and Elixir mixed\nAdds {effectStrength} strength points";
                    price = 35;                   
                    break;

                case Items.Lerite:
                    effectLife += 10;
                    effectResistence++;
                    description = $"A small recipient filled with dragon blood and human blood\nAdds {effectLife} health and {effectResistence} resistence points";
                    price = 40;                    
                    break;

                case Items.Mermel:
                    effectSpeed++;
                    effectStrength++;
                    description = $"A bottle with Elixir, Vigorite and.... melon !?\nAdds {effectSpeed} speed points and {effectStrength} strenght points";
                    price = 40;                   
                    break;

                case Items.Latus_potion:
                    effectMana++;
                    effectXp += 10;
                    description = $"A simple flask that contains a rare fluid called latus\nAdds {effectMana} mana points and {effectXp} Xp";
                    price = 40;                    
                    break;
            }

            return new Item(item.ToString().Replace("_", " "), description, effectLife, effectXp, effectStrength, effectResistence, effectMana, effectSpeed, price);
        }

        private static int CalcPoints(int difficultyFactor, int points) => rand.Next(difficultyFactor, points + 1 * difficultyFactor);

        private static Classes GetClass(string playerName)
        {
        Class:
            Console.Clear();
            Console.WriteLine($"Select a class for {playerName}:\n\n1. Warrior\nBonus: +1 Strength, +1 Resistence\n\n2. Wizzard\nBonus: +1 Mana, +1 Resistence\n\n3. Archer\nBonus: +1 Speed, +1 Mana\n");
            try { return (Classes)int.Parse(Console.ReadLine()); }
            catch (Exception)
            {
                Console.WriteLine("Invalid value !");
                Console.ReadLine();
                goto Class;
            }
        }

        private static string GetName() => Helpers.ClearDisplayRead("Type your character name: ").Trim();

        private static (LightAttacks, HeavyAttacks, int, int, int, int) GetSkills(string playerName, Classes playerClass)
        {
            LightAttacks lightAttack = default;
            HeavyAttacks heavyAttack = default;
            int strengthPoints = 1, resistencePoints = 1, speedPoints = 1, manaPoints = 1;

            switch (playerClass)
            {
                case Classes.Warrior:
                    strengthPoints++;
                    resistencePoints++;
                    lightAttack = LightAttacks.Blade_attack;
                    heavyAttack = HeavyAttacks.Blade_storm;
                    break;

                case Classes.Wizzard:
                    manaPoints++;
                    resistencePoints++;
                    lightAttack = LightAttacks.Spell_attack;
                    heavyAttack = HeavyAttacks.Strong_wind;
                    break;

                case Classes.Archer:
                    speedPoints++;
                    manaPoints++;
                    lightAttack = LightAttacks.Bow_attack;
                    heavyAttack = HeavyAttacks.Long_bow;
                    break;
            }

            for (int i = 3; i != 0; i--)
            {
            SkillPoints:
                switch (Helpers.ClearDisplayRead($"Add {i} point(s) to {playerName}:\n\n1. Strength: {strengthPoints}\n\n2. Resistence: {resistencePoints}\n\n3. Speed: {speedPoints}\n\n4. Mana: {manaPoints}\n"))
                {
                    case "1":
                        strengthPoints++;
                        break;

                    case "2":
                        resistencePoints++;
                        break;

                    case "3":
                        speedPoints++;
                        break;

                    case "4":
                        manaPoints++;
                        break;

                    default:
                        Helpers.DisplayRead("Invalid value !");
                        goto SkillPoints;
                }
            }

            return (lightAttack, heavyAttack, strengthPoints, resistencePoints, speedPoints, manaPoints);
        }

        #endregion Methods
    }
}