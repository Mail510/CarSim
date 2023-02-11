using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class Player : MonoBehaviour
{
    private SerialPort serialPort;
    // Start is called before the first frame update
    void Start()
    {
        // SerialPort�̑�1������ArduinoIDE�Őݒ肵���V���A���|�[�g��ݒ�
        // ArduinoIDE�̉E������m�F�ł���
        serialPort = new SerialPort("COM4", 57600); // �����������̐ݒ肵���V���A���|�[�g���ɕς��邱��
        serialPort.Open();
    }

    // Update is called once per frame
    void Update()
    {
        if (serialPort.IsOpen)
        {
            string data = serialPort.ReadLine();
            Debug.Log(data);
        }
    }
}