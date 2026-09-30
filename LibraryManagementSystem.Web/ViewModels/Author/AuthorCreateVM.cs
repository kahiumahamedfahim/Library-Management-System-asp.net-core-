using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.Author
{
    public class AuthorCreateVM
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(3000)]
        public string? Bio { get; set; }
    }
}
