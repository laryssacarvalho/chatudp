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
            Console.WriteLine("CHAT UDP\n");

            Console.Write("Porta do servidor: ");
            int portaServidor = Convert.ToInt32(Console.ReadLine());

            UDPSocket servidor = new UDPSocket();
            servidor.Server(portaServidor);

            Console.Write("IP do cliente: ");
            string ipCliente = Console.ReadLine();

            Console.Write("Porta do cliente: ");
            int portaCliente = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\n");            

            UDPSocket cliente = new UDPSocket();
            cliente.Client(ipCliente, portaCliente);

            while (true)
            {
                string mensagem = Console.ReadLine();
                cliente.Send(mensagem);
                Console.ReadKey();
            }
        }
    }
}
