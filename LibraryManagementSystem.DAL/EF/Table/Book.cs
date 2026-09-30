using System;
using System.Collections.Generic;

namespace DAL.EF.Table;

public partial class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int Quantity { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public int AuthorId { get; set; }

    public int CategoryId { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();

    public virtual Category Category { get; set; } = null!;
}
