using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Discord;
using Discord.WebSocket;
using MonitoringBot.Util;

namespace FreeRoomUserCounter
{
    class Program
    {
        private ManualResetEvent ManualResetEvent;
        private Counter counter;
        static void Main(string[] args)
        {
            new Program().MainAsync(args).GetAwaiter().GetResult();
        }

        public async Task MainAsync(string[] args)
        {
            
            ManualResetEvent = new ManualResetEvent(false);
            var client = new DiscordSocketClient();
            var res = Parser.Default.ParseArguments<Options>(args);
            if (res.Tag == ParserResultType.NotParsed)
                return;

            var options = ((Parsed<Options>)res).Value;
            counter = new Counter(options, client);
            client.Log += Log;
            client.Ready += Ready;
            await client.LoginAsync(TokenType.Bot, options.Token);
            await client.StartAsync();
            while (!counter.Completed)
            {
                await Task.Delay(1000);
            }
            Debug.Log("end");
        }

        private async Task Ready()
        {
            await counter.Count();
            ManualResetEvent.Set();
        }

        private async Task Log(LogMessage log)
        {
            Debug.Log(log.Message);
        }
    }
}
