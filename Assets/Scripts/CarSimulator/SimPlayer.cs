using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarSimulator
{
    public class SimPlayer : MonoBehaviour
    {
        ICar Car;
        IModule Module;
        // Start is called before the first frame update
        void Start()
        {
            Car = GetComponent<ICar>();
            Module = GetComponent<IModule>();
        }

        // Update is called once per frame
        void Update()
        {
            int mr = 0, ml = 0, arm1 = 0, arm2 = 0;
            if (Input.GetKey(KeyCode.LeftArrow)) ml = 150;

            if (Input.GetKey(KeyCode.RightArrow)) mr = 150;

            if (Input.GetKey(KeyCode.DownArrow))
            {
                ml *= -1;
                mr *= -1;
            }

            if (Input.GetKey(KeyCode.W)) arm2 = 250;
            else if (Input.GetKey(KeyCode.S)) arm2 = 0;

            if (Input.GetKey(KeyCode.Q)) arm1 = 250;
            else if (Input.GetKey(KeyCode.E)) arm1 = 0;

            Car.SetInput(mr, ml);
            Module.SetArmInput(arm1, arm2);
        }
    }
}