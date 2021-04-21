using ConsoleRPG.GameComponents;
using System;
using System.Collections.Generic;

namespace ConsoleRPG.Mobs
{
    internal enum Classes
    { Warrior = 1, Wizzard, Archer }

    internal enum AttackTypes { LightAttack, HeavyAttack }

    internal enum LightAttacks
    { Blade_attack, Spell_attack, Bow_attack }

    internal enum HeavyAttacks
    { Blade_storm, Strong_wind, Long_bow }

    internal class Player : Mob
    {
        #region Properties field

        private bool levelUp = false;
        public int XpNextLevel => 50 * level * difficultyFactor;
        public int skillPoints = 0, manaPoints, mana;
        public List<Item> inventory = new();
        public Classes playerClass;
        public LightAttacks lightAttack;
        public HeavyAttacks heavyAttack;

        public override float Xp
        {
            set
            {
                xp = value;
                if (xp >= XpNextLevel)
                {
                    levelUp = true;
                    level++;
                    skillPoints++;
                    xp = Math.Abs(xp - XpNextLevel);
                    life = NewLife;
                    Console.WriteLine($"\n{name} got a new level !\nLevel {level} !\n");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
            get => xp;
        }

        #endregion Properties field

        public Player(Classes playerClass, LightAttacks lightAttack, HeavyAttacks heavyAttack, int level, string name, int strengthPoints, int resistencePoints, int speedPoints, int manaPoints) : base(level, name, strengthPoints, resistencePoints, speedPoints)
        {
            difficultyFactor = GameManager.DifficultyFactor;
            mana = manaPoints * 10;
            this.manaPoints += manaPoints;
            this.playerClass = playerClass;
            this.heavyAttack = heavyAttack;
            this.lightAttack = lightAttack;
        }

        public void Inventory()
        {
        Start:
            Console.Clear();
            if (inventory.Count != 0)
            {
                for (int i = 0; i < inventory.Count; i++) Console.WriteLine($"{i + 1}. {inventory[i].name}\nDescription: {inventory[i].description}\n");
                Console.WriteLine("-1. Exit\n");
                string option = Console.ReadLine();
                if (option != "-1")
                {
                    try
                    {
                        var selectedItem = inventory[int.Parse(option) - 1];
                        selectedItem.Effect(this);
                        inventory.Remove(selectedItem);
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
                Console.WriteLine($"{name} has no items !");
                Console.ReadLine();
            }
        }

        public void Status()
        {
        Start:
            Console.Clear();
            Console.WriteLine($"Name: {name}\n\nLife: {life}\n\nClass: {playerClass}\n\nLevel: {level}\n\nSkill points: {skillPoints}\n\nXp: {Xp}\n\nXp for next level: {XpNextLevel}\n\nStrength: {Strength}\n\nResistence: {Resistence}\n\nMana: {mana}\n\nSpeed: {Speed}\n");
            if (levelUp)
            {
                Console.WriteLine($"\n{name} got a new level !\nLevel {level} !\n\nDo you want to upgrade ?\n\ns / n\n");
                switch (Console.ReadLine())
                {
                    case "s":
                        Upgrade();
                        break;

                    case "n":
                        break;

                    default:
                        goto Start;
                }
            }
            else Console.ReadLine();
        }

        public void Attack(AttackTypes attack, Enemy enemy)
        {
            switch (attack)
            {
                case AttackTypes.LightAttack:
                    Console.WriteLine($"\n{name} uses {lightAttack.ToString().Replace("_", " ")} !");
                    Attack(enemy, AttackDamage);
                    break;

                case AttackTypes.HeavyAttack:
                    mana -= 5 * difficultyFactor;
                    Console.WriteLine($"\n{name} uses {heavyAttack.ToString().Replace("_", " ")} !");
                    Attack(enemy, AttackDamage + 5);
                    break;
            }
            Console.ReadLine();
        }

        public bool TryRunAway(int EnemySpeed) => EnemySpeed < Speed;

        private void Upgrade()
        {
            for (; skillPoints != 0; skillPoints--)
            {
                levelUp = false;
            SkillPoints:
                Console.Clear();
                Console.WriteLine($"Add {skillPoints} point(s) to {name}:\n\n1. Strength: {strengthPoints}\n\n2. Resistence: {resistencePoints}\n\n3. Speed: {speedPoints}\n\n4. Mana: {manaPoints}\n");
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
            mana = manaPoints * 10;
        }
    }
}