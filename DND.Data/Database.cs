using System.Data.SQLite;
using System.IO;

namespace DND.Data
{
    public static class Database
    {
        public static void SetPath(string path)
        {
            Path = path;
        }

        public static string Path { get; private set; }

        public static bool NeedsInstall()
        {
            return !File.Exists(Path);
        }

        public static void Install()
        {
            SQLiteConnection.CreateFile(Path);
            try
            {
                var commands = Resources.GetResourceAsString("install.sqll");
                using (var connection = GetConnection())
                {
                    connection.Open();

                    var cmd = new SQLiteCommand(commands, connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                File.Delete(Path);
                throw;
            }
        }

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection($"Data Source={Path}");
        }

        public static SQLiteConnection OpenConnection()
        {
            var conn = GetConnection();
            conn.Open();
            return conn;
        }
    }
}
