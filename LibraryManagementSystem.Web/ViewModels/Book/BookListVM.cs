namespace Web.ViewModels.Book
{
    public class BookListVM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string AuthorName { get; set; }

        public string CategoryName { get; set; }

        public int Quantity { get; set; }

        public string? ImageUrl { get; set; }
    }
}