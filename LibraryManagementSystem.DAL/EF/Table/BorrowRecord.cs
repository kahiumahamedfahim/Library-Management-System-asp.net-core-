using System;
using System.Collections.Generic;

namespace DAL.EF.Table;

public partial class BorrowRecord
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int BookId { get; set; }

    public DateTime BorrowDate { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public decimal FineAmount { get; set; }

    public string Status { get; set; } = null!;

    public virtual Book Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
