using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsSetUp.Features.Filters
{
    public class FiltersListViewModel
    {
        public IEnumerable<string> Categories { get; set; }
        public IEnumerable<string> Statuses { get; set; }

    }
}
