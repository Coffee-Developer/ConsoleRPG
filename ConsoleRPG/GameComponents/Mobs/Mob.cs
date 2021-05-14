namespace ConsoleRPG.Mobs
{
    internal abstract class Mob
    {
        #region Properties

        public int level, coins = 0, strengthPoints, resistencePoints, speedPoints, difficultyFactor;
        public float life;
        public string name;
        protected float NewLife => level * 30 + Resistence;
        public int AttackDamage => Strength + 10 * level;
        public int Resistence => resistencePoints * 2;
        public int Speed => speedPoints * 10;
        public int Strength => strengthPoints * 10;
        protected float xp;
        public abstract float Xp { get; set; }

        #endregion

        public Mob(int level, string name, int strengthPoints, int resistencePoints, int speedPoints)
        {
            this.strengthPoints += strengthPoints;
            this.resistencePoints += resistencePoints;
            this.speedPoints += speedPoints;
            this.name = name;
            this.level = level;
            life = NewLife;
        }

        protected bool Attack(Mob deffenser, int AttackDamage, out float damage)
        {
            if (deffenser.Resistence - AttackDamage < 0)
            {
                damage = deffenser.Resistence - AttackDamage;
                deffenser.life += deffenser.Resistence - AttackDamage;
                return true;
            }
            else
            {
                damage = AttackDamage / (1 * difficultyFactor);
                deffenser.life -= AttackDamage / (1 * difficultyFactor);
                return false;
            }
        }
    }
}