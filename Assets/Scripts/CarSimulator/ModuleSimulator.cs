using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarSimulator
{
    public class ModuleSimulator : MonoBehaviour, IModule
    {
        [System.Serializable,System.Flags]
        private enum arm_state
        {
            NULL = 0,
            CLOSE = 1,
            DOWN = 2,
        }
       [SerializeField] arm_state STATE = arm_state.NULL;
        [SerializeField] int Int_State;
        public int Arm_State => (int)STATE;

        public int Temperture => thermistor.Temperature;

        public void SetArmInput(int deg1, int deg2)
        {
            if (STATE.HasFlag(arm_state.DOWN) && !STATE.HasFlag(arm_state.CLOSE))
            {
                if (deg1 >= 90) arm_unit.Pick();
            }
            if (STATE.HasFlag(arm_state.CLOSE))
            {
                if (deg1 < 90) arm_unit.Place();
            }

            if (deg1 < 90)
            {
                int m = 2;
                var tmp = (int)STATE & m;
                STATE = (arm_state)tmp;
            }
            else
            {
                STATE |= arm_state.CLOSE;
            }

            if (deg2 < 90)
            {
                int m = 1;
                var tmp = (int)STATE & m;
                STATE = (arm_state)tmp;
            }
            else
            {
                STATE |= arm_state.DOWN;
            }
        }

        [SerializeField] Arm arm_unit;
        [SerializeField] Thermistor thermistor;

        private void Update()
        {
            Int_State = (int)STATE;
        }
    }
}