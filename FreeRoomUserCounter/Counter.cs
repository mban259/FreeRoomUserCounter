using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using FreeRoomUserCounter.MySQL;
using MonitoringBot.Util;

namespace FreeRoomUserCounter
{
    class Counter
    {
        private DiscordSocketClient Client;
        private Options Option;
        private MySQLClient mySql;
        private SocketGuild XPCJP;

        public bool Completed { get; private set; } 
        public Counter(Options option, DiscordSocketClient client)
        {
            Client = client;
            Option = option;
            mySql = new MySQLClient(option.Server, option.User, option.Password);
            Completed = false;
        }

        public async Task Count()
        {
            Debug.Log("count");
            XPCJP = Client.GetGuild(Option.XPCJPGuildId);
            var now = DateTime.Now;
            var lastWeek = now.AddDays(-7);
            var beforeLastWeek = lastWeek.AddDays(-7);
            var notificationChannel = XPCJP.GetTextChannel(Option.NotificationChannelId);
            var alert = new StringBuilder();
            var bakuha = new StringBuilder();
            foreach (var optionFreeRoomCategoryId in Option.FreeRoomCategoryIds)
            {
                var category = XPCJP.GetCategoryChannel(optionFreeRoomCategoryId);
                Debug.Log($"カテゴリー:{category.Name}");
                foreach (SocketGuildChannel channel in category.Channels)
                {
                    if (channel is SocketTextChannel)
                    {
                        var textChannel = channel as SocketTextChannel;
                        Debug.Log($"チャンネル:{textChannel.Name}");

                        if (Count(lastWeek, now, channel.Id) >= 5)
                        {
                            // なし
                            Debug.Log($"なし {channel.Id}:{channel.Name}");
                            continue;
                        }

                        if (Count(beforeLastWeek, lastWeek, channel.Id) >= 5)
                        {
                            // アラート
                            Debug.Log($"アラート {channel.Id}:{channel.Name}");
                            await textChannel.SendMessageAsync("先週に書き込みをしたユニークユーザーが5人未満でした");
                            alert.AppendLine(textChannel.Mention);
                        }
                        else
                        {
                            // 爆破
                            Debug.Log($"爆破 {channel.Id}:{channel.Name}");
                            await textChannel.SendMessageAsync("先週,先々週に書き込みをしたユニークユーザーが5人未満でした");
                            bakuha.AppendLine(textChannel.Mention);
                        }
                    }
                }
            }

            await notificationChannel.SendMessageAsync($"アラート\n{alert.ToString()}\n爆破\n{bakuha.ToString()}");

            Completed = true;
        }

        private int Count(DateTime begin, DateTime end, ulong channelId)
        {
            var users = mySql.GetUsers(begin, end, channelId);
            Debug.Log(string.Join(" ", users));
            return users.Length;
        }
    }
}