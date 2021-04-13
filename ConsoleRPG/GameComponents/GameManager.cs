using ConsoleRPG.Mobs;
using System;
using System.Collections.Generic;

namespace ConsoleRPG.GameComponents
{
    internal enum Difficulties
    { Easy = 1, Medium, Hard }

    internal static class GameManager
    {
        #region Properties

        /// <summary>
        /// Checks if the player is dead or not.
        /// </summary>
        public static bool playerIsDead = false;

        /// <summary>
        /// A normal variable that stores a Random class
        /// </summary>
        public static Random rand = new();

        /// <summary>
        /// A list of all saved players.
        /// </summary>
        public static List<Player> savedPlayers = new();

        /// <summary>
        /// Stores the game difficulty (standard difficulty = medium).
        /// </summary>
        public static Difficulties difficulty = Difficulties.Medium;

        /// <summary>
        /// Stores the game difficulty factor based on game difficulty.
        /// </summary>
        public static int DifficultyFactor => (int)difficulty;

        #endregion Properties 

        #region Methods

        /// <summary>
        /// Enables the difficulty change.
        /// </summary>
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

        /// <summary>
        /// Enables the game color change.
        /// </summary>
        public static void ChangeColor()
        {
        Start:
            Console.Clear();
            Console.WriteLine($"Current color: {Console.BackgroundColor}\n\nSelect a new color:\n");
            for (int i = 0; i < 12; i++) Console.WriteLine($"{i + 1}. {(ConsoleColor)i}\n");
            try { Console.BackgroundColor = (ConsoleColor)int.Parse(Console.ReadLine().Trim()) - 1; }
            catch (Exception)
            {
                Console.WriteLine("Invalid value !");
                Console.ReadLine();
                goto Start;
            }
        }

        /// <summary>
        /// Return a random enemy.
        /// </summary>
        public static Enemy RandomEnemy(Player player)
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

        /// <summary>
        /// Displays the enemy killed action, adds xp to the player and coins.
        /// </summary>
        public static void EnemyKilled(Player player, Enemy enemy)
        {
            player.Xp += enemy.Xp;
            player.coins += enemy.coins;
            Console.WriteLine($"{player.name} has killed the {enemy.name} !\n\nXp earned: {enemy.Xp}\n\nCoins: {enemy.coins}\n");
            Console.ReadLine();
        }

        /// <summary>
        /// Displays the game over action, sets playerIsDead to true and removes the player from saved players list.
        /// </summary>
        public static void GameOver(Player player, string enemyName)
        {
            playerIsDead = true;
            savedPlayers.Remove(player);
            Console.WriteLine($"{player.name} was killed by {enemyName} !\n");
            Console.ReadLine();
        }

        /// <summary>
        /// Enables the player creation menu
        /// </summary>
        /// <returns>Player</returns>
        public static Player CreatePlayer()
        {
        Start:
            string playerName = GetName();
            Classes playerClass = GetClass(playerName);
            (LightAttacks lightAttack, HeavyAttacks heavyAttack, int strengthPoints, int resistencePoints, int speedPoints, int manaPoints) = GetSkills(playerName, playerClass);

        End:
            Console.Clear();
            Console.WriteLine($"This is your character:\n\nName: {playerName}\nClass: {playerClass}\nLight attack: {lightAttack.ToString().Replace("_", " ")}\nHeavy attack: {heavyAttack.ToString().Replace("_", " ")}\nStrength: {strengthPoints}\nResistence: {resistencePoints}\nSpeed: {speedPoints}\nMana: {manaPoints}\n\n1. Continue |  2. Restart\n");
            switch (Console.ReadLine())
            {
                case "1":
                    var player = new Player(playerClass, lightAttack, heavyAttack, 1, playerName, strengthPoints, resistencePoints, speedPoints, manaPoints);
                    savedPlayers.Add(player);
                    return player;

                case "2":
                    goto Start;

                default:
                    Console.WriteLine("Invalid value !");
                    Console.ReadLine();
                    goto End;
            }
        }

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

        private static string GetName()
        {
            Console.Clear();
            Console.Write("Type your character name: ");
            return Console.ReadLine().Trim();
        }

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
                Console.Clear();
                Console.WriteLine($"Add {i} point(s) to {playerName}:\n\n1. Strength: {strengthPoints}\n\n2. Resistence: {resistencePoints}\n\n3. Speed: {speedPoints}\n\n4. Mana: {manaPoints}\n");
                switch (Console.ReadLine())
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
                        Console.WriteLine("Invalid value !");
                        Console.ReadLine();
                        goto SkillPoints;
                }
            }

            return (lightAttack, heavyAttack, strengthPoints, resistencePoints, speedPoints, manaPoints);
        }

        private static int CalcPoints(int difficultyFactor, int points) => rand.Next(difficultyFactor, points + 1 * difficultyFactor);

        #endregion Methods
    }
}