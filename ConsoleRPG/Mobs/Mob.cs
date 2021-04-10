using System;

namespace ConsoleRPG.Mobs
{
    internal abstract class Mob<T>
    {
        public int level = 1, coins = 0, strengthPoints, resistencePoints, speedPoints, difficultyFactor;
        public float life;
        public string name;
        protected float xp;
        protected float NewLife => level * 30 + Resistence;
        public int AttackDamage => Strength + 10 * level;
        public int Resistence => resistencePoints * 5;
        public int Speed => speedPoints * 10;
        public int Strength => strengthPoints * 10;
        public int XpNextLevel => 50 * level * difficultyFactor;
        public abstract float Xp { get; set; }

        public Mob(string name, int strengthPoints, int resistencePoints, int speedPoints)
        {
            this.strengthPoints += strengthPoints;
            this.resistencePoints += resistencePoints;
            this.speedPoints += speedPoints;
            this.name = name;
            life = NewLife;
        }

        protected void Attack<D>(D deffenser, int AttackDamage) where D : Mob<D> => deffenser.life -= new Random().Next(AttackDamage, AttackDamage + 5) - deffenser.Resistence;
    }
}