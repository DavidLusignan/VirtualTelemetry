

using CoreService.UDPProjectCars2.RawPacketHandler;

namespace CoreService.UDPProjectCars2.PacketParser.Data {
    public class PC2GameStatePacket : PC2BasePacket {
        public ushort buildVersionNumber { get; }
        public PC2GameState gameState { get; }
        public PC2SessionState sessionState { get; }
        public PC2GameStatePacket(PC2PacketMeta baseUDP, ushort buildVersionNumber, PC2GameState gameState, PC2SessionState sessionState) {
            this.baseUDP = baseUDP;
            this.buildVersionNumber = buildVersionNumber;
            this.gameState = gameState;
            this.sessionState = sessionState;
        }

        public static PC2GameStatePacket Create(PC2RawPacket rawPacket, PC2PacketMeta meta) {
            var buildVersionNumber = rawPacket.Data.ReadUInt16();
            var mixedStates = rawPacket.Data.ReadByte();
            var sessionState = mixedStates >> 4;
            var gameState = mixedStates - (sessionState << 4);
            return new PC2GameStatePacket(meta, buildVersionNumber, (PC2GameState)gameState, (PC2SessionState)sessionState);
        }
    }

    public enum PC2SessionState {
        Invalid,
        Practice,
        Test,
        Qualify,
        FormationLap,
        Race,
        TimeAttack
    }

    public enum PC2GameState {
        Exited,
        FrontEnd,
        IngamePlaying,
        Paused,
        IngameInmenuTimeTicking,
        IngameRestarting,
        IngameReplay,
        FrontEndReplay
    }
}
