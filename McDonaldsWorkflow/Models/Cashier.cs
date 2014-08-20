using System;
using System.Collections.Generic;
using System.Threading;
using McDonaldsWorkflow.Models.Interfaces;

namespace McDonaldsWorkflow.Models
{
    public class Cashier : Employee, ICashier
    {

        #region Private fields

        private int _takings;
        private int _lineCount;
        private readonly object _lockObj;
        private Queue<Client> _line;
        private readonly ICompany _company;
        private Client _currentClient;
     
        #endregion
 
        #region Properties

        public bool EndOfDay { get; set; }

        public int Id { get; private set; }

        public Queue<Client> Line
        {
            get { return _line; }
            set
            {
                _line = value;
                _lineCount = Line.Count;
            }
        }

        #endregion

        #region Constructor

        public Cashier(int id)
        {
            Id = id;
            _lockObj = new object();
            _line = new Queue<Client>();
            _lineCount = Line.Count;
            _takings = Constants.InitialTakings;
        }

        #endregion


        #region Private Methods

        /// <summary>
        ///     Cashier has a rest
        /// </summary>
        private void Rest()
        {
            Console.WriteLine(@"Cashier №{0} is resting... hasn't any clients.", Id);
            _waitHandle.WaitOne();
            Console.WriteLine(@"Cashier №{0} finished resting.", Id);
        }

        private void GoHome()
        {
            Console.WriteLine(@"Cashier №{0} is going home. Bye bye", Id);
        }
        #endregion

        #region Public Methods

        public void GetMoney(int cash)
        {
            var _lock = new object();
            lock (_lock)
            {
                _takings += cash;
            }
            
        }

        public void TryToGatherOrder()
        {
            _currentClient = Line.Peek();
            // must be implemented
         
        }

        public void DoWork()
        {
            while (!_company.IsEndOfDay)
            {
                if (Line.Count != 0)
                {
                    TryToGatherOrder();
                }
                else
                {
                    Rest();
                }
            }

            GoHome();
        }

        #endregion

        #region ICashier implementation

        public int Takings
        {
            get
            {
                lock (_lockObj)
                {
                    return _takings;
                }
            }
        }

        public int LineCount
        {
            get
            {
                lock (_lockObj)
                {
                    return _lineCount;
                }
            }
        }

        #endregion
    }
}