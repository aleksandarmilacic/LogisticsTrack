using System;
using System.Collections.Generic;
using System.Text;

namespace LogisticsTrack.Domain.BaseEntities
{
    public interface ISortableEntity
    {
        int SortingIndex { get; set; }
    }
}
