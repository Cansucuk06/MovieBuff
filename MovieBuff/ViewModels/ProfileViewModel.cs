using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace MovieBuff.ViewModels
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı girmek zorunludur")]
        [StringLength(100,ErrorMessage = "Kullanıcı adı 100 harften fazla olmamalıdır.")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "E-posta girmek zorunludur")]
        [EmailAddress(ErrorMessage = "E-posta adresi geçersiz.Lütfen geçerli bir e-posta giriniz.")]
        [Display(Name = "E-posta Adresi")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ülke girmek zorunludur")]
        [StringLength(100, ErrorMessage = "Ülke adı 100 harften fazla olmamalıdır.")]
        [Display(Name ="Ülke")]
        public string? Country { get; set; }

        [Display(Name ="Profil Resmi")]
        public IFormFile? ProfilePictureFile { get; set; }

        public string? ProfilePictureUrl { get; set; }
    }
}
