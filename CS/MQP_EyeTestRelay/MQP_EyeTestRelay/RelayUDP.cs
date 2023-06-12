using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;

namespace MQP_EyeTestRelay
{
    class RelayUDP
    {
        public int port = 9999;

        public UdpClient listener;
        public IPEndPoint groupEP;

        public Thread receiveThread;

        public SerialPort comPort = null;

        public bool firstAlert = false;
        public bool verbose = false;

        public RelayUDP(int _inPort = 10024, string _comPort = "COM7")
        {
            port = _inPort;

            receiveThread = new Thread(new ThreadStart(Listen));
            receiveThread.IsBackground = true;
            receiveThread.Start();

            try
            {
                if (comPort == null)
                {
                    comPort = new SerialPort(_comPort, 9600);//Set your board COM
                    comPort.Open();

                    if (comPort.IsOpen)
                    {
                        comPort.WriteLine("26,28");
                    }
                }
            }
            catch
            {

            }
        }

        public void Shutdown()
        {
            if (receiveThread != null)
            {
                receiveThread.Abort();
            }
            listener.Close();

            if (comPort != null && comPort.IsOpen)
            {
                comPort.Close();
            }
        }

        public void Listen()
        {
            listener = new UdpClient(port);
            groupEP = new IPEndPoint(IPAddress.Any, port);

            while (true)
            {
                try
                {
                    if (!firstAlert) Console.WriteLine("Waiting for broadcast");

                    firstAlert = true;

                    byte[] bytes = listener.Receive(ref groupEP);

                    if (verbose) Console.WriteLine("Received broadcast from " + groupEP);

                    string msg = Encoding.ASCII.GetString(bytes, 0, bytes.Length);

                    Console.WriteLine(msg);

                    if (comPort != null && comPort.IsOpen && msg != null && msg != "")
                    {
                        comPort.WriteLine(msg);
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    if (verbose) Console.WriteLine(e.Message);
                }
            }
        }
    }
}
