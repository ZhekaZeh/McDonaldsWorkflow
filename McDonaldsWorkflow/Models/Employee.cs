using System;
using System.Threading;
using McDonaldsWorkflow.Models.Interfaces;

namespace McDonaldsWorkflow.Models
{
    public abstract class Employee : IEmployee
    {
        #region Protected members

        protected readonly string _employeeName;
        protected readonly object _endOfDayLocker;
        protected readonly object _lockObj;
        protected readonly AutoResetEvent _waitHandle;
        protected bool _isEndOfDay;

        #endregion

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="Employee" /> class.
        /// </summary>
        protected Employee(string employeeName)
        {
            _waitHandle = new AutoResetEvent(false);
            _isEndOfDay = false;
            _endOfDayLocker = new object();
            _employeeName = employeeName;
            _lockObj = new object();
        }

        #endregion

        #region IEmployee implementation

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is end of day.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is end of day; otherwise, <c>false</c>.
        /// </value>
        public bool IsEndOfDay
        {
            get
            {
                lock (_endOfDayLocker)
                {
                    return _isEndOfDay;
                }
            }

            set
            {
                _waitHandle.Set();

                lock (_endOfDayLocker)
                {
                    _isEndOfDay = value;
                }
            }
        }

        public bool IsFinishedWork { get; set; }

        /// <summary>
        ///     Cook's work logic.
        /// </summary>
        public void DoWork()
        {
            while (!IsEndOfDay)
            {
                if (HasSomethingToDo())
                {
                    Work();
                }
                else
                {
                    Rest();
                }
            }
            FinishedWork();
        }

        #endregion

        #region Private methods

        /// <summary>
        ///     Cook has a rest
        /// </summary>
        private void Rest()
        {
            Console.WriteLine(@"{0} is resting, nothing to do...", _employeeName);
            _waitHandle.WaitOne();
            Console.WriteLine(@"{0} finished resting.", _employeeName);
        }

        #endregion

        #region Abstract methods

        /// <summary>
        ///     Determines whether [has something to do].
        /// </summary>
        /// <returns></returns>
        protected abstract bool HasSomethingToDo();

        /// <summary>
        ///     Works this instance.
        /// </summary>
        protected abstract void Work();

        /// <summary>
        ///     The work was finished.
        /// </summary>
        protected abstract void FinishedWork();

        #endregion
    }
}