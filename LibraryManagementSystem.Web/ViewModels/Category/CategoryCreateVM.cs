using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.Category
{
    public class CategoryCreateVM
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
