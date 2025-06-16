using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Domain.Resource.Base
{
    public class PaginatedResource
    {
        public string? SortFieldId { get; set; }
        public string? SortOrder { get; set; }
        public int Page { get; set; }
        public int QuantityRecords { get; set; }
    }
}
