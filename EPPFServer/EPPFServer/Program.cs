using System;
using EPPFServer.Network;

namespace EPPFServer
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
