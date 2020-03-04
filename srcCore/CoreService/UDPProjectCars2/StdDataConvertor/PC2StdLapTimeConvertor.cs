using CoreService.Data;
using CoreService.Storage;
using CoreService.Storage.DTOs;
using CoreService.UDPProjectCars2.PacketParser;
using Global.Enumerable;
using Global.Observable;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreService {
    public class PC2StdLapTimeConvertor {
        private object _stateLock = new object();
        public CollectionStore<ParticipantLapTimesDTO> lapTimes { get; private set; }
        public PC2StdLapTimeConvertor(IObservable<PC2BasePacket> packetHandler, LiteDatabase db) {
            lapTimes = new CollectionStore<ParticipantLapTimesDTO>(db);
            packetHandler.Subscribe(new Observer<PC2BasePacket>(OnState));
        }

        private void OnState(PC2BasePacket newState) {
            lock(_stateLock) {
                try {
                    if (newState.baseUDP.packetType.Equals(PC2PacketType.TimeStats)) {
                        var time = (PCars2TimeStatsData)newState;
                        time.participantStats.ForEach(participant => {
                            ParticipantLapTimes current;
                            if (lapTimes.ExistsWhere(l => l.participantIndex.Equals(participant.participantIndex))) {
                                current = lapTimes.FindWhere(l => l.participantIndex.Equals(participant.participantIndex)).Entity();
                            } else {
                                current = new ParticipantLapTimes(Key.Create(), participant.participantIndex, new Dictionary<int, ParticipantLapTime>());
                            }
                            var updated = current.InsertIfNewTime(participant);
                            lapTimes.Update(current.DTO());
                        });
                    }
                } catch (Exception e) {
                    Console.WriteLine("Error while updating lap times");
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
