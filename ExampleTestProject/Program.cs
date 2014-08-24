using System;
using System.Threading;
using McDonaldsWorkflow.Models;

namespace ExampleTestProject
{
    internal class Program
    {
        /// <summary>
        ///     In this example you can create multiple instances
        /// </summary>
        private static void Main()
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

            ConsoleKeyInfo info;

            ThreadPool.QueueUserWorkItem(obj => McDonalds.Instance.StartWork());
            do
            {
                info = Console.ReadKey();
            } while (info.Key != ConsoleKey.Escape);

            McDonalds.Instance.EndTheDay();
            Thread.Sleep(5000);
            Manager.ShowTakings();
            Console.ReadLine();

            #endregion
        }
    }
}