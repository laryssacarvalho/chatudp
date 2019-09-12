using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Chat
{
    class Program
    {
        static void Main(string[] args)
        {
            string q = "";
            List<Tuple<string, int>> ips = new List<Tuple<string, int>>();

            Console.Write("Digite a porta do servidor: ");
            int porta = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\nDigite os enderecos no seguinte formato IP:PORTA. Para finalizar a lista digite Q\n");
            
            while(q != "Q")
            {
                string end = Console.ReadLine();
                string[] ip = end.Split(':');
                ips.Add(new Tuple<string, int>(ip[0], Convert.ToInt32(ip[1])));
                q = Console.ReadLine();
            }
            Console.Clear();
            UDPSocket servidor = new UDPSocket(ips);
            servidor.Server(porta);
        }
    }
}
