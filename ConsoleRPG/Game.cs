using ConsoleRPG.GameComponents;
using ConsoleRPG.Mobs;

namespace ConsoleRPG
{
    internal static class Game
    {
        public static void Start(Player player)
        {
            GameManager.playerIsDead = false;
            if (player is null) player = GameManager.CreatePlayer();

            Start:
            switch (Helpers.ClearDisplayRead($"Select an action for {player.name}\n\n1. Explore\n\n2. Go to the market\n\n3. Check inventory\n\n4. Check status\n\n-1. Exit game\n"))
            {
                case "-1":
                    break;

                case "1":
                    Event.RandomEvent(player);
                    if (GameManager.playerIsDead) break;
                    else goto Start;

                case "2":
                    Market.Menu(player);
                    goto Start;

                case "3":
                    player.Inventory();
                    goto Start;

                case "4":
                    player.Status();
                    goto Start;

                default:
                    Helpers.DisplayRead("Invalid value !");
                    goto Start;
            }
        }

        public static void Continue()
        {
        Start:
            if (GameManager.savedPlayers.Count != 0)
            {
                string option = Helpers.DisplayItemsInList("Select a save to continue or type -1 to exit:\n", GameManager.savedPlayers, player => $"{player.name}\nDifficulty: {(Difficulties)player.difficultyFactor}\n");
                if (!option.Equals("-1"))
                {
                    Helpers.ValidateOption(() => Start(GameManager.savedPlayers[int.Parse(option) - 1]));
                    goto Start;
                }
            }
            else Helpers.ClearDisplayRead("No save detected !");
        }

        public static void Configs()
        {
        Start:
            switch (Helpers.ClearDisplayRead("1. Change difficulty\n\n2. Change game color\n\n3. Delete a save\n\n4. Exit\n"))
            {
                case "1":
                    GameManager.ChangeDifficulty();
                    goto Start;

                case "2":
                    GameManager.ChangeColor();
                    goto Start;

                case "3":
                    GameManager.DeleteSave();
                    goto Start;

                case "4":
                    break;

                default:
                    Helpers.DisplayRead("Invalid value !");
                    goto Start;
            }
        }
    }
}