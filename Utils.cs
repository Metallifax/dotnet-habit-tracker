#nullable enable
using System;

namespace HabitTracker
{
    public static class Utils
    {
        private const string Title = "~ Welcome to Habit Tracker ~";

        private const string Menu = "\nPlease select an activity:\n1. Store a Habit\n" +
                                    "2. Read your Habits\n3. Update a Habit\n4. Delete\n5. Quit";

        public static void AppLoop()
        {
            Print(Title);
            while (true)
            {
                Print(Menu);
                var choice = PromptForInput("Your choice: ");

                switch (Convert.ToInt32(choice))
                {
                    case 1:
                        HabitActions.CreateHabit();
                        break;
                    case 2:
                        HabitActions.ReadHabits();
                        break;
                    case 3:
                        HabitActions.UpdateHabit();
                        break;
                    case 4:
                        HabitActions.DeleteHabit();
                        break;
                    case 5:
                        return;
                    default:
                        Print("Could not understand!");
                        break;
                }
            }
        }

        public static string? PromptForInput(string message)
        {
            try
            {
                Console.Out.Write(message);
                return Console.ReadLine();
            }
            catch (Exception e)
            {
                return ReturnError(e);
            }
        }

        public static string ReturnError(Exception exception)
        {
            return "Oops.... Something went wrong!: " + exception.Message;
        }

        public static void Print(string message)
        {
            Console.Out.WriteLine(message);
        }
    }
}