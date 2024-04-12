using BW_Beverages.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BW_Beverages.Data.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> Categories { get;}
    }
}