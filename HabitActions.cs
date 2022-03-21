#nullable enable
using System;
using static HabitTracker.Utils;

namespace HabitTracker
{
    public static class HabitActions
    {
        public static void CreateHabit()
        {
            try
            {
                var habitName = PromptForInput("Which new habit would you like to create?: ");
                var habitTime = PromptForInput("How many times did you do the habit?: ");

                if (NameAndTimeIsInvalid(habitName, habitTime)) return;

                using var conn = DbUtils.GenerateConnection();
                var habitExists = DbUtils.CheckIfHabitExists(habitName, conn);

                if (habitExists)
                {
                    Print("Habit already exists...");
                    while (true)
                    {
                        Print("Would you like to update this habit?\n\n1. Yes\n2. No");
                        var choice = PromptForInput("Your choice: ");
                        switch (Convert.ToInt32(choice))
                        {
                            case 1:
                                UpdateHabit(habitName, habitTime);
                                return;
                            case 2:
                                Print("Returning to main menu");
                                return;
                            default:
                                Print("Could not understand command");
                                break;
                        }
                    }
                }

                using var cmd = conn.CreateCommand();
                cmd.CommandText =
                    $"INSERT INTO Habit (Habit_Name, Time_Logged) VALUES ('{habitName}', {habitTime});";
                cmd.ExecuteNonQuery();
                Print($"Habit '{habitName}' was inserted!");
            }
            catch (Exception e)
            {
                Print(ReturnError(e));
            }
        }

        public static void ReadHabits()
        {
            using var conn = DbUtils.GenerateConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Habit";
            try
            {
                var reader = cmd.ExecuteReader();
                Print("\n     Habits     ");
                Print("----------------");
                while (reader.Read())
                {
                    Print($"{reader.GetString(1)} (qty {reader.GetDouble(2)})");
                }
            }
            catch (Exception e)
            {
                Print(ReturnError(e));
            }
        }

        public static void UpdateHabit(string? habitNameArg = null, string? habitTimeArg = null)
        {
            try
            {
                if (habitTimeArg != null && habitNameArg != null)
                {
                    using var conn = DbUtils.UpdateHabitInDb(habitNameArg, habitTimeArg);
                }
                else
                {
                    var habitName = PromptForInput("Which new habit would you like to update?: ");
                    var habitTime = PromptForInput("How many times did you do the habit?: ");

                    if (NameAndTimeIsInvalid(habitName, habitTime)) return;

                    using var conn = DbUtils.UpdateHabitInDb(habitName, habitTime);
                }
            }
            catch (Exception e)
            {
                Print(ReturnError(e));
            }
        }

        public static void DeleteHabit()
        {
            try
            {
                var habitName = PromptForInput("Which habit would you like to delete?: ");

                using var conn = DbUtils.GenerateConnection();
                var habitExists = DbUtils.CheckIfHabitExists(habitName, conn);

                if (habitExists)
                {
                    Print($"Are you sure you want to delete {habitName}?\n1. Yes\n2. No");
                    var choice = PromptForInput("Your choice: ");
                    switch (Convert.ToInt32(choice))
                    {
                        case 1:
                            break;
                        case 2:
                            return;
                    }

                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = $"DELETE FROM Habit WHERE Habit_Name='{habitName}';";
                    cmd.ExecuteNonQuery();
                    Print($"Habit '{habitName}' was deleted!");
                }
                else
                {
                    Print($"Habit with the name '{habitName}' doesn't exist!");
                }
            }
            catch (Exception e)
            {
                Print(ReturnError(e));
            }
        }
    }
}