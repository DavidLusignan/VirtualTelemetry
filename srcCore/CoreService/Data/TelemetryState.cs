using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.Data {
    public class TelemetryState : DataState {
        public double lapNumber;
        public LapData previousLapData;

        public TelemetryState(double lapNumber, LapData previousLapData) {
            this.lapNumber = lapNumber;
            this.previousLapData = previousLapData;
        }
    }
}
