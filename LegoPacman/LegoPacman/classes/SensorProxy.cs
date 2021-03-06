﻿using MonoBrickFirmware.Display;
using MonoBrickFirmware.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LegoPacman.classes
{
    class SensorProxy
    {
        private EV3GyroSensor gyroSensor;
        private EV3UltrasonicSensor ultrasonicSensor;

        public ColorReader ColorReader { get; }

        public SensorProxy(SensorPort gyroPort, SensorPort ultrasonicPort, SensorPort colorPort)
        {
            gyroSensor = new EV3GyroSensor(gyroPort, GyroMode.Angle);
            ultrasonicSensor = new EV3UltrasonicSensor(ultrasonicPort, UltraSonicMode.Centimeter);
            ColorReader = new ColorReader(colorPort);
            LegoUtils.NullPrint("gyroSensor", gyroSensor);
        }

        public double ReadDistanceInCm()
        {
            const int MaxTries = 100;
            // the ultrasonic sensor seemingly returns the distance in millimeters
            const double MmToCmFactor = 0.1d;

            var val = ultrasonicSensor.Read();
            var tries = 0;
            while (val == 0 && tries < MaxTries)
            {
                val = ultrasonicSensor.Read();
                tries++;
                Thread.Sleep(20);
            }

            return val * MmToCmFactor;
        }

        public int ReadGyro(RotationDirection direction)
        {
            if (direction == RotationDirection.Left)
            {
                return Math.Abs(gyroSensor.Read());
            }
            else
            {
                return 360 - gyroSensor.Read();
            }
        }

        public void ResetGyro()
        {
            gyroSensor.Reset();
        }
    }
}
