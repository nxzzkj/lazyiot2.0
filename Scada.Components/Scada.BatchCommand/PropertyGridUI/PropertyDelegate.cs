using System;
using System.Collections.Generic;
using System.Text;

 


namespace Scada.BatchCommand
{
    public delegate void DataChanged();
    /// <summary>
    ///  
    ///  
    /// </summary>
    [Serializable]
    public delegate void PropertySpecEventHandler(object sender, PropertySpecEventArgs e);
    public delegate void PropertiesInfo(object sender, object[] props);
  
}
