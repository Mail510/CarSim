using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathUnit
{
    public class Path
    {
        public Vector2 Start, End;
        public Path(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }
    }
    public class PathAlgs
    {
        public float LookAheadDistance = 1;
        public float Speed = 1;
        public float Tread = 1;
        public float Diamitor = 1;
        
        public Vector2 LookPosition(Vector2 pos,Path path)
        {
            var start = path.Start;
            var end = path.End;
            var tilt = end - start;
            tilt.Normalize();
            var perpend = Vector2.Perpendicular(tilt);
            perpend.Normalize();

            float a, b;
            Vector2 result;

            if(tilt.y == 0)
            {
                result.y = start.y;
                b = (start.y - pos.y) / perpend.y;
                result.x = pos.x + perpend.x * b;
            }
            else
            {
                var m = tilt.x / tilt.y;
                b = ((start.x - m * start.y) - (pos.x - m * pos.y)) / (perpend.x - m * perpend.y);
                result.x = pos.x + perpend.x * b;
                result.y = pos.y + perpend.y * b;
            }

            var distance = Vector2.Distance(pos, result);
            if (distance < LookAheadDistance)
            {
                var length = Mathf.Sqrt(LookAheadDistance * LookAheadDistance - distance * distance);
                result += tilt*length;

                
            }

            return result;
        }

        public Vector2 CalcRotateSpeed(Vector2 pos,float angle,Vector2 target)
        {
            Vector2 result;

            float alpha = Mathf.Atan2(target.y - pos.y,target.x - pos.x) - angle;

            Debug.Log(alpha);

            var sina = Mathf.Sin(alpha);

            if(sina == 0)
            {
                if (target.y - pos.y >= 0) sina = 0.01f;
                else sina = -0.01f;
            }

            var R = LookAheadDistance / (2 * sina);

            var omega = 2 * Speed * sina / LookAheadDistance;

            result.x = (R + Tread / 2) * omega / (Diamitor / 2);
            result.y = (R - Tread / 2) * omega / (Diamitor / 2);

            return result;
        }

        public Vector2 CalcWheelSpeed(Vector2 pos,float angle,Vector2 target)
        {
            Vector2 result;

            float alpha = Mathf.Atan2(target.y - pos.y, target.x - pos.x) - angle;

            //Debug.Log(alpha);

            var sina = Mathf.Sin(alpha);

            if (sina == 0)
            {
                if (target.y - pos.y >= 0) sina = 0.01f;
                else sina = -0.01f;
            }

            var R = LookAheadDistance / (2 * sina);

            var omega = 2 * Speed * sina / LookAheadDistance;

            result.x = (R + Tread / 2) * omega;
            result.y = (R - Tread / 2) * omega;

            return result;
        }
    }
}