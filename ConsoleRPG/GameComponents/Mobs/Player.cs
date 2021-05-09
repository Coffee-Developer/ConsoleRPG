using ConsoleRPG.GameComponents;
using System;
using System.Collections.Generic;

namespace ConsoleRPG.Mobs
{
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
                    xp = Math.Abs(xp - XpNextLevel);
                    levelUp = true;
                    level++;
                    skillPoints++;
                    life = NewLife;
                    GameManager.ClearDisplayRead($"\n{name} got a new level !\nLevel {level} !\n");
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
            if (inventory.Count != 0)
            {
                string option = GameManager.DisplayItemsInList("Select a item to use:\n", inventory, item => $"{item.name}\nDescription: {item.description}\n");
                if (option != "-1")
                {
                    GameManager.ValidateOption(() => {
                        var selectedItem = inventory[int.Parse(option) - 1];
                        selectedItem.Effect(this);
                        inventory.Remove(selectedItem);
                    });
                    goto Start;
                }
            }
            else GameManager.ClearDisplayRead($"{name} has no items !");
        }

        public void Status()
        {
        Start:
            GameManager.ClearDisplayRead($"Name: {name}\n\nLife: {life}\n\nClass: {playerClass}\n\nLevel: {level}\n\nSkill points: {skillPoints}\n\nXp: {Xp}\n\nXp for next level: {XpNextLevel}\n\nStrength: {Strength}\n\nResistence: {Resistence}\n\nMana: {mana}\n\nSpeed: {Speed}\n");
            if (levelUp)
            {
                switch (GameManager.DisplayRead($"\n{name} got a new level !\nLevel {level} !\n\nDo you want to upgrade ?\n\ns / n\n"))
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
                switch (GameManager.ClearDisplayRead($"Add {skillPoints} point(s) to {name}:\n\n1. Strength: {strengthPoints}\n\n2. Resistence: {resistencePoints}\n\n3. Speed: {speedPoints}\n\n4. Mana: {manaPoints}\n"))
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
                        GameManager.DisplayRead("Invalid value !");
                        goto SkillPoints;
                }
            }
            mana = manaPoints * 10;
        }
    }
}