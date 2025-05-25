using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Pagination
{
    public class PageResult<T>
    {
        public PageResult()
        {
            
        }
        public PageResult(IEnumerable<T> items,int totalItems, int currentPage,  int pageSize)
        {
            Items = items;
            TotalItems = totalItems;
            TotalPages =(int)Math.Ceiling(totalItems / (double) pageSize ); 
            CurrentPage = currentPage;

        }
        public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
