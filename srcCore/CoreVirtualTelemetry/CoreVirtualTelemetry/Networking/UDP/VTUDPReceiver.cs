using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Global.Networking.UDP {
    public class VTUDPReceiver {
        private UdpClient listener;
        private IPEndPoint groupEP;
        private Action<byte[]> onReceive;
        private bool keepRunning;

        public VTUDPReceiver(int listenPort, Action<byte[]> onReceive) {
            listener = new UdpClient(listenPort);
            groupEP = new IPEndPoint(IPAddress.Any, listenPort);
            this.onReceive = onReceive;
        }

        public void Start() {
            keepRunning = true;
            Task.Run(() => {
                while (keepRunning) {
                    try {
                        byte[] bytes = listener.Receive(ref groupEP);
                        Task.Run(() => onReceive(bytes));
                    } catch (Exception e) {
                        Console.WriteLine("An exception was thrown while receiving bytes from VTUDPReceiver");
                        Console.WriteLine(e.Message);
                    }
                }
            });
        }

        public void Stop() {
            keepRunning = false;
            listener.Close();
        }
    }
}
