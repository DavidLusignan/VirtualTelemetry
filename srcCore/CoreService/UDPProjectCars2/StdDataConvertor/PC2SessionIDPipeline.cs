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
    public class PC2SessionIDPipeline : IObservable<SessionState> {
        private object _stateLock = new object();
        private List<IObserver<SessionState>> _observers;
        private int _viewedParticipant { get; }
        private SessionState _currentState { get; set; }

        public PC2SessionIDPipeline(int viewedParticipant, IObservable<PC2BasePacket> packetHandler) {
            _viewedParticipant = viewedParticipant;
            _currentState = new SessionState(Key.Create(), SessionType.Invalid, SessionProgress.Invalid);
            _observers = new List<IObserver<SessionState>>();
            packetHandler.Subscribe(new Observer<PC2BasePacket>(OnState));
        }

        private void OnState(PC2BasePacket newState) {
            try {
                var packetType = newState.baseUDP.packetType;
                if (packetType.Equals(PC2PacketType.Timings)) {
                    var timings = (PCars2Timings)newState;
                    OnTiming(timings);
                } else if (packetType.Equals(PC2PacketType.GameState)) {
                    var gameState = (PC2GameStatePacket)newState;
                    OnGameState(gameState);
                }
            } catch {

            }
        }

        private void OnTiming(PCars2Timings timings) {
            var viewed = timings.participants.First(p => p.participantIndex.Equals(_viewedParticipant));
            var convertedProgress = ToSessionProgress(viewed.raceState);
            UpdateIfChanged(convertedProgress);
        }

        private void UpdateIfChanged(SessionProgress sessionProgress) {
            lock(_stateLock) {
                if (!_currentState.SessionProgress.Equals(sessionProgress)) {
                    _currentState = new SessionState(_currentState.SessionID, _currentState.SessionType, sessionProgress);
                    NotifyAll(_currentState);
                }
            }
        }

        private void OnGameState(PC2GameStatePacket gameState) {
            var convertedState = ToSessionType(gameState.sessionState);
            UpdateIfChanged(convertedState);
        }

        private void UpdateIfChanged(SessionType sessionType) {
            lock(_stateLock) {
                if (!_currentState.SessionType.Equals(sessionType)) {
                    _currentState = new SessionState(Key.Create(), sessionType, _currentState.SessionProgress);
                    NotifyAll(_currentState);
                }
            }
        }

        private void NotifyAll(SessionState sessionState) {
            _observers.ForEach(o => o.OnNext(sessionState));
        }

        public IDisposable Subscribe(IObserver<SessionState> observer) {
            if (!_observers.Contains(observer)) {
                _observers.Add(observer);
            }
            return new Unsubscriber<SessionState>(_observers, observer);
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
