namespace VNotify.Common
{
    public record Channel
    {
        public string id { init; get; }
        public string name { init; get; }
        public string english_name { init; get; }
        public string group { init; get; }
        public string[] top_topics { init; get; }
    }
}
