

namespace Scada.MDSCore.Client.AppService
{
    /// <summary>
    /// Plug-In applications may derive this class to perform some operations on starting/stopping of MDS.
    /// </summary>
    public abstract class MDSClientApplicationBase : MDSAppServiceBase
    {
        /// <summary>
        /// This method is called on startup of MDS.
        /// </summary>
        public virtual void OnStart()
        {
            //No action
        }

        /// <summary>
        /// This method is called on stopping of MDS.
        /// </summary>
        public virtual void OnStop()
        {
            //No action            
        }
    }
}
