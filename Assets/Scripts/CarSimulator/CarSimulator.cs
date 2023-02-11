using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarSimulator
{
    public class CarSimulator : MonoBehaviour, ICar
    {
        [SerializeField] float tread;
        [SerializeField] float Max_Speed;
        [SerializeField] float AC_Rate;
        public float Tread { get => tread; }
        public Vector2 WheelSpeed { get => wheelSpeed; }
        private Vector2 wheelSpeed;
        public Vector2 Position { get => position; }
        private Vector2 position;
        public float Angle { get => angle; }
        private float angle = Mathf.PI/2;

        [SerializeField]private float UR, UL;
        CarView CarView;

        public void SetInput(float ur, float ul)
        {
            UR = ur;
            UL = ul;
        }

        private void Start()
        {
            CarView = GetComponent<CarView>();
        }

        void Update()
        {
            float ar = (Max_Speed * (UR / 255) - wheelSpeed.x) * AC_Rate;
            float al = (Max_Speed * (UL / 255) - wheelSpeed.y) * AC_Rate;
            wheelSpeed.x += ar;
            wheelSpeed.y += al;

            wheelSpeed.x =  Max_Speed *(UR / 255);
            wheelSpeed.y = Max_Speed * (UL / 255);

            float vr = wheelSpeed.x;
            float vl = wheelSpeed.y;

            float v = (vr + vl) / 2;
            float w = (vr - vl) / tread;

            position.x += v * Mathf.Cos(angle);
            position.y += v * Mathf.Sin(angle);
            angle += w;

            CarView.Pos = position;
            CarView.Angle = angle;
        }

        float PItoPI(float angle)
        {
            while (angle >= Mathf.PI * 2) angle -= Mathf.PI;
            while (angle <= -Mathf.PI * 2) angle += Mathf.PI;
            return angle;
        }
    }
}