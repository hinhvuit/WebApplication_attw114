using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication_attw114.Models
{
    public class UserRecord
    {
        public UserRecord() { }
        public int IDRecord { get; set; }
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public string Location { get; set; }
        public DateTime TimeRecord { get; set; }
        public int Type { get; set; }
        public string Avarta { get; set; }
        public string QRImage { get; set; }
        public string BuName { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }
}