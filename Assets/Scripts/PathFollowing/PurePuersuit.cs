using PathUnit;
using UnityEngine;

public class PurePuersuit : MonoBehaviour
{
    [SerializeField] Vector2 Target;
    [SerializeField] float Speed;
    [SerializeField] float LookAheadDistance;
    [SerializeField] float Max_Input;
    [SerializeField] float max_Diff;
    [SerializeField] float Kp;
    [SerializeField] float stopDistance;
    [SerializeField] Vector2 U;
    [SerializeField] bool Back = false;
    [SerializeField,Range(0,2)] float Propotion=1.0f;
    [SerializeField] Vector2 Offset;
    [SerializeField] bool Arm_Move = false;
    ICar Car;

    RouteUnit.ArmOffset ArmOffset;

    PathAlgs PathAlgs;
    Path path;
    Vector2 Carrot;
    float Kp_start;

    // Start is called before the first frame update
    void Start()
    {
        Car = GetComponent<ICar>();
        PathAlgs = new PathAlgs();
        PathAlgs.Tread = Car.Tread;
        PathAlgs.Speed = Speed;
        PathAlgs.LookAheadDistance = LookAheadDistance;
       // path = GetPath();
        Kp_start = Kp;
        ArmOffset = new RouteUnit.ArmOffset(Car, Offset);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            path = GetPath();
        }
        if (path is null) return;

        if (Vector2.Distance(path.Start,GetPosition()) + stopDistance > Vector2.Distance(path.Start,path.End))
        {
            Car.SetInput(0, 0);
            return;
        }
        var pos = PathAlgs.LookPosition(GetPosition(), path);
        var angle = Car.Angle;
        if (Back) angle += Mathf.PI;
        var target = PathAlgs.CalcWheelSpeed(GetPosition(), angle, pos);

        Carrot = pos;

        //Debug.Log(target);
        var Gain = new NewSystem.CarControlUnit.DiffGain(Max_Input,max_Diff);
        var input = Vector2.zero;
        input.x = Mathf.Max(target.x - Car.WheelSpeed.x, 0) * Propotion;
        input.y = Mathf.Max(target.y - Car.WheelSpeed.y, 0) * (2-Propotion);
        Kp = Gain.Gain(input,Kp_start);

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(Target.x, 0, Target.y) * (float)(1e-2), 1f);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(new Vector3(Carrot.x,0, Carrot.y)*(float)(1e-2), 1f);
    }

    Vector2 GetPosition()
    {
        Vector2 result = new Vector2(Car.Position.x, Car.Position.y);
        if (Arm_Move) result = ArmOffset.Arm_Position();
        return result;
    }

    Path GetPath()
    {
        return new Path(GetPosition(), Target);
    }
}
