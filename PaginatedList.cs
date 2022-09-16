 
using Microsoft.EntityFrameworkCore;
 

namespace University_Information_System
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public List<int>? RangePageList { get; set; }

    //start
 

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            var currentPage1 = PageIndex;
            var currentPage2 = PageIndex;
            var rangeBtn = new HashSet<int>();
            int cnt1 = 3, cnt2 = 3;

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

            RangePageList = new List<int>(rangeBtn);
            RangePageList.Sort();

            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}