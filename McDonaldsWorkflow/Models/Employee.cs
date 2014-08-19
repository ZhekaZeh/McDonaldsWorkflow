using System.Threading;

namespace McDonaldsWorkflow.Models
{
    public class Employee
    {
        #region Private members

        protected readonly AutoResetEvent _waitHandle;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Employee"/> class.
        /// </summary>
        public Employee()
        {
            _waitHandle = new AutoResetEvent(false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the wait handle.
        /// </summary>
        /// <value>
        /// The wait handle.
        /// </value>
        public AutoResetEvent WaitHandle
        {
            get { return _waitHandle; }
        }

        #endregion
    }
}
