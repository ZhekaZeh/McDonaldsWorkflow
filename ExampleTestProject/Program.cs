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
            var cook = new Cook(MealTypes.Hamburger, Constants.CookingTimeBurgerMs);
            var cookThread = new Thread(cook.DoWork);
            cookThread.IsBackground = true;
            cookThread.Start();

            int requestedCount;
            do
            {
                Console.WriteLine(@"How many meals to grab? int only. NOTE: New thread will be created.");
                requestedCount = int.Parse(Console.ReadLine());
                if (requestedCount > 0)
                {
                    int count;
                    ThreadPool.QueueUserWorkItem( (object obj) => cook.TryGetMeals(requestedCount, out count));
                    
                }
            } while (requestedCount > 0);

            McDonalds.Instance.EndTheDay();
            //This line should be called by EndTheDay Method
            cook.WaitHandle.Set();

            Console.ReadLine();
        }
    }
}
