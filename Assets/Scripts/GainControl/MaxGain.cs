using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewSystem.CarControlUnit
{
    public class MaxGain
    {
        private float Max;
        public MaxGain(float max)
        {
            Max = max;
        }

        public float Gain(Vector2 input,float gain)
        {
            var max_input = Mathf.Max(Mathf.Abs(input.x), Mathf.Abs(input.y));

            if (max_input == 0) return 1;

            var max_gain = Max / max_input;

            return Mathf.Min(gain,max_gain);
        }
    }
}