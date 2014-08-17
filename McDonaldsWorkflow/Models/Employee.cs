using System.Threading;

namespace McDonaldsWorkflow.Models
{
    public class Employee
    {
        #region Private members

        protected readonly ManualResetEvent _waitHandle;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Employee"/> class.
        /// </summary>
        public Employee()
        {
            _waitHandle = new ManualResetEvent(true);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the wait handle.
        /// </summary>
        /// <value>
        /// The wait handle.
        /// </value>
        public ManualResetEvent WaitHandle
        {
            get { return _waitHandle; }
        }

        #endregion
    }
}
