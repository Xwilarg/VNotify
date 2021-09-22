using System;

namespace VNotify.Common
{
    public record Video
    {
        public string id { init; get; }
        public string title { init; get; }
        public DateTime start_scheduled { init; get; }
        public Channel channel { init; get; }
    }
}
