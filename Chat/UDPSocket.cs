using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat
{
    class UDPSocket
    {
        private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private UdpClient udp = new UdpClient(10000);
        private const int bufSize = 8 * 1024;
        private State state = new State(bufSize);
        private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
        private EndPoint ipReply;
        //private AsyncCallback recv = null;
        Thread receiver, sender, senderReply;
        List<Tuple<string, int>> ips;
        byte[] bytes = new byte[1024];
        
        public UDPSocket(List<Tuple<string, int>> ips)
        {
            this.ips = ips;
        }

        public void Server(int port)
        {
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Any, port));            

            //receiver.IsBackground = true;
            receiver = new Thread(new ThreadStart(Receive));
            sender = new Thread(new ThreadStart(Send));
            senderReply = new Thread(new ThreadStart(SendReply));
            receiver.Start();
            sender.Start();
            senderReply.Start();
            //Receive();

        }

        public void Client(string address, int port)
        {
            _socket.Connect(IPAddress.Parse(address), port);
            //Receive();
        }

        public void Send()
        {
            while (true)
            {
                foreach (Tuple<string, int> ip in ips)
                {
                    Client(ip.Item1, ip.Item2);
                    byte[] data = Encoding.ASCII.GetBytes("Heartbeat request");
                    _socket.Send(data);
                    //_socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
                    //{
                    //    State so = (State)ar.AsyncState;
                    //    int bytes = _socket.EndSend(ar);
                    //}, state);
                    Console.WriteLine("Hearbeat Request enviado para: {0}\n", ip.Item1);
                    Thread.Sleep(2000);
                    
                }
            }
        }

        public void SendReply()
        {
            while (true)
            {
                if (ipReply != null)
                {
                    string[] ip_split = ipReply.ToString().Split(':');
                    Client(ip_split[0], Convert.ToInt32(ip_split[1]));
                    byte[] data = Encoding.ASCII.GetBytes("Heartbeat Reply");
                    _socket.Send(data);

                    //_socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
                    //{
                    //    State so = (State)ar.AsyncState;
                    //    int bytes = _socket.EndSend(ar);
                    //}, state);
                    ipReply = null;
                }
            }
        }

        private void Receive()
        {
            while (true)
            {
                int bytesRec = _socket.Receive(bytes);
                string textReceived = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                IPEndPoint end = _socket.RemoteEndPoint as IPEndPoint;
                Console.WriteLine("{0}: {1}", end.ToString(), textReceived);
                if (textReceived.Contains("request") && ipReply == null)
                {
                    ipReply = end;
                }
                //_socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
                //{
                //    State so = (State)ar.AsyncState;
                //    int bytes = _socket.EndReceiveFrom(ar, ref epFrom);
                //    _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
                //    Console.WriteLine("{0}: {1}", epFrom.ToString(), Encoding.ASCII.GetString(so.buffer, 0, bytes));
                //    //if (Encoding.ASCII.GetString(so.buffer, 0, bytes).Contains("Request"))
                //    //{
                //    //    ipReply = epFrom;
                //    //}
                //}, state);
            }
        }
    }
}
