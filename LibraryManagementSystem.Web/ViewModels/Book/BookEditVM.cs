using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Book
{
    public class BookEditVM
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Range(0, 1000)]
        public int Quantity { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public string? ExistingImageUrl { get; set; }

        public IFormFile? ImageFile { get; set; }

        public List<SelectListItem>? Authors { get; set; }

        public List<SelectListItem>? Categories { get; set; }
    }
}