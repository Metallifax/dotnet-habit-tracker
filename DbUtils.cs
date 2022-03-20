#nullable enable
using System;
using System.Data.SQLite;
using static HabitTracker.Utils;

namespace HabitTracker
{
    public static class DbUtils
    {
        private const string DbPath = "./database.db";

        public static SQLiteConnection GenerateConnection()
        {
            var conn = new SQLiteConnection($"Data Source={DbPath}");
            conn.Open();

            return conn;
        }

        public static SQLiteConnection UpdateHabitInDb(string? habitName, string? habitTime)
        {
            var conn = GenerateConnection();
            var habitExists = CheckIfHabitExists(habitName, conn);
            if (habitExists)
            {
                try
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText =
                        $"UPDATE Habit SET Time_Logged=Time_Logged+{habitTime} WHERE Habit_Name='{habitName}';";
                    cmd.ExecuteNonQuery();
                    Print($"Habit '{habitName}' was updated!");
                }
                catch (Exception e)
                {
                    Print(ReturnError(e));
                }
            }
            else
            {
                Print($"Habit with the name '{habitName}' doesn't exist!");
            }

            return conn;
        }

        public static bool CheckIfHabitExists(string? habitName, SQLiteConnection conn)
        {
            var habitExistsFlag = false;
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Habit;";

            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(1) == habitName)
                    {
                        habitExistsFlag = true;
                    }
                }
            }
            catch (Exception e)
            {
                Print(ReturnError(e));
            }

            return habitExistsFlag;
        }
    }
}