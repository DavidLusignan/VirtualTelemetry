﻿using Global.Networking.UDP;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.F12018 {
    public class F12018PacketHandler {
        VTUDPReceiver udpReceiver;
        F12018PacketFactory factory;
        const int DEFAULT_PORT = 20777;
        public F12018PacketHandler(Action<F12018Packet> onPacket) {
            factory = new F12018PacketFactory();
            udpReceiver = new VTUDPReceiver(DEFAULT_PORT, b => {
                var packet = factory.CreatePacket(b);
                onPacket(packet);
            });
        }

        public void Start() {
            udpReceiver.Start();
        }

        public void Stop() {
            udpReceiver.Stop();
        }
    }
}