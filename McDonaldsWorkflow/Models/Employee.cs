using System;
using System.Threading;
using McDonaldsWorkflow.Models.Enums;

namespace McDonaldsWorkflow.Models
{
    public abstract class Employee
    {
        #region Protected members

        protected readonly string _employeeName;
        protected readonly object _endOfDayLocker;
        protected readonly object _lockObj;
        protected readonly AutoResetEvent _waitHandle;
        protected EmployeeStates _employeeState;
        protected bool _isEndOfDay;

        #endregion

        #region Properties

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

        /// <summary>
        ///     Gets or sets a value indicating current state of employee.
        /// </summary>
        public EmployeeStates EmployeeState
        {
            get
            {
                lock (_lockObj)
                {
                    return _employeeState;
                }
            }
            protected set
            {
                lock (_lockObj)
                {
                    _employeeState = value;
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="Employee" /> class.
        /// </summary>
        protected Employee(string employeeName)
        {
            _waitHandle = new AutoResetEvent(false);
            _isEndOfDay = false;
            _lockObj = new object();
            _endOfDayLocker = new object();
            _employeeName = employeeName;
        }

        #endregion

        #region Private methods

        /// <summary>
        ///     Cook has a rest.
        /// </summary>
        private void Rest()
        {
            Console.WriteLine(@"{0} is resting, nothing to do...", _employeeName);
            _waitHandle.WaitOne();
            Console.WriteLine(@"{0} finished resting.", _employeeName);
        }

        #endregion

        #region Public methods

        /// <summary>
        ///     Employee's work logic.
        /// </summary>
        public void DoWork()
        {
            while (!IsEndOfDay)
            {
                if (HasSomethingToDo())
                {
                    _employeeState = EmployeeStates.Working;
                    Work();
                }
                else
                {
                    _employeeState = EmployeeStates.Resting;
                    Rest();
                }
            }
            GoHome();
            _employeeState = EmployeeStates.WentHome;
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
        ///     Employee's 'Go home' logic.
        /// </summary>
        protected abstract void GoHome();

        #endregion
    }
}