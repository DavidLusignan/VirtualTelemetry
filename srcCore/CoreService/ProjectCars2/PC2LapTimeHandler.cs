using CoreService.Data;
using CoreService.Storage;
using CoreService.Storage.DTOs;
using Global.Enumerable;
using Global.Observable;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreService {
    public class PC2LapTimeHandler {
        private object _stateLock = new object();
        public CollectionStore<ParticipantLapTimesDTO> lapTimes { get; private set; }
        public PC2LapTimeHandler(IObservable<DataState> packetHandler, LiteDatabase db) {
            lapTimes = new CollectionStore<ParticipantLapTimesDTO>(db);
            packetHandler.Subscribe(new Observer<DataState>(OnState));
        }

        private void OnState(DataState newState) {
            lock(_stateLock) {
                try {
                    if (newState.dataType.Equals(DataStateType.Time)) {
                        var time = (TimeState)newState;
                        ParticipantLapTimes current;
                        if (lapTimes.ExistsWhere(l => l.participantIndex.Equals(time.participantIndex))) {
                            current = lapTimes.FindWhere(l => l.participantIndex.Equals(time.participantIndex)).Entity();
                        } else {
                            current = new ParticipantLapTimes(Key.Create(), time.participantIndex, new Dictionary<int, ParticipantLapTime>());
                        }
                        var updated = current.InsertIfNewTime(time);
                        lapTimes.Update(current.DTO());
                    }
                } catch (Exception e) {
                    Console.WriteLine("Error while updating lap times");
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
