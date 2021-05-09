namespace ConsoleRPG.GameComponents
{
    internal enum Items { Xp_potion, HP_potion, Xp_flask, HP_flask, Elixir, Booster, Estus_flask, Vigorite, Lerite, Mermel, Latus_potion }

    internal enum Events { Battle, Nothing, Items }

    internal enum Difficulties { Easy = 1, Medium, Hard }

    internal enum Enemys { Zombie, Skeleton, Slime, Dragon, Burned, Iceman }

    internal enum Classes { Warrior = 1, Wizzard, Archer }

    internal enum AttackTypes { LightAttack, HeavyAttack }

    internal enum LightAttacks { Blade_attack, Spell_attack, Bow_attack }

    internal enum HeavyAttacks { Blade_storm, Strong_wind, Long_bow }
}