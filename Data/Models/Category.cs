using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BW_Beverages.Data.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public List<Drink>? Drinks { get; set; }
    }
}