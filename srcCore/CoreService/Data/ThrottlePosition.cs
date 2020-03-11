using System;

namespace CoreService.UDPProjectCars2.StdDataConvertor {
    public class ThrottlePosition {
        public double Min = 0d;
        public double Max = 100d;
        public double Value { get; }
        public DateTime TimeStamp { get; }
        public ThrottlePosition(double value, DateTime time) {
            if (value < Min || value > Max) {
                throw new ArgumentException("Throttle position was not between min and max values");
            }
            Value = value;
            TimeStamp = time;
        }
    }
}
