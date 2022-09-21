using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Printing;

namespace University_Information_System.Models
{
    public class Base
    {
        [NotMapped]
        public int PageSize { get; set; }

        [NotMapped]
        public int TotalPages { get; set; }

        [NotMapped]
        public int PageIndex { get; set; }
        [NotMapped]
        public string? SearchString { get; set; }


        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;


        [NotMapped]
        public List<int>? Pages { get; set; }

        public List<int> GetPages(int count,int pageSize)
        {
            int TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            var pagesVar = new List<int>();
            var rangeBtn = new HashSet<int>();
            int cnt1 = 3, cnt2 = 3;

            var currentPage1 = PageIndex;
            var currentPage2 = PageIndex;
            

            while (currentPage2 <= TotalPages && cnt2 > 0)
            {
                rangeBtn.Add(currentPage2);
                currentPage2++;
                cnt2--;

            }

            while (currentPage1 >= 1 && cnt1 > 0)
            {
                rangeBtn.Add(currentPage1);
                currentPage1--;
                cnt1--;
            }

            while (cnt2 > 0 && currentPage1 >= 1)
            {
                rangeBtn.Add(currentPage1);
                currentPage1--;
                cnt2--;
            }

            while (cnt1 > 0 && currentPage2 <= TotalPages)
            {
                rangeBtn.Add(currentPage2);
                currentPage2++;
                cnt1--;
            }

            pagesVar =  new List<int>(rangeBtn);
            pagesVar.Sort();
            return  pagesVar;
        }
    }
}
