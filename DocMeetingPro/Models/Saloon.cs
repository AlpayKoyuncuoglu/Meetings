using System.ComponentModel.DataAnnotations;

namespace DocMeetingPro.Models
{
    public class Saloon
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "salon adı boş bırakılamaz")]
        public string Name { get; set; }
    }
}
