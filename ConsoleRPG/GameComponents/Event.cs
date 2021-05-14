using ConsoleRPG.Mobs;

namespace ConsoleRPG.GameComponents
{
    internal static class Event
    {
        public static void RandomEvent(Player player)
        {
            switch ((Events)GameManager.rand.Next(3))
            {
                case Events.Battle:
                    Battle(player);
                    break;

                case Events.Nothing:
                    Nothing(player.name);
                    break;

                case Events.Items:
                    Items(player);
                    break;
            }
        }

        private static void Battle(Player player)
        {
            var enemy = GameManager.GenerateEnemy(player);
            Helpers.DisplayRead($"{player.name} found an enemy, {enemy.name} !");

        Start:
            switch (Helpers.ClearDisplayRead($"{enemy.name}:\nLife: {enemy.life}\nLevel: {enemy.level}\nAttack damage: {enemy.AttackDamage}\nStrength: {enemy.Strength}\nResistence: {enemy.Resistence}\nSpeed: {enemy.Speed}\n\n" +
                $"{player.name}:\nLife: {player.life}\nLevel: {player.level}\nAttack damage: {player.AttackDamage}\nStrength: {player.Strength}\nResistence: {player.Resistence}\nSpeed: {player.Speed}\nMana: {player.mana}\n\n" +
                $"1. Light attack ({player.lightAttack.ToString().Replace("_", "")})  2. Heavy attack ({player.heavyAttack.ToString().Replace("_", " ")})  3. Run away  4. Check inventory\n"))
            {
                case "1":
                    player.Attack(AttackTypes.LightAttack, enemy);
                    break;

                case "2":
                    if (player.mana - 10 <= 0)
                    {
                        Helpers.DisplayRead($"{player.name} has no mana !");
                        goto Start;
                    }
                    else player.Attack(AttackTypes.HeavyAttack, enemy);
                    break;

                case "3":
                    if (player.TryRunAway(enemy.Speed))
                    {
                        Helpers.DisplayRead($"{player.name} ran away !");
                        goto End;
                    }
                    else Helpers.DisplayRead($"{player.name} failed to run away !");
                    break;

                case "4":
                    player.Inventory();
                    goto Start;

                default:
                    Helpers.DisplayRead("Invalid value !");
                    goto Start;
            }

            System.Console.Clear();
            if (enemy.life <= 0) GameManager.EnemyKilled(player, enemy);
            else
            {
                enemy.EnemyAttack(player);
                if (player.life <= 0) GameManager.GameOver(player, enemy.name);
                else goto Start;
            }
        End:;
        }

        private static void Items(Player player)
        {
            var itemsFound = GameManager.GenerateItemList(1, 4);
            Helpers.DisplayItemsInList($"{player.name} found {itemsFound.Count} item(s) !\n", itemsFound, item => $"{item.name}\nPrice: {item.price}\nDescription: {item.description}\n");
            player.inventory.AddRange(itemsFound);
        }

        private static void Nothing(string playerName) => Helpers.ClearDisplayRead($"{playerName} found nothing !");
    }
}