using System.Collections;
using System.Collections.Generic;
using PathUnit;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    //ゲームの進行状況
    public enum game_chapter
    {
        INIT,BRANCH,LL,LH,HL,HH,TEST,
    }
    game_chapter CHAPTER = game_chapter.TEST;

    public enum branch_state
    {
        IDLE,MOVE1,MOVE2,PICK1,MOVE3,MOVE4,ARM1,SENSE1,
        LOW_PLACE,HIGH_ARM,
        MOVE5,MOVE6,MOVE7,ARM2,SENSE2,
        HIGH_LOW_PLACE,ELSE_ARM,
    }
    branch_state BRANCH_STATE = branch_state.IDLE;

    public enum ll_state
    {
        IDLE,
    }
    ll_state LL_STATE = ll_state.IDLE;

    public enum test_state
    {
        IDLE,MOVE1,PICK,MOVE2,PLACE,BACK,FINISH,
    }
    test_state TEST_STATE = test_state.IDLE;


    //移動の状態
    public enum move_state
    {
        IDLE,MOVE,STOP,
    }
    move_state MOVE_STATE;

    //アームの操作状態
    public enum arm_state
    {
        IDLE,MOVE,STOP,
    }
    arm_state ARM_STATE;

    //温度測定操作の状態
    public enum sensor_state
    {
        IDLE,SENSING,HIGH,LOW,
    }
    sensor_state SENSOR_STATE;

    void Start()
    {
        move_init();
        module_init();
    }

    int cnt = 0;
    void Update()
    {
        move_update();
        arm_update();
        sense_update();

        switch (CHAPTER)
        {
            case game_chapter.INIT:
                if(cnt == 0)
                {
                    arm_open();
                    if(ARM_STATE == arm_state.STOP)
                    {
                        cnt++;
                        ARM_STATE = arm_state.IDLE;
                    }
                }
                else
                {
                    arm_up();
                    if (ARM_STATE == arm_state.STOP)
                    {
                        cnt=0;
                        ARM_STATE = arm_state.IDLE;
                        CHAPTER = game_chapter.BRANCH;
                    }
                }
                break;
            case game_chapter.BRANCH:
                branch_update();
                break;
            case game_chapter.LL:
                break;
            case game_chapter.LH:
                break;
            case game_chapter.HL:
                break;
            case game_chapter.HH:
                break;
            case game_chapter.TEST:
                test_update();
                break;
            default:
                break;
        }
    }

    void branch_update()
    {
        switch (BRANCH_STATE)
        {
            case branch_state.IDLE:
                BRANCH_STATE = branch_state.MOVE1;
                move(lf_point);
                break;
            case branch_state.MOVE1:
                if(MOVE_STATE == move_state.STOP)
                {
                    MOVE_STATE = move_state.IDLE;
                    move(lb_obj);
                    BRANCH_STATE = branch_state.MOVE2;
                }
                break;
            case branch_state.MOVE2:
                if(MOVE_STATE == move_state.STOP)
                {
                    MOVE_STATE = move_state.IDLE;
                    arm_down();
                    BRANCH_STATE = branch_state.PICK1;
                }
                break;
            case branch_state.PICK1:
                if(ARM_STATE == arm_state.STOP)
                {
                    if(cnt == 0)
                    {
                        ARM_STATE = arm_state.IDLE;
                        arm_close();
                        cnt++;
                    }
                    else
                    {
                        ARM_STATE = arm_state.IDLE;
                        cnt = 0;
                        move(g1_point);
                        BRANCH_STATE = branch_state.MOVE3;
                    }
                }
                break;
            case branch_state.MOVE3:
                if(MOVE_STATE == move_state.STOP)
                {
                    MOVE_STATE = move_state.IDLE;
                    move(g1_target);
                    BRANCH_STATE = branch_state.MOVE4;
                }
                break;
            case branch_state.MOVE4:
                if(MOVE_STATE == move_state.STOP)
                {
                    MOVE_STATE = move_state.IDLE;
                    arm_down();
                    BRANCH_STATE = branch_state.ARM1;
                }
                break;
            case branch_state.ARM1:
                if(ARM_STATE == arm_state.STOP)
                {
                    ARM_STATE = arm_state.IDLE;
                    sense();
                    BRANCH_STATE = branch_state.SENSE1;
                }
                break;
            case branch_state.SENSE1:
                if(SENSOR_STATE == sensor_state.HIGH)
                {
                    SENSOR_STATE = sensor_state.IDLE;
                    arm_up();
                    BRANCH_STATE = branch_state.HIGH_ARM;
                }
                if(SENSOR_STATE == sensor_state.LOW)
                {
                    SENSOR_STATE = sensor_state.IDLE;
                    arm_open();
                    BRANCH_STATE = branch_state.LOW_PLACE;
                }
                break;
            case branch_state.LOW_PLACE:
                if(ARM_STATE == arm_state.STOP)
                {
                    ARM_STATE = arm_state.IDLE;
                    if(cnt == 0)
                    {
                        arm_up();
                        cnt++;
                    }
                    else
                    {
                        cnt = 0;
                        move(g1_point, true);
                        BRANCH_STATE = branch_state.MOVE5;
                    }
                }
                break;
            case branch_state.HIGH_ARM:
                if(ARM_STATE == arm_state.STOP)
                {
                    ARM_STATE = arm_state.IDLE;
                    move(g1_point, true);
                    BRANCH_STATE = branch_state.MOVE5;
                }
                break;
            case branch_state.MOVE5:
                if(MOVE_STATE == move_state.STOP)
                {
                    MOVE_STATE = move_state.IDLE;
                    move(g4_point);
                    BRANCH_STATE = branch_state.MOVE6;
                }
                break;
            case branch_state.MOVE6:
                if (MOVE_STATE == move_state.STOP)
                {
                    MOVE_STATE = move_state.IDLE;
                    move(g4_target);
                    BRANCH_STATE = branch_state.MOVE7;
                }
                break;
            case branch_state.MOVE7:
                if (MOVE_STATE == move_state.STOP)
                {
                    MOVE_STATE = move_state.IDLE;
                    arm_down();
                    BRANCH_STATE = branch_state.ARM2;
                }
                break;
            case branch_state.ARM2:
                if(ARM_STATE == arm_state.STOP)
                {
                    ARM_STATE = arm_state.IDLE;
                    sense();
                    BRANCH_STATE = branch_state.SENSE2;
                }
                break;
            case branch_state.SENSE2:
                if(SENSOR_STATE == sensor_state.HIGH)
                {

                }
                break;
            case branch_state.HIGH_LOW_PLACE:
                break;
            case branch_state.ELSE_ARM:
                break;
            default:
                break;
        }
    }

    void ll_update()
    {

    }

    void test_update()
    {
        switch (TEST_STATE)
        {
            case test_state.IDLE:
                if(cnt == 0)
                {
                    arm_open();
                    cnt++;
                }else if (cnt == 1)
                {
                    if(ARM_STATE == arm_state.STOP)
                    {
                        ARM_STATE = arm_state.IDLE;
                        cnt++;
                        arm_up();
                    }
                }
                else
                {
                    if(ARM_STATE == arm_state.STOP)
                    {
                        ARM_STATE = arm_state.IDLE;
                        cnt = 0;
                        move(arm_pos(lf_point));
                        TEST_STATE = test_state.MOVE1;
                    }
                }
                break;
            case test_state.MOVE1:
                if(MOVE_STATE == move_state.STOP)
                {
                    MOVE_STATE = move_state.IDLE;

                    arm_down();

                    TEST_STATE = test_state.PICK;
                }
                break;
            case test_state.PICK:
                if(cnt == 0)
                {
                    if (ARM_STATE == arm_state.STOP)
                    {
                        ARM_STATE = arm_state.IDLE;

                        arm_close();

                        cnt++;
                    }
                }
                else
                {
                    if(ARM_STATE == arm_state.STOP)
                    {
                        ARM_STATE = arm_state.IDLE;

                        move(g1_point);

                        cnt = 0;
                        TEST_STATE = test_state.MOVE2;
                    }
                }
                break;
            case test_state.MOVE2:
                if(cnt == 0)
                {
                    if(MOVE_STATE == move_state.STOP)
                    {
                        MOVE_STATE = move_state.IDLE;

                        move(arm_pos(g1_target));

                        cnt++;
                    }
                }
                else
                {
                    if(MOVE_STATE == move_state.STOP)
                    {
                        MOVE_STATE = move_state.IDLE;

                        arm_down();

                        cnt = 0;
                        TEST_STATE = test_state.PLACE;
                    }
                }
                break;
            case test_state.PLACE:
                if(MOVE_STATE == move_state.STOP)
                {
                    MOVE_STATE = move_state.IDLE;

                    move(g1_point, true);
                    TEST_STATE = test_state.BACK;
                }
                break;
            case test_state.BACK:
                if(MOVE_STATE == move_state.STOP)
                {
                    MOVE_STATE = move_state.IDLE;
                    TEST_STATE = test_state.FINISH;
                }
                break;
            case test_state.FINISH:
                break;
            default:
                break;
        }
    }

    //----------------------------------------------------------------------------------------------------------------------

    //経路追従ユニット
    void move(Vector2 target,bool back = false)
    {
        path = new Path(GetPosition(), target);
        Target = target;
        back = false;
        MOVE_STATE = move_state.MOVE;
    }

    [SerializeField] float Speed;
    [SerializeField] float LookAheadDistance;
    [SerializeField] float Max_Input;
    [SerializeField] float max_Diff;
    [SerializeField] float Kp;
    [SerializeField] float stopDistance;
    
    ICar Car;
    PathAlgs PathAlgs;
    Path path;
    Vector2 Carrot, Target;
    float Kp_start;
    bool Back = false;

    void move_init()
    {
        Car = GetComponent<ICar>();
        PathAlgs = new PathAlgs();
        PathAlgs.Tread = Car.Tread;
        PathAlgs.Speed = Speed;
        PathAlgs.LookAheadDistance = LookAheadDistance;
        path = null;
        Kp_start = Kp;
        MOVE_STATE = move_state.IDLE;
    }

    void move_update()
    {
        if (MOVE_STATE == move_state.STOP) return;

        if (Vector2.Distance(path.Start, GetPosition()) < Vector2.Distance(path.Start, path.Start) - stopDistance)
        {
            MOVE_STATE = move_state.STOP;
            Car.SetInput(0, 0);
            return;
        }
        var pos = PathAlgs.LookPosition(GetPosition(), path);
        var angle = Car.Angle;
        if (Back) angle += Mathf.PI;
        var target = PathAlgs.CalcWheelSpeed(GetPosition(), angle, pos);

        Carrot = pos;

        var Gain = new NewSystem.CarControlUnit.DiffGain(Max_Input, max_Diff);
        var input = Vector2.zero;
        input.x = Mathf.Max(target.x - Car.WheelSpeed.x, 0);
        input.y = Mathf.Max(target.y - Car.WheelSpeed.y, 0);
        Kp = Gain.Gain(input, Kp_start);

        var U = Kp * input;
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
        Gizmos.DrawWireSphere(new Vector3(Carrot.x, 0, Carrot.y) * (float)(1e-2), 1f);
    }

    Vector2 GetPosition()
    {
        Vector2 result = new Vector2(Car.Position.x, Car.Position.y);
        return result;
    }

    //アーム位置補正ユニット
    [SerializeField] Vector2 Offset;

    private Vector2 arm_pos(Vector2 target)
    {
        var pos = Car.Position;

        var c = Vector2.Distance(pos, Target);
        var a = Mathf.Abs(Offset.x);
        var b = Mathf.Sqrt(Mathf.Pow(c, 2) - Mathf.Pow(a, 2));

        var c_theta = (Mathf.Pow(a, 2) + Mathf.Pow(b, 2) - Mathf.Pow(c, 2)) / 2 / a / b;
        var theta = Mathf.Acos(c_theta);

        var direction = Target - pos;
        direction.Normalize();

        var result = Quaternion.Euler(0, 0, theta) * direction * b;

        return result;
    }


    //アーム操作ユニット
    void arm_open()
    {
        ARM_STATE = arm_state.MOVE;
        arm1 = 0;

        int m = 1<<1;
        state &= m;
    }

    void arm_close()
    {
        ARM_STATE = arm_state.MOVE;
        arm1 = 180;

        int m = 1;
        state |= m;
    }

    void arm_up()
    {
        ARM_STATE = arm_state.MOVE;
        arm2 = 0;

        int m = 1;
        state &= m;
    }

    void arm_down()
    {
        ARM_STATE = arm_state.MOVE;
        arm2 = 180;

        int m = 1 << 1;
        state |= m;
    }

    int arm1 = 0, arm2 = 0;
    int state = 0;
    IModule Module;

    void module_init()
    {
        Module = GetComponent<IModule>();
    }

    void arm_update()
    {
        Module.SetArmInput(arm1, arm2);
        if (ARM_STATE == arm_state.MOVE && Module.Arm_State == state) ARM_STATE = arm_state.STOP;
    }

    //温度測定ユニット
    void sense()
    {
        SENSOR_STATE = sensor_state.SENSING;
        temp_cnt = 0;
        cened_param = new List<int>(ave_len + 2);
    }

    [SerializeField] int Temp_Threshold;
    int temp_cnt = 0;
    List<int> cened_param;
    int ave_len = 10;

    void sense_update()
    {
        if (SENSOR_STATE != sensor_state.SENSING) return;

        cened_param[temp_cnt] = Module.Temperture;
        temp_cnt++;

        if(temp_cnt == ave_len)
        {
            cened_param.Sort();
            int ave = 0;
            for (int i = 1; i <= ave_len; i++)
            {
                ave += cened_param[i];
            }
            ave /= ave_len;

            if (ave < Temp_Threshold) SENSOR_STATE = sensor_state.LOW;
            else SENSOR_STATE = sensor_state.HIGH;
        }
    }

    //--------------------------------------------------------------------------------------------

    Vector2 lf_point = new Vector2(-300, 200);
    Vector2 lb_obj = new Vector2(-200, 500);
    Vector2 g1_point = new Vector2(-300, 650);
    Vector2 g1_target = new Vector2(-300, 750);
    Vector2 g4_point = new Vector2(300, 650);
    Vector2 g4_target = new Vector2(300, 750);
}
