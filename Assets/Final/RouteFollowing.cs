using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathUnit;

namespace RouteUnit
{
    public class RouteFollowing : MonoBehaviour
    {
        [SerializeField] float Speed;
        [SerializeField] float LookAheadDistance;
        [SerializeField] float Max_Input;
        [SerializeField] float max_Diff;
        [SerializeField] bool is_pick = false;
        [SerializeField] float Max_Input_Pick;
        [SerializeField] float max_Diff_Pick;
        [SerializeField] float Kp;
        [SerializeField] float stopDistance;
        [SerializeField] Vector2 Offset;
        [SerializeField, Range(0, 2)] float Propotion = 1.0f;
        [SerializeField] Vector2 U;
        ICar Car;

        PathAlgs PathAlgs;
        ArmOffset ArmOffset;
        Path path;
        Vector2 Carrot;
        float Kp_start;

        public void SetPick(bool pick = true)
        {
            is_pick = pick;
        }

        public void SetRoute(Route route,bool back = false)
        {
            State = state.MOVE;
            Route = route;
            Back = back;
            path = GetPath(route.Position[0]);
            route_cnt = 0;
        }

        private Route Route;
        private bool Back = false;
        public state State = state.IDLE;

        private void Start()
        {
            Car = GetComponent<ICar>();
            PathAlgs = new PathAlgs();
            PathAlgs.Tread = Car.Tread;
            PathAlgs.Speed = Speed;
            PathAlgs.LookAheadDistance = LookAheadDistance;
            Kp_start = Kp;
            ArmOffset = new ArmOffset(Car, Offset);
        }

        private int route_cnt = 0;
        private bool arm_run = false;
        private void Update()
        {
            if (State != state.MOVE) return;

            if (Vector2.Distance(path.End, GetPosition()) < stopDistance || Vector2.Distance(path.Start,path.End)+stopDistance < Vector2.Distance(path.Start,GetPosition()))
            {
                if(Route.Position.Count > route_cnt+1)
                {
                    route_cnt++;
                    arm_run = (Route.is_ArmTarget && (Route.Position.Count == route_cnt + 1));
                    path = GetPath(Route.Position[route_cnt]);
                }
                else
                {
                    State = state.STOP;
                    Car.SetInput(0, 0);
                    route_cnt = 0;
                }
                return;
            }
            var pos = PathAlgs.LookPosition(GetPosition(), path);
            var angle = Car.Angle;
            if (Back)
            {

                angle += Mathf.PI;
                angle = PItoPI(angle);

            }
            var target = PathAlgs.CalcWheelSpeed(GetPosition(), angle, pos);

            Carrot = pos;

            var Gain = new NewSystem.CarControlUnit.DiffGain(Max_Input, max_Diff);
            if (is_pick)
            {
                Gain = new NewSystem.CarControlUnit.DiffGain(Max_Input_Pick, max_Diff_Pick);
            }
            var input = Vector2.zero;
            input.x = Mathf.Max(target.x - Car.WheelSpeed.x, 0) * Propotion;
            input.y = Mathf.Max(target.y - Car.WheelSpeed.y, 0) * (2 - Propotion);
            Kp = Gain.Gain(input, Kp_start);

            U = Kp * input;
            if (Back)
            {
                var u = Vector2.zero;
                u.x = -1 * U.y;
                u.y = -1 * U.x;
                U = u;
            }

            Car.SetInput(U.x, U.y);
        }

        Vector2 GetPosition()
        {
            Vector2 result = new Vector2(Car.Position.x, Car.Position.y);
            if (arm_run) result = ArmOffset.Arm_Position();
            return result;
        }

        Path GetPath(Vector2 target)
        {
            return new Path(GetPosition(), target);
        }

        float PItoPI(float angle)
        {
            while (angle >= Mathf.PI * 2) angle -= Mathf.PI;
            while (angle <= -Mathf.PI * 2) angle += Mathf.PI;
            return angle;
        }

        public enum state
        {
            IDLE,MOVE,STOP,
        }
    }
}