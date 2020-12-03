﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SensorData.SharedComponents
{
    public static class SensorDataOverTcpProtocol
    {
        //These are not configurable values
        //These are kept as constants for the sake of readability

        public const int NumberOfBytesToRepresentPacketLength = 2;

        public class Commands
        {
            public const byte StartSensorDataService = 0x00;
            public const byte StopSensorDataService = 0x01;
        }
    }
}