namespace ConsoleRPG
{
    internal class Program
    {
        private static void Main()
        {
        Start:
            switch (Helpers.ClearDisplayRead("===========================\nWELLCOME TO MY GAME\n==========================\n\n1. New game\n\n2. Continue\n\n3. Configs\n\n4. Exit\n"))
            {
                case "1":
                    Game.Start(null);
                    goto Start;

                case "2":
                    Game.Continue();
                    goto Start;

                case "3":
                    Game.Configs();
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