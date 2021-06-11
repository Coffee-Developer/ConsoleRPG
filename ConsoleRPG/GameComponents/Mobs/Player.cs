using ConsoleRPG.GameComponents;
using System.Collections.Generic;

namespace ConsoleRPG.Mobs
{
    internal class Player : Mob
    {
        #region Properties

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
                    xp = System.Math.Abs(xp - XpNextLevel);
                    levelUp = true;
                    level++;
                    skillPoints++;
                    life = NewLife;
                    Helpers.ClearDisplayRead($"\n{name} got a new level !\nLevel {level} !\n");
                }
            }
            get => xp;
        }

        #endregion Properties field

        #region Methods

        public Player(Classes playerClass, LightAttacks lightAttack, HeavyAttacks heavyAttack, int level, string name, int strengthPoints, int resistencePoints, int speedPoints, int manaPoints) : base(level, name, strengthPoints, resistencePoints, speedPoints)
        {
            difficultyFactor = (int)GameManager.difficulty;
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
                string option = Helpers.DisplayItemsInList("Select a item to use:\n", inventory, item => $"{item.name}\nDescription: {item.description}\n");
                if (option != "-1")
                {
                    Helpers.ValidateOption(() =>
                    {
                        var selectedItem = inventory[int.Parse(option) - 1];
                        selectedItem.Effect(this);
                        inventory.Remove(selectedItem);
                        GameManager.usedItems++;
                    });
                    goto Start;
                }
            }
            else Helpers.ClearDisplayRead($"{name} has no items !");
        }

        public void Status()
        {
        Start:
            Helpers.ClearDisplayRead($"Name: {name}\n\nLife: {life}\n\nClass: {playerClass}\n\nLevel: {level}\n\nSkill points: {skillPoints}\n\nXp: {Xp}\n\nXp for next level: {XpNextLevel}\n\nStrength: {Strength}\n\nResistence: {Resistence}\n\nMana: {mana}\n\nSpeed: {Speed}\n");
            if (levelUp)
            {
                switch (Helpers.DisplayRead($"\n{name} got a new level !\nLevel {level} !\n\nDo you want to upgrade ?\n\ns / n\n"))
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
            float damageDone = 0;
            switch (attack)
            {
                case AttackTypes.LightAttack:
                    System.Console.WriteLine($"\n{name} uses {lightAttack.ToString().Replace("_", " ")} !\n");
                    if (!Attack(enemy, AttackDamage, out damageDone)) Helpers.DisplayRead($"The {enemy.name} repelled the attack !");
                    break;

                case AttackTypes.HeavyAttack:
                    mana -= 5 * difficultyFactor;
                    System.Console.WriteLine($"\n{name} uses {heavyAttack.ToString().Replace("_", " ")} !\n");
                    if (!Attack(enemy, AttackDamage + 5, out damageDone)) Helpers.DisplayRead($"The {enemy.name} repelled the attack !");
                    break;
            }
            GameManager.damageDone += damageDone;
            System.Console.ReadLine();
        }

        public bool TryRunAway(int EnemySpeed) => EnemySpeed < Speed;

        private void Upgrade()
        {
            for (; skillPoints != 0; skillPoints--)
            {
                levelUp = false;
            SkillPoints:
                switch (Helpers.ClearDisplayRead($"Add {skillPoints} point(s) to {name}:\n\n1. Strength: {strengthPoints}\n\n2. Resistence: {resistencePoints}\n\n3. Speed: {speedPoints}\n\n4. Mana: {manaPoints}\n"))
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
            mana = manaPoints * 10;
        }

        #endregion
    }
}
