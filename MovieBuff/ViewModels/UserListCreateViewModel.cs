using System.ComponentModel.DataAnnotations;
namespace MovieBuff.ViewModels
{
    public class UserListCreateViewModel
    {
        [Required(ErrorMessage = "Liste adı boş bırakılamaz.")]
        [StringLength(100)]
        [Display(Name = "Liste Adı")]
        public string Name { get; set; }

        [StringLength(500)]
        [Display(Name = "Açıklama (İsteğe Bağlı)")]
        public string? Description { get; set; }
    }
}
