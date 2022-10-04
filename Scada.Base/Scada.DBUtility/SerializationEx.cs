using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Scada.DBUtility
{
    /// <summary>
    /// 由于不断完善的原因，不同版本的序列化会导致一些图件无法读取，因此在此处增加一个判断处理的方法
    /// </summary>
    public abstract  class SerializationEx
    {
        public static bool ExistSerializationName(SerializationInfo info, string name)
        {
            foreach (SerializationEntry entry in info)
            {
                if(name== entry.Name)
                {
                    return true;
                }
                
            }
            return false;
        }
    }
}
