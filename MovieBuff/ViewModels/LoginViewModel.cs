using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace MovieBuff.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-posta girmek zorunludur")]
        [EmailAddress(ErrorMessage = "E-posta adresi geçersiz.Lütfen geçerli bir e-posta giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola girmek zorunludur")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Beni Hatırla")]
        public bool RememberMe { get; set; }
      
    }
}
