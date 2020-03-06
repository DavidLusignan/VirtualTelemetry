

using CoreService.UDPProjectCars2.RawPacketHandler;

namespace CoreService.UDPProjectCars2.PacketParser.Data {
    class PC2GameStatePacket : PC2BasePacket {
        ushort buildVersionNumber { get; }
        PC2GameState gameState { get; }
        PC2SessionState sessionState { get; }
        public PC2GameStatePacket(PC2PacketMeta baseUDP, ushort buildVersionNumber, PC2GameState gameState, PC2SessionState sessionState) {
            this.baseUDP = baseUDP;
            this.buildVersionNumber = buildVersionNumber;
            this.gameState = gameState;
            this.sessionState = sessionState;
            System.Console.WriteLine("GameState: " + gameState.ToString() + "; SessionState: " + sessionState.ToString());
        }

        public static PC2GameStatePacket Create(PC2RawPacket rawPacket, PC2PacketMeta meta) {
            var buildVersionNumber = rawPacket.Data.ReadUInt16();
            var mixedStates = rawPacket.Data.ReadByte();
            var gameState = mixedStates >> 5;
            var sessionState = mixedStates - (gameState << 5) >> 2;
            return new PC2GameStatePacket(meta, buildVersionNumber, (PC2GameState)gameState, (PC2SessionState)sessionState);
        }
    }

    enum PC2SessionState {
        Invalid,
        Practice,
        Test,
        Qualify,
        FormationLap,
        Race,
        TimeAttack
    }

    enum PC2GameState {
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
