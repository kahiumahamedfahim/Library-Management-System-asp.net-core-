using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.Dashboard
{
    public class DashboardDTO
    {
        public int TotalBooks { get; set; }

        public int TotalUsers { get; set; }

        public int TotalBorrowedBooks { get; set; }

        public int PendingBorrows { get; set; }
    }
}
