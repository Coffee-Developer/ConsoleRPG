namespace ConsoleRPG.Mobs
{
    internal class Enemy : Mob
    {
        public Enemy(float xp, int coins, string name, int level, int strengthPoints, int resistencePoints, int speedPoints) : base(level, name, strengthPoints, resistencePoints, speedPoints)
        {
            this.coins = coins;
            this.xp = xp;
        }

        public override float Xp { get => xp; set => xp = value; }

        public void EnemyAttack(Player player)
        {
            if (Attack(player, GameComponents.GameManager.rand.Next(AttackDamage, AttackDamage + 10), out float damageTaken))
                Helpers.DisplayRead($"{name} attacks {player.name}");
            else
                Helpers.DisplayRead($"{player.name} has repelled the attack !");
            GameComponents.GameManager.damageTaken += damageTaken;
        }
    }
}