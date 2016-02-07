using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.SQLite;
using System.Text;
using System.Threading.Tasks;

namespace Encrypter
{
    public class DataSource
    {
        public static SQLiteConnection Connection;

        public static void Connect()
        {
            bool existsBeforeConnection = File.Exists("ApplicationDatabase.db");

            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = "ApplicationDatabase.db";
            builder.Version = 3;
            builder.FailIfMissing = false;

            SQLiteConnection connection = new SQLiteConnection();
            connection.ConnectionString = builder.ConnectionString;
            Connection = connection.OpenAndReturn();

            if (!existsBeforeConnection)
            {
                CreateUsersTable();
            }
        }

        public static void Disconnect()
        {
            Connection.Close();
            SQLiteConnection.ClearAllPools();
        }

        public static void AddUser(User user)
        {
            string password = Convert.ToBase64String(user.Password.Hash);
            string salt = Convert.ToBase64String(user.Password.Salt.ToByteArray());

            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine("INSERT INTO Users VALUES (");
            commandText.AppendLine("'" + user.Username + "',");
            commandText.AppendLine("'" + password + "',");
            commandText.AppendLine("'" + salt + "'");
            commandText.AppendLine(");");

            SQLiteCommand command = new SQLiteCommand();
            command.Connection = Connection;
            command.CommandText = commandText.ToString();
            command.ExecuteNonQuery();
            command.Dispose();
        }

        public static bool UsernameExists(string username)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine("SELECT EXISTS(");
            commandText.AppendLine("SELECT 1");
            commandText.AppendLine("FROM Users");
            commandText.AppendLine("WHERE Username = '" + username + "'");
            commandText.AppendLine(");");

            SQLiteCommand command = new SQLiteCommand();
            command.Connection = Connection;
            command.CommandText = commandText.ToString();

            bool userExists = command.ExecuteScalar().ToString() == "1";
            command.Dispose();

            return userExists;
        }

        public static bool TryGetUser(string username, out User user)
        {
            //-----2---------3---------4---------5---------6---------7---------8---------9
            if (UsernameExists(username))
            {
                StringBuilder commandText = new StringBuilder();
                commandText.AppendLine("SELECT Password, Salt");
                commandText.AppendLine("FROM Users");
                commandText.AppendLine("WHERE Username = '" + username + "';");

                SQLiteCommand command = new SQLiteCommand();
                command.Connection = Connection;
                command.CommandText = commandText.ToString();

                SQLiteDataReader reader = command.ExecuteReader();
                reader.Read();

                string passwordText = reader.GetString(0);
                string saltText = reader.GetString(1);

                byte[] hash = Convert.FromBase64String(passwordText);
                Guid salt = new Guid(Convert.FromBase64String(saltText));

                user = new User();
                user.Username = username;
                user.Password = new Password(hash, salt);

                reader.Close();
                command.Dispose();
                
                return true;
            }
            else
            {
                user = null;
                return false;
            }
        }

        private static void CreateUsersTable()
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine("CREATE TABLE Users(");
            commandText.AppendLine("Username varchar(20) NOT NULL UNIQUE,");
            commandText.AppendLine("Password char(44) NOT NULL UNIQUE,");
            commandText.AppendLine("Salt char(24) NOT NULL UNIQUE");
            commandText.AppendLine(");");

            SQLiteCommand command = new SQLiteCommand();
            command.Connection = Connection;
            command.CommandText = commandText.ToString();
            command.ExecuteNonQuery();
            command.Dispose();
        }
    }
}
