using System;
using System.Threading;
using McDonaldsWorkflow.Models;

namespace ExampleTestProject
{
    class Program
    {
        /// <summary>
        /// In this example you can create multiple instances
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            #region Test cook

            //var cook = new Cook(MealTypes.Hamburger, Constants.CookingTimeHamburgerMs);
            //var cookThread = new Thread(cook.DoWork) {IsBackground = true};
            //cookThread.Start();

            //int requestedCount;
            //do
            //{
            //    Console.WriteLine(@"How many meals to grab? int only. NOTE: New thread will be created.");
            //    requestedCount = int.Parse(Console.ReadLine());
            //    if (requestedCount > 0)
            //    {
            //        int count;
            //        ThreadPool.QueueUserWorkItem((object obj) => cook.TryGetMeals(requestedCount, out count));

            //    }
            //} while (requestedCount > 0);

            //McDonalds.Instance.EndTheDay();


            ////This line should be called by EndTheDay Method
            //cook.WaitHandle.Set();

            #endregion

            #region Test_2

            ThreadPool.QueueUserWorkItem(obj => McDonalds.Instance.StartWork());
            Console.ReadLine();
            McDonalds.Instance.EndTheDay();

            Console.ReadLine();

            #endregion
        }
    }
}
