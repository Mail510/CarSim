using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarSimulator {
    public class Arm : MonoBehaviour
    {
        [SerializeField] string obj_tag = "obj";

        private Transform hit_obj;
        private Transform pick_obj;

        public void Pick()
        {
            if (hit_obj != null) pick_obj = hit_obj;
        }

        public void Place()
        {
            pick_obj = null;
        }

        private void Update()
        {
            if (pick_obj != null)
            {
                pick_obj.position = transform.position;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == obj_tag) hit_obj = other.gameObject.transform;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == obj_tag) hit_obj = null;
        }
    }
}
