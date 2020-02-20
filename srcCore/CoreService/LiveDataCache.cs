using CoreService.Data;
using Global.Enumerable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreService {
    public class LiveDataCache {
        List<TelemetryState> dataCache;
        Dictionary<int, LapData> lapDatas;

        public LiveDataCache() {
            dataCache = new List<TelemetryState>();
            lapDatas = new Dictionary<int, LapData>();
        }

        public void AddData(TelemetryState state) {
            dataCache.Add(state);
        }

        public void AddLapData(int lap, LapData lapData) {
            lock(lapDatas) {
                lapDatas[lap] = lapData;
            }
        }

        public Dictionary<int, LapData> GetLapDatas() {
            return lapDatas.ToDictionary();
        }
    }

    public class LapData {
        public double lapTime;
        public Dictionary<int, double> sectorTimes;

        public LapData(double lapTime, Dictionary<int, double> sectorTimes) {
            this.lapTime = lapTime;
            this.sectorTimes = sectorTimes;
        }
    }
}
