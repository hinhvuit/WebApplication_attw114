using System.Collections.Generic;

namespace WebApplication_attw114.Models.PatrolFit.DTO
{
    public class PatrolSignDTO
    {
        public int TypePatrol { get; set; }
        public string code_Point { get; set; }
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
    }
    public class PatrolSignListDTO
    {
        public int TypePatrol { get; set; }
        public string CodePoint { get; set; }
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public List<PatrolChecked> ListChecked { get; set; }
    }
}