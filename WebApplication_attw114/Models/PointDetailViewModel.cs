using System;
using System.Collections.Generic;

namespace WebApplication_attw114.Models
{
    public class PointDetailViewModel
    {
        public int IdChecked { get; set; }
        public string NamePoint { get; set; }
        public string StatusSign { get; set; }
        public string SignTime { get; set; }
        public int PointID { get; set; }
        public List<PointDetailRule> ListPointDetail { get; set; }
    }
    public class PointDetailRule
    {
        public string RuleName { get; set; }
        public bool IsOk { get; set; }
        public string Memo { get; set; }
    }
}
