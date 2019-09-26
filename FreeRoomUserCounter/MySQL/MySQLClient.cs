using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace FreeRoomUserCounter.MySQL
{
    class MySQLClient
    {
        private MySqlConnection connection;
        public MySQLClient(string server, string user, string password)
        {
            var sb = new MySqlConnectionStringBuilder()
            {
                Server = server,
                Port = 3306,
                UserID = user,
                Password = password,
                Database = "messages"
            };
            connection = new MySqlConnection(sb.ToString());
            connection.Open();
        }

        ~MySQLClient()
        {
            connection.Close();
        }

        public ulong[] GetUsers(DateTime begin, DateTime end, ulong channelId)
        {
            var l = new List<ulong>();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "SELECT DISTINCT user FROM guild_messages WHERE (channel = @channel) AND (time BETWEEN @begin AND @end) AND (is_bot = 0)";
                command.Parameters.AddWithValue("@channel", channelId);
                command.Parameters.AddWithValue("@begin", begin);
                command.Parameters.AddWithValue("@end", end);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        l.Add(reader.GetUInt64("user"));
                    }
                }
            }

            return l.ToArray();
        }
    }
}
