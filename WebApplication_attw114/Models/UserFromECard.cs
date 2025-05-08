using System;

namespace WebApplication_attw114.Models
{
    public class UserFromECard
    {
        public UserFromECard() { }
        public int ID { get; set; }
        public string Card_No { get; set; }
        public string Emp_Name { get; set; }
        public string Emp_No { get; set; }
        public string Photo { get; set; }
        public string Types { get; set; }
        public string Dept { get; set; }
        public string GRP { get; set; }
        public string TOTALNAME { get; set; }
        public string BirthDay { get; set; }
        public string DateIn { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateTime { get; set; }
        public string IDNo { get; set; }
        public int BGID { get; set; }
        public int IsDeleted { get; set; }
        public string BBNAME { get; set; }
        public string Avartar { get; set; }
    }
}