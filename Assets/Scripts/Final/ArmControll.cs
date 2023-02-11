using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArmControllUnit
{
    public class ArmControll : MonoBehaviour
    {
        public state State = state.IDLE;

        //アーム操作ユニット
        public void arm_open()
        {
            State = state.MOVE;
            arm1 = 0;

            int m = 1 << 1;
           arm_state &= m;
        }

        public  void arm_close()
        {
            State = state.MOVE;
            arm1 = 150;

            int m = 1;
            arm_state |= m;
        }

        public void arm_up()
        {
            State = state.MOVE;
            arm2 = 0;

            int m = 1;
            arm_state &= m;
        }

        public void arm_half()
        {
            arm2 = 80;
        }

        public void arm_down()
        {
            State = state.MOVE;
            arm2 = 150;

            int m = 1 << 1;
            arm_state |= m;
        }

        int arm1 = 0, arm2 = 0;
        IModule Module;
       [SerializeField] int arm_state = 0;

        void Start()
        {
            Module = GetComponent<IModule>();
        }

        void Update()
        {
            Module.SetArmInput(arm1, arm2);
            if (State == state.MOVE && Module.Arm_State == arm_state) State = state.STOP;
        }

        public enum state
        {
            IDLE,MOVE,STOP,
        }
    }
}