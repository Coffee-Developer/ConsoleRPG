using System;
using System.Collections.Generic;

namespace ConsoleRPG
{
    public static class Helpers 
    {
        public delegate string ForEachItemList<T>(T item);

        public static void ValidateOption(Action action) 
        {
            try { action(); } 
            catch (Exception) { DisplayRead("Invalid value !"); }
        }

        public static string ClearDisplayRead(string textToDisplay) 
        {
            Console.Clear();
            return DisplayRead(textToDisplay);
        }

        public static string DisplayRead(string textToDisplay) 
        {
            Console.WriteLine(textToDisplay);
            return Console.ReadLine();
        }
        
        public static string DisplayItemsInList<T>(string text, List<T> items, ForEachItemList<T> textForEachItem)
        {
            Console.Clear();
            Console.WriteLine(text);
            for (int i = 0; i < items.Count; i++) Console.WriteLine($"{i + 1}. {textForEachItem(items[i])}");
            return DisplayRead("-1. Exit\n");
        }
    }
}