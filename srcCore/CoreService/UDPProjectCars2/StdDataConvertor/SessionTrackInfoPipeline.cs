using CoreService.Data;
using CoreService.UDPProjectCars2.PacketParser;
using CoreService.UDPProjectCars2.PacketParser.Data;
using Global.Observable;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.UDPProjectCars2.StdDataConvertor {
    public class SessionTrackInfoPipeline : IObservable<SessionTrackInfo> {
        private List<IObserver<SessionTrackInfo>> _observers;
        private SessionState _currentSession { get; set; }
        public SessionTrackInfoPipeline(IObservable<PC2BasePacket> packetHandler, IObservable<SessionState> sessionStates) {
            _observers = new List<IObserver<SessionTrackInfo>>();
            packetHandler.Subscribe(new Observer<PC2BasePacket>(OnState));
            sessionStates.Subscribe(new Observer<SessionState>(OnSession));
        }

        private void OnState(PC2BasePacket newState) {
            try {
                if (newState.baseUDP.packetType.Equals(PC2PacketType.RaceDefinition)) {
                    var raceDef = (PC2RaceDefinition)newState;
                    var trackName = Encoding.UTF8.GetString(raceDef.trackLocation);
                    var trackVariation = Encoding.UTF8.GetString(raceDef.trackVariation);
                    var trackInfo = new SessionTrackInfo(_currentSession.Id, trackName, trackVariation);
                    NotifyAll(trackInfo);
                }
            } catch {

            }
        }

        private void OnSession(SessionState session) {
            _currentSession = session;
        }

        private void NotifyAll(SessionTrackInfo sessionTrackInfo) {
            _observers.ForEach(o => o.OnNext(sessionTrackInfo));
        }

        public IDisposable Subscribe(IObserver<SessionTrackInfo> observer) {
            if (!_observers.Contains(observer)) {
                _observers.Add(observer);
            }
            return new Unsubscriber<SessionTrackInfo>(_observers, observer);
        }
    }
}
