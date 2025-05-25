using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Pagination
{
    public class PageResult<T>
    {
<<<<<<< HEAD

        public PageResult(IEnumerable<T> items, int totalItems, int totalPages, int pageSize)
        {
            Items = items;
            TotalItems = totalItems;
            TotalPages = totalPages;
            CurrentPage = (int)Math.Ceiling(totalItems / (double) pageSize );
=======
        public PageResult()
        {
            
        }
        public PageResult(IEnumerable<T> items,int totalItems, int currentPage,  int pageSize)
        {
            Items = items;
            TotalItems = totalItems;
            TotalPages =(int)Math.Ceiling(totalItems / (double) pageSize ); 
            CurrentPage = currentPage;
>>>>>>> origin/dev

        }
        public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
