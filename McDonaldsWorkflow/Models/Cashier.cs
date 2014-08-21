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
        private Queue<Client> _line;
        private Client _currentClient;
        private readonly List<ICook> _cooks;
     
        #endregion
 
        #region Properties

        public int Id { get; private set; }

        #endregion

        #region Constructor

        public Cashier(int id, List<ICook> cooks):base(String.Format("Cashier #{0}", id))
        {
            Id = id;
            _line = new Queue<Client>();
            _takings = Constants.InitialTakings;
            _cooks = cooks;
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Takes cash from client and add to takings
        /// </summary>
        /// <param name="cash"></param>
        public void GetMoney(int cash)
        {
            var _lock = new object();
            lock (_lock)
            {
                _takings += cash;
            }
            
        }

        /// <summary>
        ///     Tries to gather next client's order
        /// </summary>
        public void TryToGatherOrder()
        {
            _currentClient = _line.Peek();
            do
            {
                int count;
                //TODO: REWORK THE WHOLE METHOD!!!
                if (_cooks[0].TryGetMeals(_currentClient.MealCount, out count)) break;
                _currentClient.MealCount -= count;
                Thread.Sleep(500);
            } while (true);

            _line.Dequeue();
            Console.WriteLine(@"          Client {0} go away!!!", _currentClient.ClientId);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public void StandOnLine(Client client)
        {
            lock (_lockObj)
            {
                _line.Enqueue(client);
            }

            Console.WriteLine(@"Client{0} stand in line.", client.ClientId);
            _waitHandle.Set();
        }

        #endregion

        #region Employee abstract methods implementation

        /// <summary>
        /// Determines whether the table is full or nor.
        /// </summary>
        /// <returns>true if the table is full, false otherwise</returns>
        protected override bool HasSomethingToDo()
        {
            //check if the table is full
            lock (_lockObj)
            {
                return LineCount > 0;
            }
        }

        /// <summary>
        /// Works this instance.
        /// </summary>
        protected override void Work()
        {
            TryToGatherOrder();
            GrabMissingMeals();
        }

        #endregion

        private void GrabMissingMeals()
        {
            //TODO: Implement!!!
        }
    }
}