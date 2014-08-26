using System;
using System.Threading;
using log4net.Config;
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

            //Configures log4net. 
            XmlConfigurator.Configure(); 

            ConsoleKeyInfo info;
            ThreadPool.QueueUserWorkItem(obj => McDonalds.Instance.StartWork());

            do
            {
                info = Console.ReadKey();
            } while (info.Key != ConsoleKey.Escape); 

            McDonalds.Instance.EndTheDay();
            Console.WriteLine(@"App is closing. Press Enter to continue.");
            Console.ReadLine();

            #endregion
        }
    }
}