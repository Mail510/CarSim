using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RouteUnit
{
    public class Route
    {
        public List<Vector2> Position;
        public bool is_ArmTarget;
        public Route(List<Vector2> position,bool is_armtarget)
        {
            Position = position;
            is_ArmTarget = is_armtarget;
        }
    }

    public class ArmOffset
    {
        public ArmOffset(ICar car,Vector2 offset)
        {
            Offset = offset;
            Car = car;
        }
        Vector2 Offset;
        ICar Car;

        public Vector2 Calc(Vector2 Target)
        {
            var pos = Car.Position;

            var c = Vector2.Distance(pos, Target);
            var a = Mathf.Abs(Offset.x);
            var b = Mathf.Sqrt(Mathf.Pow(c, 2) - Mathf.Pow(a, 2));

            var c_theta = (Mathf.Pow(a, 2) + Mathf.Pow(b, 2) - Mathf.Pow(c, 2)) / 2 / a / b;
            var theta = Mathf.Acos(c_theta);

            var direction = Target - pos;
            direction.Normalize();

            return Quaternion.Euler(0, 0, theta) * direction * b;
        }

        public Vector2 Arm_Position()
        {
            var angle = Car.Angle;
            var offset = Vector2.zero;
            offset.x = Offset.x * Mathf.Sin(angle) + Offset.y * Mathf.Cos(angle);
            offset.y = -Offset.x * Mathf.Cos(angle) + Offset.y * Mathf.Sin(angle);

            return Car.Position + offset;
        }
    }
}