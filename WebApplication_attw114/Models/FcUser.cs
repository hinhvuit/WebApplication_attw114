using System.Collections.Generic;
using WebApplication_attw114.Models.PatrolFit.DTO;

namespace WebApplication_attw114.Models
{
    public class FcUser
    {
        public int Id { get; set; }
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
    }
    public class Devices
    {
        public string Code { get; set; }
        public string Location { get; set; }
        public string FactoryName { get; set; }
        public int Id { get; set; }
        public string ZoneName { get; set; }
        public string TypeName { get; set; }
        public int Type { get; set; }
    }
    public class CheckedList
    {
        public int UserID { get; set; }
        public int DeviceID { get; set; }
        public string Memo { get; set; }
    }
    public class RuleCheckedList
    {
        public int RuleID { get; set; }
        public bool IsOk { get; set; }
        public string Memo { get; set; }
        public int IdChecked { get; set; }
    }
    public class RuleCheckedListSign
    {
        public int RuleID { get; set; }
        public bool IsOk { get; set; }
        public string Memo { get; set; }
    }
    public class RuleInfor
    {
        public int Id { get; set; }
        public string RuleName { get; set; }
    }
    public class DevicesSign {
        public int UserID { get; set; }
        public int DeviceID { get; set; }
        public string Memo { get; set; }
        public List<RuleCheckedListSign> ListChecked { get; set; }
    }
}
