using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.Category
{
    public class CategoryEditVM
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
