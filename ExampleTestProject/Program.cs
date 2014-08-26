using System;
using System.Threading;
using McDonaldsWorkflow.Models;

namespace ExampleTestProject
{
    internal class Program
    {
        /// <summary>
        ///     In this example you can create multiple instances.
        /// </summary>
        private static void Main()
        {
            #region Test

            ConsoleKeyInfo info;
            ThreadPool.QueueUserWorkItem(obj => McDonalds.Instance.StartWork());
            do
            {
                info = Console.ReadKey();
            } while (info.Key != ConsoleKey.Escape); 

            McDonalds.Instance.EndTheDay();
            Console.WriteLine(@"App is closing. Press any key to continue.");
            Console.ReadLine();

            #endregion
        }
    }
}