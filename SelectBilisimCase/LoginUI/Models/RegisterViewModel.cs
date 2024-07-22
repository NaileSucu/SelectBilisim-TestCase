using System.ComponentModel.DataAnnotations;

namespace LoginUI.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Lütfen adınızı giriniz!")]
        [Display(Name = "Ad: ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Lütfen soyadınızı giriniz!")]
        [Display(Name = "Soyadı: ")]

        public string SurName { get; set; }

        [Display(Name = "E-Mail Adresi: ")]
        [Required(ErrorMessage = "Lütfen mail adresinizi eksiksiz giriniz!")]
        public string Email { get; set; }
        [Display(Name = "Parola: ")]
        [Required(ErrorMessage = "Lütfen şifrenizi giriniz!")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Lütfen şifrenizi tekrar giriniz!")]
        [Display(Name = "Parola Tekrarı: ")]
        [Compare("Password", ErrorMessage = "Şifreler uyumlu değil!")]
        public string ConfirmPassword { get; set; }
    }
}
