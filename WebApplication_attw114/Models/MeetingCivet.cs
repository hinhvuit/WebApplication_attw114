using System;

namespace WebApplication_attw114.Models
{
    public class MeetingCivet
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public string TimeStart { get; set; }
        public string QRImage { get; set; }
        public DateTime TimeApplication { get; set; }
        public string Notes { get; set; }
        public string EndTime { get; set; }
    }
}
