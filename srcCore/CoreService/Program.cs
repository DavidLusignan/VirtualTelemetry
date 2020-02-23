using CoreService.Data;
using CoreService.F12018;
using CoreService.ProjectCars2;
using Global.Observable;
using System;
using System.Linq;

namespace CoreService {
    class Program {
        static int viewedParticipant = -1;
        static void Main(string[] args) {
            var cache = new LiveDataCache();
            var handler = new PC2PacketHandler();
            handler.Subscribe(new Observer<DataState>(state => {
                if (state.dataType.Equals(DataStateType.ViewedParticipant)) {
                    viewedParticipant = ((ViewedParticipantIndexState)state).viewedParticipantIndex;
                } else if (state.dataType.Equals(DataStateType.CurrentTime)) {
                    var currentTime = (CurrentTimeState)state;
                    if (currentTime.participantIndex == viewedParticipant) {
                        Console.WriteLine(currentTime.currentTime);
                    }
                }
            }));
            handler.Start();
            while(true){

            }
        }
    }
}
