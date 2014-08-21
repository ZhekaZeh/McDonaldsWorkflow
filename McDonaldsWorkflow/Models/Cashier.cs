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
        private readonly object _lockObj;
        private Queue<Client> _line;
        private readonly ICompany _company;
        private Client _currentClient;
        private readonly List<ICook> _cooks;
     
        #endregion
 
        #region Properties

        public bool EndOfDay { get; set; }

        public int Id { get; private set; }

        #endregion

        #region Constructor

        public Cashier(int id, List<ICook> cooks)
        {
            Id = id;
            _lockObj = new object();
            _line = new Queue<Client>();
            _takings = Constants.InitialTakings;
            _company = McDonalds.Instance;
            _cooks = cooks;
        }

        #endregion


        #region Private Methods

        /// <summary>
        ///     Cashier has a rest.
        /// </summary>
        private void Rest()
        {
            Console.WriteLine(@"Cashier №{0} is resting... hasn't any clients.", Id);
            _waitHandle.WaitOne();
            Console.WriteLine(@"Cashier №{0} finished resting.", Id);
        }

        /// <summary>
        ///     Cashier is going home.
        /// </summary>
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
            _currentClient = _line.Dequeue();
            int count;
            _cooks[0].TryGetMeals(_currentClient.MealCount, out count);
            Console.WriteLine(@"Try to get meals...");
            Thread.Sleep(500);
        }

        /// <summary>
        ///     Start Cashier's workflow
        /// </summary>
        public void DoWork()
        {
            while (!_company.IsEndOfDay)
            {
                if (LineCount != 0)
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
                    return _line.Count;
                }
            }
        }

        public void StandOnLine(Client client)
        {
            _line.Enqueue(client);
            _waitHandle.Set();
        }

        #endregion
    }
}