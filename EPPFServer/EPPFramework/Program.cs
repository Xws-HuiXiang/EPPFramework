using System;
using EPPFramework.Network;
using System.Text;
using System.Collections.Generic;

namespace EPPFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerSocket.Instance.Init();

            Console.ReadLine();
        }
    }
}
