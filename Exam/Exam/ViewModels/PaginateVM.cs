using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

namespace Exam.ViewModels
{
    public class PaginateVM<T>
    {
        public List<T> Item { get; set; }
        public decimal TotalPage { get;set; }
        public decimal CurrentPage { get;set;}
    }
}



