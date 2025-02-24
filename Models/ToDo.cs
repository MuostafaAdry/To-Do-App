using System.ComponentModel.DataAnnotations;

namespace To_Do_List.Models
{
    public class ToDo
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Name ,at lest 3 char")]
        [MaxLength(100)]
        public string Name { get; set; }
        [RegularExpression("^.*\\.(png|jpg|pdf)$")]
        public string? File { get; set; }
        public DateTime DeadLine { get; set; }
    }
}
