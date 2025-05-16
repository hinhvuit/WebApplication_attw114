namespace WebApplication_attw114.Models.PatrolFit.DTO
{
    public class PatrolCheckedDTO
    {
        public int PointCheckedID { get; set; }
        public int RuleID { get; set; }
        public string ImageName { get; set; }
        public string Memo { get; set; }
        public bool IsOk { get; set; }
    }
    public class PatrolChecked
    {
        public int RuleID { get; set; }
        public string ImageName { get; set; }
        public string Memo { get; set; }
        public bool IsOk { get; set; }
    }
}
