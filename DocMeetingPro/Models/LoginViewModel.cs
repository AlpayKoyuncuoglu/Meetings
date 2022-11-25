using System.ComponentModel.DataAnnotations;

namespace DocMeetingPro.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "username is required")]
        [StringLength(30)]
        public string Username { get; set; }

        [Required(ErrorMessage = "password is required")]
        [MinLength(6, ErrorMessage = "Password can be min 6 charachters")]
        [MaxLength(16, ErrorMessage = "Password can be max 16 charachters")]
        public string Password { get; set; }
    }
}
