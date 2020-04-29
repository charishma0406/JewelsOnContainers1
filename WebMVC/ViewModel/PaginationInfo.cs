using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.ViewModels
{
    public class PaginationInfo
    {
        //we are showing wat the total items in my page were
        public long TotalItems { get; set; }
        //how many items per page
        public int ItemsPerPage { get; set; }

        //actual page
        public int ActualPage { get; set; }
        //how many pages are there
        public int TotalPages { get; set; }
        //previous and next
        public string Previous { get; set; }
        public string Next { get; set; }

    }
}
