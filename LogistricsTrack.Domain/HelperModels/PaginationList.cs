using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsTrack.Domain.HelperModels
{
    public class PaginationList<T> where T : class
    {
        public PaginationList(T data, int total)
        {
            Data = data;
            TotalCount = total;
        }

        public T Data { get; set; }
        public int TotalCount { get; set; }
    }
}
