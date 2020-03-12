using CoreService.Data;
using CoreService.Storage;
using CoreService.UDPProjectCars2.PacketParser;
using CoreService.UDPProjectCars2.PacketParser.Data;
using Global.Enumerable;
using Global.Observable;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreService.UDPProjectCars2.StdDataConvertor {
    public class PC2SessionTypePipeline : IObservable<SessionEntry> {
        private object _stateLock = new object();
        private List<IObserver<SessionEntry>> _observers;
        private SessionEntry _currentSession { get; set; }

        public PC2SessionTypePipeline(IObservable<PC2BasePacket> packetHandler) {
            _currentSession = new SessionEntry(Key.Create(), SessionType.Invalid, DateTime.MinValue, null);
            _observers = new List<IObserver<SessionEntry>>();
            packetHandler.Subscribe(new Observer<PC2BasePacket>(OnState));
        }

        private void OnState(PC2BasePacket newState) {
            try {
                var packetType = newState.baseUDP.packetType;
                if (packetType.Equals(PC2PacketType.GameState)) {
                    var gameState = (PC2GameStatePacket)newState;
                    OnGameState(gameState);
                }
            } catch {

            }
        }

        private void OnGameState(PC2GameStatePacket gameState) {
            var convertedState = ToSessionType(gameState.sessionState);
            UpdateIfChanged(convertedState);
        }

        private void UpdateIfChanged(SessionType sessionType) {
            lock(_stateLock) {
                if (!_currentSession.SessionType.Equals(sessionType)) {
                    var previousSession = new SessionEntry(_currentSession.Id, _currentSession.SessionType, _currentSession.Beginning, DateTime.UtcNow);
                    _currentSession = new SessionEntry(Key.Create(), sessionType, DateTime.UtcNow, null);
                    NotifyAll(previousSession);
                    NotifyAll(_currentSession);
                }
            }
        }

        private void NotifyAll(SessionEntry sessionState) {
            _observers.ForEach(o => o.OnNext(sessionState));
        }

        public IDisposable Subscribe(IObserver<SessionEntry> observer) {
            if (!_observers.Contains(observer)) {
                _observers.Add(observer);
            }
            return new Unsubscriber<SessionEntry>(_observers, observer);
        }

        private SessionProgress ToSessionProgress(PC2RaceState raceState) {
            switch (raceState) {
                case PC2RaceState.Invalid:
                    return SessionProgress.Invalid;
                case PC2RaceState.NotStarted:
                    return SessionProgress.NotStarted;
                case PC2RaceState.Racing:
                    return SessionProgress.Racing;
                case PC2RaceState.Finished:
                    return SessionProgress.Finished;
                case PC2RaceState.Disqualified:
                    return SessionProgress.Disqualified;
                case PC2RaceState.Retired:
                    return SessionProgress.Retired;
                case PC2RaceState.DNF:
                    return SessionProgress.DNF;
                default:
                    throw new NotImplementedException("Unhandled case converting PC2RaceState to SessionProgress");
            }
        }

        private SessionType ToSessionType(PC2SessionState sessionState) {
            switch (sessionState) {
                case PC2SessionState.Invalid:
                    return SessionType.Invalid;
                case PC2SessionState.Practice:
                    return SessionType.Practice;
                case PC2SessionState.Test:
                    return SessionType.Test;
                case PC2SessionState.Qualify:
                    return SessionType.Qualification;
                case PC2SessionState.FormationLap:
                    return SessionType.FormationLap;
                case PC2SessionState.Race:
                    return SessionType.Race;
                case PC2SessionState.TimeAttack:
                    return SessionType.TimeAttack;
                default:
                    throw new NotImplementedException("Unhandled case converting PC2SessionState to SessionType");
            }
        }
    }
}
