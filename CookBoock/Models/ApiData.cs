namespace CookBoock.Models
{
    public class Rootobject
    {
        public Result[] results { get; set; }
    }

    public class Result
    {
        public string name { get; set; }
        public Instruction[] instructions { get; set; }
        public Section[] sections { get; set; }
        public string thumbnail_url { get; set; }
        public int id { get; set; }
    }

    public class Instruction
    {
        public string display_text { get; set; }
    }

    public class Section
    {
        public Component[] components { get; set; }
    }

    public class Component
    {
        public string raw_text { get; set; }
    }
}
