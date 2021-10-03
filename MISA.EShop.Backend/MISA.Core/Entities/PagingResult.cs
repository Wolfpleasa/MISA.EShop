using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    public class PagingResult
    {
        public int TotalRecord { get; set; }

        public int TotalPageNumber { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}
