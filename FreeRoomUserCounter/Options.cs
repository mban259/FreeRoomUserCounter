using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace FreeRoomUserCounter
{
    class Options
    {
        [Option('t', "token", Required = true)]
        public string Token { get; set; }

        [Option('f', "freeroomcategory")]
        public IEnumerable<ulong> FreeRoomCategoryIds { get; set; }

        [Option('g', "xpc-jp-id", Required = true)]
        public ulong XPCJPGuildId { get; set; }

        [Option('n', "notification_channel_id", Required = true)]
        public ulong NotificationChannelId { get; set; }

        [Option('s', "server", Required = true)]
        public string Server { get; set; }

        [Option('p', "password", Required = true)]
        public string Password { get; set; }

        [Option('u', "user", Required = true)]
        public string User { get; set; }
    }
}
