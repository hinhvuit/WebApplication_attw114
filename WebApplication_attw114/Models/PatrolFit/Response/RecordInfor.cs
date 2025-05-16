using System;

namespace WebApplication_attw114.Models.PatrolFit.Response
{
    public class RecordInfor
    {
        public int Id { get; set; }
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public string AreaName { get; set; }
        public string FrameName { get; set; }
        public int TypePatrol { get; set; }
        public DateTime DatePatrol { get; set; }
        public string UrlImage { get; set; }
        public int AreaID { get; set; }
    }
}
