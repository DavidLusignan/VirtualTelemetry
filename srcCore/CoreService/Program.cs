using CoreService.Data;
using CoreService.F12018;
using Global.Observable;
using System;

namespace CoreService {
    class Program {
        static void Main(string[] args) {
            var handler = new F12018PacketHandler();
            handler.Subscribe(new Observer<DataState>(state => Console.WriteLine(state)));
            handler.Start();
            while(true){

            }
        }
    }
}
