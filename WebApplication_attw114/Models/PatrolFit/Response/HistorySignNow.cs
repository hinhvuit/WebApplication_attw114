using System.Collections.Generic;

namespace WebApplication_attw114.Models.PatrolFit.Response
{
    public class HistorySignNow
    {
        public int Id { get; set; }
        public string CodePoint { get; set; }
        public string NamePoint { get; set; }
        public string StatusSign { get; set; }
        public string StatusPlace { get; set; }
        public string Memo { get; set; }
        public string WorkDate { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
    public class HistorySignNowList
    {
        public List<HistorySignNow> List { get; set; }
        public int Total { get; set; }
    }
    public class HistorySignID
    {
        public int Id { get; set; }
        public int PointID { get; set; }
        public string CodePoint { get; set; }
        public string NamePoint { get; set; }
        public string StatusSign { get; set; }
        public string StatusPlace { get; set; }
        public int PointCheckedID { get; set; }
        public string WorkDate { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
    public class HistorySignIDList
    {
        public List<HistorySignID> List { get; set; }
        public int Total { get; set; }
    }
}
