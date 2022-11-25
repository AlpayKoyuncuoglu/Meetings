using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocMeetingPro.Models
{
    public class Meeting
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "isim alanı boş bırakılamaz")]
        public string Name { get; set; }
        [Required(ErrorMessage = "başlangıç tarihi boş bırakılamaz")]

        public DateTime StartTime { get; set; }
        [Required(ErrorMessage = "bitiş tarihi boş bırakılamaz")]

        public DateTime EndTime { get; set; }

        public int SaloonId { get; set; }

        [ForeignKey("SaloonId")]
        // eager loading, lazy loading vs vs

        public Saloon Saloon { get; set; }
    }
}
