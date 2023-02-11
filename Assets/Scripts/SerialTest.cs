using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerialComunicationUnit;

public class SerialTest : MonoBehaviour
{
    [SerializeField] string PortName = "COM4";
    [SerializeField] int BaudRate = 57600;
    [SerializeField] string Message = "HELLO!!";
    SerialComunication SC;

    // Start is called before the first frame update
    void Start()
    {
        SC = new SerialComunication(PortName, BaudRate);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.deltaTime);

        SC.Write(toByte(Message));

        byte[] buf = new byte[6];
        if (SC.Read(out buf, 12))
        {
            string txt = "";

            for (int i = 0; i < 6; i++)
            {
                int tmp = 0;
                tmp += (buf[i * 2]) & 0xFF;
                tmp += (buf[i * 2 + 1] << 8)&0xFF;
                txt += (char)tmp;
            }
            Debug.Log(txt);
        }
    }

    private void OnApplicationQuit()
    {
        SC.EndSerial();
    }

    byte[] toByte(string message)
    {
        byte[] buffer = new byte[message.Length*2];
        for (int i = 0; i < message.Length; i++)
        {
            char tmp = message[i];
            buffer[i * 2 + 1] = (byte)((tmp >> 8) & 0xFF);
            buffer[i*2] = (byte)(tmp & 0xFF);
        }

        return buffer;
    }
}
