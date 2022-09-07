namespace iHome.Core.Models.Database
{
    public class TBills
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int Type { get; set; }
        public int Value { get; set; }
        public string Uuid { get; set; } = "";
    }
}
