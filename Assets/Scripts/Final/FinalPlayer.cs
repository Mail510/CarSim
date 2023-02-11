using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RouteUnit;
using ArmControllUnit;

public class FinalPlayer : MonoBehaviour
{
    [SerializeField] int tmp_threshold;
    [SerializeField] float tmp_time = 3;

    public enum game_chapter
    {
        INIT,MEASURE,SCENARIO,
    }
    game_chapter Chapter = game_chapter.INIT;

    [System.Serializable]
    public enum game_scenario
    {
        NULL,LL,LH,HL,HH,
    }
    [SerializeField] game_scenario Scenario = game_scenario.NULL;

    public int STATE = 0;

    private RouteFollowing RF;
    private ArmControll AC;
    private IModule Module;
    // Start is called before the first frame update
    void Start()
    {
        RF = GetComponent<RouteFollowing>();
        AC = GetComponent<ArmControll>();
        Module = GetComponent<IModule>();
    }

    // Update is called once per frame
    void Update()
    {

        switch (Chapter)
        {
            case game_chapter.INIT:
                Chapter = game_chapter.MEASURE;
                break;
            case game_chapter.MEASURE:
                measure_update();
                break;
            case game_chapter.SCENARIO:
                switch (Scenario)
                {
                    case game_scenario.LL:
                        ll_scenario();
                        break;
                    case game_scenario.LH:
                        lh_scenario();
                        break;
                    case game_scenario.HL:
                        hl_scenario();
                        break;
                    case game_scenario.HH:
                        hh_scenario();
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    bool Left_TMP = false, Right_TMP = false;
    float time_cnt = 0;
    private void measure_update()
    {
        switch (STATE)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    RF.SetRoute(new Route(to_L2, true));
                    STATE++;
                }
                break;
            case 1:
                if (RF.State == RouteFollowing.state.STOP)
                {
                    AC.arm_down();
                    STATE++;
                }
                break;
            case 2:
                if (AC.State == ArmControll.state.STOP)
                {
                    AC.arm_close();
                    STATE++;
                }
                break;
            case 3:
                if (AC.State == ArmControll.state.STOP)
                {
                    AC.arm_up();
                    STATE++;
                }
                break;
            case 4:
                if (AC.State == ArmControll.state.STOP)
                {
                    RF.SetPick();
                    RF.SetRoute(new Route(to_T1, true));
                    STATE++;
                }
                break;
            case 5:
                if (RF.State == RouteFollowing.state.STOP)
                {
                    AC.arm_half();
                    STATE++;
                }
                break;
            case 6:
                time_cnt += Time.deltaTime;
                if (time_cnt >= tmp_time)
                {
                    AC.arm_open();
                    RF.SetPick(false);
                    if (Module.Temperture == 1)
                    {
                        Left_TMP = true;
                   
                    }
                    else
                    {
                        STATE++;
                    }
                    time_cnt = 0;
                    STATE++;
                }
                break;
            case 7:
                if (AC.State == ArmControll.state.STOP) STATE++;
                break;
            case 8:
                AC.arm_up();
                STATE++;
                break;
            case 9:
                if (AC.State == ArmControll.state.STOP)
                {
                    RF.SetRoute(new Route(new List<Vector2>() { new Vector2(-300, 850 - 165) }, false), true);
                    STATE = 200;
                }
                break;
            case 10:
                if (RF.State == RouteFollowing.state.STOP)
                {
                    RF.SetRoute(new Route(to_T4, true));
                    STATE++;
                }
                break;
            case 11:
                if (RF.State == RouteFollowing.state.STOP)
                {
                    STATE++;
                    AC.arm_half();
                }
                break;
            case 12:
                time_cnt += Time.deltaTime;
                if (time_cnt >= tmp_time)
                {
                    if (Module.Temperture == 1)
                    {
                        Right_TMP = true;
                        RF.SetPick(false);
                        AC.arm_open();
                    }
                    else
                    {
                        STATE++;
                    }
                    time_cnt = 0;
                    STATE++;
                }
                break;
            case 13:
                if (AC.State == ArmControll.state.STOP) STATE++;
                break;
            case 14:
                AC.arm_up();
                STATE++;
                break;
            case 15:
                if (AC.State == ArmControll.state.STOP)
                {
                    RF.SetRoute(new Route(new List<Vector2>() { new Vector2(-200, 700 - 165) }, false), true);
                    STATE++;
                }
                break;
            case 16:
                if (RF.State == RouteFollowing.state.STOP)
                {
                    Debug.Log("END_MEASURE!!!");

                    if (Left_TMP)
                    {
                        if (Right_TMP) Scenario = game_scenario.HH;
                        else Scenario = game_scenario.HL;
                    }
                    else
                    {
                        if (Right_TMP) Scenario = game_scenario.LH;
                        else Scenario = game_scenario.LL;
                    }
                    Chapter = game_chapter.SCENARIO;
                    STATE = 0;
                }
                break;
            default:
                break;
        }
    }

    private void ll_scenario()
    {
        switch (STATE)
        {
            case 0:
                RF.SetRoute(new Route(to_T2, true));
                STATE++;
                break;
            case 1:
                if (is_move_stop())
                {
                    AC.arm_down();
                    STATE++;
                }
                break;
            case 2:
                if (is_arm_stop())
                {
                    AC.arm_open();
                    STATE++;
                }
                break;
            case 3:
                if (is_arm_stop())
                {
                    AC.arm_up();
                    STATE++;
                }
                break;
            case 4:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_ll1, false), true);
                    STATE++;
                }
                break;
            case 5:
                if (is_move_stop())
                {
                    RF.SetRoute(new Route(to_L1, true));
                    STATE++;
                }
                break;
            case 6:
                if (is_move_stop())
                {
                    AC.arm_down();
                    STATE++;
                }
                break;
            case 7:
                if (is_arm_stop())
                {
                    AC.arm_close();
                    STATE++;
                }
                break;
            case 8:
                if (is_arm_stop())
                {
                    AC.arm_up();
                    STATE++;
                }
                break;
            case 9:
                STATE++;
                /*
                if (is_move_stop())
                {
                    RF.SetRoute(new Route(to_ll1, false), true);
                    STATE++;
                }*/
                break;
            case 10:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_T1, true));
                    STATE++;
                }
                break;
            case 11:
                if (is_move_stop())
                {
                    AC.arm_open();
                    STATE++;
                }
                break;
            case 12:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_ll2, false), true);
                    STATE++;
                }
                break;
            case 13:
                if (is_move_stop())
                {
                    RF.SetRoute(new Route(to_R2, true));
                    STATE++;
                }
                break;
            case 14:
                if (is_move_stop())
                {
                    AC.arm_down();
                    STATE++;
                }
                break;
            case 15:
                if (is_arm_stop())
                {
                    AC.arm_close();
                    STATE++;
                }
                break;
            case 16:
                if (is_arm_stop())
                {
                    AC.arm_up();
                    STATE++;
                }
                break;
            case 17:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_T4, true));
                    STATE++;
                }
                break;
            case 18:
                if (is_move_stop())
                {
                    AC.arm_open();
                    STATE++;
                }
                break;
            case 19:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_ll3, false), true);
                    STATE++;
                }
                break;
            case 20:
                if (is_move_stop())
                {
                    RF.SetRoute(new Route(to_R1, true));
                    STATE++;
                }
                break;
            case 21:
                if (is_move_stop())
                {
                    AC.arm_down();
                    STATE++;
                }
                break;
            case 22:
                if (is_arm_stop())
                {
                    AC.arm_close();
                    STATE++;
                }
                break;
            case 23:
                if (is_arm_stop())
                {
                    AC.arm_up();
                    STATE++;
                }
                break;
            case 24:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_ll3, false), true);
                    STATE++;
                }
                break;
            case 25:
                if (is_move_stop())
                {
                    RF.SetRoute(new Route(to_T3, true));
                    STATE++;
                }
                break;
            case 26:
                if (is_move_stop())
                {
                    AC.arm_down();
                    STATE++;
                }
                break;
            case 27:
                if (is_arm_stop())
                {
                    AC.arm_open();
                    STATE++;
                }
                break;
            case 28:
                if (is_arm_stop())
                {
                    AC.arm_up();
                    STATE++;
                }
                break;
            case 29:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_s, false), true);
                    STATE++;
                }
                break;
            default:

                break;
        }
    }

    private void lh_scenario()
    {
        switch (STATE)
        {
            case 0:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_ll1, false), true);
                    STATE=5;
                }
                break;
            case 5:
                if (is_move_stop())
                {
                    RF.SetRoute(new Route(to_L1, true));
                    STATE++;
                }
                break;
            case 6:
                if (is_move_stop())
                {
                    AC.arm_down();
                    STATE++;
                }
                break;
            case 7:
                if (is_arm_stop())
                {
                    AC.arm_close();
                    STATE++;
                }
                break;
            case 8:
                if (is_arm_stop())
                {
                    AC.arm_up();
                    STATE++;
                }
                break;
            case 9:
                STATE++;
                /*
                if (is_move_stop())
                {
                    RF.SetRoute(new Route(to_ll1, false), true);
                    STATE++;
                }*/
                break;
            case 10:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_T1, true));
                    STATE++;
                }
                break;
            case 11:
                if (is_move_stop())
                {
                    AC.arm_open();
                    STATE++;
                }
                break;
            case 12:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_ll2, false), true);
                    STATE++;
                }
                break;
            case 13:
                if (is_move_stop())
                {
                    RF.SetRoute(new Route(to_R2, true));
                    STATE++;
                }
                break;
            case 14:
                if (is_move_stop())
                {
                    AC.arm_down();
                    STATE++;
                }
                break;
            case 15:
                if (is_arm_stop())
                {
                    AC.arm_close();
                    STATE++;
                }
                break;
            case 16:
                if (is_arm_stop())
                {
                    AC.arm_up();
                    STATE++;
                }
                break;
            case 17:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_T3, true));
                    STATE++;
                }
                break;
            case 18:
                if (is_move_stop())
                {
                    AC.arm_open();
                    STATE++;
                }
                break;
            case 19:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_ll3, false), true);
                    STATE++;
                }
                break;
            case 20:
                if (is_move_stop())
                {
                    RF.SetRoute(new Route(to_R1, true));
                    STATE++;
                }
                break;
            case 21:
                if (is_move_stop())
                {
                    AC.arm_down();
                    STATE++;
                }
                break;
            case 22:
                if (is_arm_stop())
                {
                    AC.arm_close();
                    STATE++;
                }
                break;
            case 23:
                if (is_arm_stop())
                {
                    AC.arm_up();
                    STATE++;
                }
                break;
            case 24:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_ll3, false), true);
                    STATE++;
                }
                break;
            case 25:
                if (is_move_stop())
                {
                    RF.SetRoute(new Route(to_T2, true));
                    STATE++;
                }
                break;
            case 26:
                if (is_move_stop())
                {
                    AC.arm_down();
                    STATE++;
                }
                break;
            case 27:
                if (is_arm_stop())
                {
                    AC.arm_open();
                    STATE++;
                }
                break;
            case 28:
                if (is_arm_stop())
                {
                    AC.arm_up();
                    STATE++;
                }
                break;
            case 29:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_s, false), true);
                    STATE++;
                }
                break;
            default:

                break;
        }
    }

    private void hl_scenario()
    {
        switch (STATE)
        {
            case 0:
                RF.SetRoute(new Route(to_R2, true));
                STATE = 6;
                break;
            case 6:
                if (is_move_stop())
                {
                    AC.arm_down();
                    STATE++;
                }
                break;
            case 7:
                if (is_arm_stop())
                {
                    AC.arm_close();
                    STATE++;
                }
                break;
            case 8:
                if (is_arm_stop())
                {
                    AC.arm_up();
                    STATE++;
                }
                break;
            case 9:
                if (is_move_stop())
                {
                    RF.SetRoute(new Route(to_home, false), true);
                    STATE++;
                }
                break;
            case 10:
                if (is_move_stop())
                {
                    RF.SetRoute(new Route(to_T4, true));
                    STATE++;
                }
                break;
            case 11:
                if (is_move_stop())
                {
                    AC.arm_open();
                    STATE++;
                }
                break;
            case 12:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_home, false), true);
                    STATE++;
                }
                break;
            case 13:
                if (is_move_stop())
                {
                    RF.SetRoute(new Route(to_L1, true));
                    STATE++;
                }
                break;
            case 14:
                if (is_move_stop())
                {
                    AC.arm_down();
                    STATE++;
                }
                break;
            case 15:
                if (is_arm_stop())
                {
                    AC.arm_close();
                    STATE++;
                }
                break;
            case 16:
                if (is_arm_stop())
                {
                    AC.arm_up();
                    STATE++;
                }
                break;
            case 17:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_T3, true));
                    STATE++;
                }
                break;
            case 18:
                if (is_move_stop())
                {
                    AC.arm_down();
                    STATE++;
                }
                break;
            case 19:
                if (is_arm_stop())
                {
                    AC.arm_open();
                    STATE++;
                }
                break;
            case 20:
                if (is_arm_stop())
                {
                    AC.arm_up();
                    STATE++;
                }
                break;
            case 21:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_home, false), true);
                    STATE++;
                }
                break;
            case 22:
                if (is_move_stop())
                {
                    RF.SetRoute(new Route(to_R1, true));
                    STATE++;
                }
                break;
            case 23:
                if (is_move_stop())
                {
                    AC.arm_down();
                    STATE++;
                }
                break;
            case 24:
                if (is_arm_stop())
                {
                    AC.arm_close();
                    STATE++;
                }
                break;
            case 25:
                if (is_arm_stop())
                {
                    AC.arm_up();
                    STATE++;
                }
                break;
            case 26:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_home, false), true);
                    STATE++;
                }
                break;
            case 27:
                if (is_move_stop())
                {
                    RF.SetRoute(new Route(to_T2, true));
                    STATE++;
                }
                break;
            case 28:
                if (is_move_stop())
                {
                    AC.arm_down();
                    STATE++;
                }
                break;
            case 29:
                if (is_arm_stop())
                {
                    AC.arm_open();
                    STATE++;
                }
                break;
            case 30:
                if (is_arm_stop())
                {
                    AC.arm_up();
                    STATE++;
                }
                break;
            case 31:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_s, false), true);
                    STATE++;
                }
                break;
            default:

                break;
        }
    }

    private void hh_scenario()
    {
        switch (STATE)
        {
            case 0:
                RF.SetRoute(new Route(to_R1, true));
                STATE = 6;
                break;
            case 6:
                if (is_move_stop())
                {
                    AC.arm_down();
                    STATE++;
                }
                break;
            case 7:
                if (is_arm_stop())
                {
                    AC.arm_close();
                    STATE++;
                }
                break;
            case 8:
                if (is_arm_stop())
                {
                    AC.arm_up();
                    STATE++;
                }
                break;
            case 9:
                if (is_move_stop())
                {
                    RF.SetRoute(new Route(to_home, false), true);
                    STATE++;
                }
                break;
            case 10:
                if (is_move_stop())
                {
                    RF.SetRoute(new Route(to_T4, true));
                    STATE++;
                }
                break;
            case 11:
                if (is_move_stop())
                {
                    AC.arm_open();
                    STATE++;
                }
                break;
            case 12:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_home, false), true);
                    STATE++;
                }
                break;
            case 13:
                if (is_move_stop())
                {
                    RF.SetRoute(new Route(to_L1, true));
                    STATE++;
                }
                break;
            case 14:
                if (is_move_stop())
                {
                    AC.arm_down();
                    STATE++;
                }
                break;
            case 15:
                if (is_arm_stop())
                {
                    AC.arm_close();
                    STATE++;
                }
                break;
            case 16:
                if (is_arm_stop())
                {
                    AC.arm_up();
                    STATE = 100;
                }
                break;
            case 100:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_home, false), true);
                    STATE = 17;
                }
                break;
            case 17:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_T3, true));
                    STATE++;
                }
                break;
            case 18:
                if (is_move_stop())
                {
                    AC.arm_down();
                    STATE++;
                }
                break;
            case 19:
                if (is_arm_stop())
                {
                    AC.arm_open();
                    STATE++;
                }
                break;
            case 20:
                if (is_arm_stop())
                {
                    AC.arm_up();
                    STATE++;
                }
                break;
            case 21:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_home, false), true);
                    STATE++;
                }
                break;
            case 22:
                if (is_move_stop())
                {
                    RF.SetRoute(new Route(to_R2, true));
                    STATE++;
                }
                break;
            case 23:
                if (is_move_stop())
                {
                    AC.arm_down();
                    STATE++;
                }
                break;
            case 24:
                if (is_arm_stop())
                {
                    AC.arm_close();
                    STATE++;
                }
                break;
            case 25:
                if (is_arm_stop())
                {
                    AC.arm_up();
                    STATE++;
                }
                break;
            case 26:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_home, false), true);
                    STATE++;
                }
                break;
            case 27:
                if (is_move_stop())
                {
                    RF.SetRoute(new Route(to_T2, true));
                    STATE++;
                }
                break;
            case 28:
                if (is_move_stop())
                {
                    AC.arm_down();
                    STATE++;
                }
                break;
            case 29:
                if (is_arm_stop())
                {
                    AC.arm_open();
                    STATE++;
                }
                break;
            case 30:
                if (is_arm_stop())
                {
                    AC.arm_up();
                    STATE++;
                }
                break;
            case 31:
                if (is_arm_stop())
                {
                    RF.SetRoute(new Route(to_s, false), true);
                    STATE++;
                }
                break;
            default:

                break;
        }
    }

    private bool is_arm_stop()
    {
        return AC.State == ArmControll.state.STOP;
    }

    private bool is_move_stop()
    {
        return RF.State == RouteFollowing.state.STOP;
    }
    //-----------
    //ÉãÅ[ÉgíËã`
    List<Vector2> to_L2 = new List<Vector2>() { new Vector2(-300, 500-165), new Vector2(-200, 700 - 165) };
    List<Vector2> to_T1 = new List<Vector2>() { new Vector2(-300, 850 - 165), new Vector2(-300, 1000 - 165) };
    List<Vector2> to_T4 = new List<Vector2>() { new Vector2(300, 850 - 165), new Vector2(300, 1000 - 165) };

    List<Vector2> to_T2 = new List<Vector2>() { new Vector2(-100, 850 - 165), new Vector2(-100, 1000 - 165) };
    List<Vector2> to_T3 = new List<Vector2>() { new Vector2(100, 850 - 165), new Vector2(100, 1000 - 165) };

    List<Vector2> to_L1 = new List<Vector2>() { new Vector2(-100, 500 - 165) };
    List<Vector2> to_R1 = new List<Vector2>() { new Vector2(100, 500 - 165) };
    List<Vector2> to_R2 = new List<Vector2>() { new Vector2(200, 700 - 165) };

    List<Vector2> to_ll1 = new List<Vector2>() { new Vector2(-300, 300 - 165) };
    List<Vector2> to_ll2 = new List<Vector2>() { new Vector2(-300, 500 - 165) };
    List<Vector2> to_ll3 = new List<Vector2>() { new Vector2(-200, 600 - 165) };

    List<Vector2> to_s = new List<Vector2>() { new Vector2(0, 0) };

    List<Vector2> to_home = new List<Vector2>() { new Vector2(-200, 700 - 165) };
}
