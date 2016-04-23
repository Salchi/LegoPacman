﻿using MonoBrickFirmware.Display;
using MonoBrickFirmware.Sensors;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegoPacman.classes
{
    public class ColorAnalyzer
    {
        public List<KnownColor> ValidColors { get; }

        public ColorAnalyzer(List<KnownColor> knownColors)
        {
            ValidColors = new List<KnownColor>(knownColors);
        }

        private const double MaxAverageDistance = 25d;
        public KnownColor Analyze(RGBColor c)
        {
            var currentDistance = MaxAverageDistance;
            var result = KnownColor.Invalid;

            foreach(var color in ValidColors)
            {
                var tempDistance = AverageDistance(color, c);
                //LcdConsole.WriteLine("{0} {1} {2}", color, tempDistance, MaxDistance(kc, c));

                if (SpikeTest(color, c) && tempDistance < currentDistance)
                {
                    currentDistance = tempDistance;
                    result = color;
                }
            }

            return result;
        }

        // max allowed distance for a single color-component
        private const double MaxSpikeDistance = 35d;
        private static bool SpikeTest(KnownColor color, RGBColor c)
        {
            return MaxDistance(color, c) <= MaxSpikeDistance;
        }

        private static double MaxDistance(KnownColor color, RGBColor c)
        {
            var kcRgb = color.RgbDefinition;

            return LegoMath.Max3(Math.Abs(kcRgb.Red - c.Red), Math.Abs(kcRgb.Green - c.Green), Math.Abs(kcRgb.Blue - c.Blue));
        }

        private static double AverageDistance(KnownColor color, RGBColor c)
        {
            var kcRgb = color.RgbDefinition;

            return (Math.Abs(kcRgb.Red - c.Red) + Math.Abs(kcRgb.Green - c.Green) + Math.Abs(kcRgb.Blue - c.Blue)) / 3d;
        }
    }
}
