using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerialComunicationUnit;
using Tools;

public class CarPlayer : MonoBehaviour
{
    [SerializeField] string PortName = "COM1";
    [SerializeField] int BaudRate = 57600;
    [SerializeField] bool UseCRC = false;
    private CarView CarView;
    private SerialComunication Serial;
    // Start is called before the first frame update
    void Start()
    {
        CarView = GetComponent<CarView>();
        Serial = new SerialComunication(PortName, BaudRate);
    }


    // Update is called once per frame
    void Update()
    {
        byte[] buffer = new byte[16];
        if(Serial.Read(out buffer, 16, UseCRC)){
            float x, y, angle;
            x = ByteTranslate.ByteToInt(buffer[4], buffer[5]);
            y = ByteTranslate.ByteToInt(buffer[6], buffer[7]);
            angle = ByteTranslate.ByteToInt(buffer[8], buffer[9]);
            WheelSpeed.x = (float)(1e-3) * ByteTranslate.ByteToInt(buffer[10], buffer[11]);
            WheelSpeed.y = (float)(1e-3) * ByteTranslate.ByteToInt(buffer[12], buffer[13]);
            Temperture = (int)buffer[14];
            Arm_state = (int)buffer[15];

            tmp_value = (int)ByteTranslate.ByteToLong(buffer);

            CarView.Pos = new Vector2(x, y);
            CarView.Angle = angle*(float)(1e-4);
        }

        int mr = 0, ml = 0, arm1 = 0,arm2 = 0;
        if (Input.GetKey(KeyCode.LeftArrow)) ml = 150;
        else ml = UL;

        if (Input.GetKey(KeyCode.RightArrow)) mr = 150;
        else mr = UR;

        if (Input.GetKey(KeyCode.DownArrow))
        {
            ml *= -1;
            mr *= -1;
        }

        if (Input.GetKey(KeyCode.W)) arm2 = 250;
        else if (Input.GetKey(KeyCode.S)) arm2 = 0;
        else arm2 = Deg2;

        if (Input.GetKey(KeyCode.Q)) arm1 = 250;
        else if (Input.GetKey(KeyCode.E)) arm1 = 0;
        else arm1 = Deg1;

        buffer = new byte[9];
        if(mr >= 0)
        {
            buffer[0] = (byte)mr;
            buffer[1] = (byte)0;
        }
        else
        {
            buffer[0] = (byte)0;
            buffer[1] = (byte)Mathf.Abs(mr);
        }

        if(ml >= 0)
        {
            buffer[2] = (byte)ml;
            buffer[3] = (byte)0;
        }
        else
        {
            buffer[2] = (byte)0;
            buffer[3] = (byte)Mathf.Abs(ml);
        }

        buffer[5] = (byte)arm1;
        buffer[7] = (byte)arm2;
        buffer[8] = (byte)1;

        Serial.Write(buffer, UseCRC);
    }

    private void OnApplicationQuit()
    {
        Serial.EndSerial();
    }

    [SerializeField]private int UR=0, UL=0;
    public void SetInput(float ur,float ul)
    {
        UR = (int)ur;
        UL = (int)ul;
    }

    public Vector2 WheelSpeed;

    public int Temperture;
    public int tmp_value;
    public int Arm_state;

    private int Deg1, Deg2;
    public void SetArmInput(int deg1,int deg2)
    {
        Deg1 = deg1;
        Deg2 = deg2;
    }
}
