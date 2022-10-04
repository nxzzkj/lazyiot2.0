using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Scada.BatchCommand
{
    [Serializable]
    public class BatchCommandTaskGraphAbstract : ISerializable
    {
        public BatchCommandTaskGraphAbstract()
        {
            Shapes = new List<BatchCommandShape>();
        }
        public BatchCommandTask BatchCommandTask { set; get; }
        public List<BatchCommandShape> Shapes { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }
    }
}
