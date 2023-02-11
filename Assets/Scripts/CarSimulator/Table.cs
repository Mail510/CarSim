using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarSimulator
{
    public class Table : MonoBehaviour
    {
        [SerializeField] int temperature;
        [SerializeField] int range;
        [SerializeField, Range(0, 100)] int outlier_frequency;

        public int Temperature;
        private void Update()
        {
            var error = Random.Range(-range / 2, range / 2);

            if (Random.Range(0, 100) < outlier_frequency) error *= 5;

            Temperature = temperature + error;
        }
    }
}