using System.ComponentModel.DataAnnotations;

namespace WebApplication_attw114.Models
{
    public class ModelForTestTable
    {
        public int ID { get; set; }
        [Required(ErrorMessage="Please Enter sdt")]
        public int sdt { get; set; }
        public string Mathe { get; set; }
        public string Thoigian { get; set; }
        public string Diachi { get; set; }
        public string Memo { get; set; }
    }
}
