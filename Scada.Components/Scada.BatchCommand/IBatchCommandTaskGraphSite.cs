using System.Collections.Generic;

namespace Scada.BatchCommand
{
    public interface IBatchCommandTaskGraphSite
    {
        BatchCommandTask BatchCommandTask { set; get; }
        List<BatchCommandShape> Shapes { get; set; }

    }
}
