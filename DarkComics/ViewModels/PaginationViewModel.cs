using DarkComics.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkComics.ViewModels
{
    public class PaginationViewModel<T>
         where T : class
    {
        public PaginationViewModel(List<T> products,int pageSize,int pageIndex)
           
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = products.Count();
            Products = products.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList<T>();
            
        }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int PageIndex { get; private set; }
        public int LastPageIndex { get {
                return Convert.ToInt32(Math.Ceiling(TotalCount * 1.0 / PageSize));
            } }
        public IEnumerable<T> Products { get; private set; }

        public string Getpagination(IUrlHelper url,string action)
        {
            if (TotalCount <= PageSize)
                return "";

            StringBuilder pag = new StringBuilder();
            pag.Append("<ul class='pagination justify-content-center'>");

           

            if (PageIndex != 1)
            {
                var link = url.Action(action, values: new
                {
                    PageIndex = this.PageIndex - 1,
                    PageSize = this.PageSize
                });

                pag.Append($"<li class='page-item'><a href='{link}' class='page-link' aria-label='Previous'><span aria-hidden='true'>«</span><span class='sr-only'>Previous</span></a></li>");
            }
            for (int i = 1; i <= LastPageIndex; i++)
            {
                if (((i <= (PageIndex + 2) && i >= (PageIndex - 2))))
                {
                    if (PageIndex == i)
                        pag.Append($"<li class='page-item active'><a class='page-link'>{i}</a></li>");
                    else
                    {
                        var link = url.Action(action, values: new
                        {
                            PageIndex = i,
                            PageSize = this.PageSize
                        });

                        pag.Append($"<li class='page-item'><a href='{link}' class='page-link'>{i}</a></li>");

                    }
                }
            }

            if (PageIndex != LastPageIndex)
            {
                var link = url.Action(action, values: new
                {
                    PageIndex = this.PageIndex + 1,
                    PageSize = this.PageSize
                });

                pag.Append($"<li class='page-item'><a href='{link}' class='page-link' aria-label='Next'><span aria-hidden='true'>»</span><span class='sr-only'>Next</span></a></li>");
            }

            pag.Append("</ul>");
            return pag.ToString();
        }
    }
}
