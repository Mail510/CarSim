using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewSystem.CarControlUnit
{
    public class DiffGain
    {
        private float Max;
        private float Diff;
        public DiffGain(float max,float diff)
        {
            Max = max;
            Diff = diff;
        }

        public float Gain(Vector2 input,float gain)
        {
            var diff = Mathf.Abs(input.x - input.y);
            var max = Mathf.Max(Mathf.Abs(input.x), Mathf.Abs(input.y));

            if (max == 0) return 1;

            var mg = Max / max;
            var dg = Diff / diff;

            return Mathf.Min(mg, dg, gain);
        }
    }
}