﻿using CoreService.Storage;
using CoreService.Storage.DTOs;
using CoreService.UDPProjectCars2.PacketParser;
using Global.Enumerable;
using Global.Observable;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreService {
    public class PC2StdLapTimePipeline : IObservable<ParticipantLapTimes> {
        private object _stateLock = new object();
        private List<IObserver<ParticipantLapTimes>> observers;
        public CollectionStore<ParticipantLapTimesDTO> lapTimes { get; private set; }
        public PC2StdLapTimePipeline(IObservable<PC2BasePacket> packetHandler, LiteDatabase db) {
            observers = new List<IObserver<ParticipantLapTimes>>();
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
                            var updated = PC2StdLapTimeFactory.InsertIfNewTime(participant, current);
                            lapTimes.Update(current.DTO());
                            NotifyAll(updated);
                        });
                    }
                } catch (Exception e) {
                    
                }
            }
        }
        
        private void NotifyAll(ParticipantLapTimes lapTimes) {
            observers.ForEach(o => o.OnNext(lapTimes));
        }

        public IDisposable Subscribe(IObserver<ParticipantLapTimes> observer) {
            if (!observers.Contains(observer)) {
                observers.Add(observer);
            }
            return new Unsubscriber<ParticipantLapTimes>(observers, observer);
        }
    }

    public static class PC2StdLapTimeFactory {
        public static ParticipantLapTimes InsertIfNewTime(PCars2ParticipantStatsInfo newTime, ParticipantLapTimes currentTimes) {
            if (IsOutLap(newTime)) {
                return currentTimes;
            }
            else if (IsFirstSector(currentTimes)) {
                var toInsert = new ParticipantLapTime(-1, newTime.lastSectorTime, -1, -1);
                var newLapTimes = currentTimes.lapTimes.Concat(0, toInsert);
                return new ParticipantLapTimes(currentTimes.key, currentTimes.participantIndex, newLapTimes);
            }
            else if (IsNewSector1(newTime, currentTimes)) {
                var toInsert = new ParticipantLapTime(-1, newTime.lastSectorTime, -1, -1);
                var newLapTimes = currentTimes.lapTimes.Concat(currentTimes.lapTimes.Keys.Max() + 1, toInsert);
                return new ParticipantLapTimes(currentTimes.key, currentTimes.participantIndex, newLapTimes);
            }
            else if (IsNewSector2(newTime, currentTimes)) {
                var toUpdate = new ParticipantLapTime(-1, CurrentLap(currentTimes).sector1Time, newTime.lastSectorTime, -1);
                var newLapTimes = currentTimes.lapTimes.Except(currentTimes.lapTimes.Keys.Max()).Concat(currentTimes.lapTimes.Keys.Max(), toUpdate);
                return new ParticipantLapTimes(currentTimes.key, currentTimes.participantIndex, newLapTimes);
            } else if (IsNewSector3(newTime, currentTimes)) {
                var toUpdate = new ParticipantLapTime(newTime.lastLapTime, CurrentLap(currentTimes).sector1Time, CurrentLap(currentTimes).sector2Time, newTime.lastSectorTime);
                var newLapTimes = currentTimes.lapTimes.Except(currentTimes.lapTimes.Keys.Max()).Concat(currentTimes.lapTimes.Keys.Max(), toUpdate);
                return new ParticipantLapTimes(currentTimes.key, currentTimes.participantIndex, newLapTimes);
            } else {
                return currentTimes;
            }
        }

        private static ParticipantLapTime CurrentLap(ParticipantLapTimes currentTimes) {
            return currentTimes.lapTimes[currentTimes.lapTimes.Keys.Max()];
        }

        private static bool IsOutLap(PCars2ParticipantStatsInfo time) {
            return time.lastSectorTime < 0;
        }

        private static bool IsFirstSector(ParticipantLapTimes currentTime) {
            return !currentTime.lapTimes.Any();
        }

        private static bool IsNewSector1(PCars2ParticipantStatsInfo time, ParticipantLapTimes currentTime) {
            var lapTimes = currentTime.lapTimes;
            return lapTimes[lapTimes.Keys.Max()].IsComplete && lapTimes[lapTimes.Keys.Max()].sector3Time != time.lastSectorTime;
        }

        private static bool IsNewSector2(PCars2ParticipantStatsInfo time, ParticipantLapTimes currentTime) {
            var lapTimes = currentTime.lapTimes;
            return lapTimes[lapTimes.Keys.Max()].sector1Time > 0 && 
                lapTimes[lapTimes.Keys.Max()].sector2Time <= 0 && 
                lapTimes[lapTimes.Keys.Max()].sector1Time != time.lastSectorTime;
        }

        private static bool IsNewSector3(PCars2ParticipantStatsInfo time, ParticipantLapTimes currentTime) {
            var lapTimes = currentTime.lapTimes;
            return lapTimes[lapTimes.Keys.Max()].sector2Time > 0 && 
                lapTimes[lapTimes.Keys.Max()].sector3Time <= 0 && 
                lapTimes[lapTimes.Keys.Max()].sector2Time != time.lastSectorTime;
        }
    }
}