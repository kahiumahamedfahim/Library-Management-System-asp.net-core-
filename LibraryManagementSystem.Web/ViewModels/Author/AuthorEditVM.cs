using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.Author
{
    public class AuthorEditVM
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(3000)]
        public string? Bio { get; set; }
    }
}
