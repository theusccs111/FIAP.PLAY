using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Application.Shared.Resource
{
    public class ListResource<T>
    {
        public IEnumerable<T> Items { get; set; }
        public long TotalRecords { get; set; }
    }
}
