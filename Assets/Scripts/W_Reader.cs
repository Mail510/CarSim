using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using SerialComunicationUnit;
using Tools;

public class W_Reader : MonoBehaviour
{
    [SerializeField]public string PortName = "COM1";
    [SerializeField] int BaudRate = 57600;
    [SerializeField] bool UseCRC = false;
    private SerialComunication Serial;

    private string result = "";
    // Start is called before the first frame update
    void Start()
    {
        Serial = new SerialComunication(PortName, BaudRate);
    }

    float t = 0;
    // Update is called once per frame
    void Update()
    {
        byte[] buffer= new byte[11];
        
        if (Serial.Read(out buffer, 11, UseCRC))
        {
            float x, y, angle;
            x = ByteTranslate.ByteToInt(buffer[4], buffer[5]);
            y = ByteTranslate.ByteToInt(buffer[6], buffer[7]);
            angle = ByteTranslate.ByteToInt(buffer[8], buffer[9]);

            result += t + "," + x + "," + y + "\n";
        }
        t += Time.deltaTime;
        Debug.Log(t);
        if (t >= 1)
        {
            Debug.Log("ON");
            buffer = new byte[9];
            buffer[1] = (byte)100;
            buffer[3] = (byte)100;
            buffer[5] = (byte)0;
            buffer[7] = (byte)0;
            buffer[8] = (byte)1;
            Serial.Write(buffer, UseCRC);
        }
    }

    private void OnApplicationQuit()
    {
        Serial.EndSerial();
        StreamWriter sw = new StreamWriter("C:/Users/masaf/OneDrive/ƒhƒLƒ…ƒƒ“ƒg/Arduino/Result.txt", false);
        sw.Write(result);
        sw.Flush();
        sw.Close();
    }
}
