using System.ComponentModel.DataAnnotations;

namespace DocMeetingPro.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "Kullanıcı Adı", Prompt = "")]
        [Required(ErrorMessage = "username is required")]
        [StringLength(30, ErrorMessage = "username can be max 30 charachters")]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password can be min 6 charachters")]
        [MaxLength(16, ErrorMessage = "Password can be max 16 charachters")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Re-Password is required")]
        [MinLength(6, ErrorMessage = "Re-Password can be min 6 charachters")]
        [MaxLength(16, ErrorMessage = "Re-Password can be max 16 charachters")]
        //[Compare("Password")] bu şekilde yazıldığında nameof kullanılmadığında, ileride property adı değişse bile hata vermez ve kod çalışmaz. Sağlıklı bir yöntem değil
        [Compare(nameof(Password))]
        public string RePassword { get; set; }

    }
}
