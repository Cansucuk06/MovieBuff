using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieBuff.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Lütfen kullanıcı adınızı giriniz.")]
        [StringLength(100,ErrorMessage ="Kullanıcı adı en fazla 100 karakter olabilir")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "E-posta girmek zorunludur")]
        [EmailAddress(ErrorMessage = "E-posta adresi geçersiz.Lütfen geçerli bir e-posta giriniz.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola girmek zorunludur")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name ="Şifre Tekrarı")]
        [Compare("Password", ErrorMessage = "Parolalar uyuşmuyor.")]
        public string ConfirmPassword { get; set; }


        [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir")]
        [Display(Name = "Ülke")]
        public string? Country { get; set; }
    }
}
