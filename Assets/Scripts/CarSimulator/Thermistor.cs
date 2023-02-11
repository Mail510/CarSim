using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarSimulator
{
    public class Thermistor : MonoBehaviour
    {
        [SerializeField] string tmp_tag = "tmp";

        public int Temperature = 1000;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == tmp_tag) Temperature = other.gameObject.GetComponent<Table>().Temperature;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == tmp_tag) Temperature = 1000;
        }
    }
}