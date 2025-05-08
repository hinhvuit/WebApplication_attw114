using System;

namespace WebApplication_attw114.Models
{
    public class UserMember
    {
        public UserMember() { }
        public int UserID { get; set; }
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public string BuName { get; set; }
        public string Avartar { get; set; }
        public int IsDeleted { get; set; }
        public DateTime TimeApplication { get; set; }
        public string QRImage { get; set; }
        public int Status { get; set; }
        public string AvartarUpdate { get; set; }
        public DateTime TimeUpdateAvartar { get; set; }
        public int AreaIDDefault { get; set; }
    }
}
