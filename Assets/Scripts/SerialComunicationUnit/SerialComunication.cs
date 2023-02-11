using System.IO.Ports;
using UnityEngine;

namespace SerialComunicationUnit
{
    public class SerialComunication
    {
        public string PortName { get; }
        public int BaudRate { get; }

        private SerialPort serial;

        private byte DLM = 0x80;
        private byte ESC = 0xFF;
        private byte CRC = 0x85;

        private int Parity = 0;

        public SerialComunication(string portName , int baudRate,int parity = 0)
        {
            PortName = portName;
            BaudRate = baudRate;
            Parity = parity;
            var ports = SerialPort.GetPortNames();

            bool check = false;
            for (int i = 0; i < ports.Length; i++)
            {
                Debug.Log(ports[i]);
                if (portName == ports[i]) check = true;
            }

            if (!check) Debug.LogError("NO PORT FIND");

            serial = new SerialPort(PortName, BaudRate);
            serial.ReadTimeout = 1500;
            serial.WriteTimeout = 1500;
            switch (Parity)
            {
                case 1:
                    serial.Parity = System.IO.Ports.Parity.Odd;
                    break;
                case 2:
                    serial.Parity = System.IO.Ports.Parity.Even;
                    break;
                default:
                    break;
            }

            serial.Open();
        }

        public void EndSerial()
        {
            serial.Close();
        }

        public void Write(byte[] data,bool crc)
        {
            if (crc)
            {
                int cnt = data.Length;
                byte[] buffer = new byte[cnt + 1];
                for (int i = 0; i < cnt; i++)
                {
                    buffer[i] = data[i];
                }
                buffer[cnt] = division(data);
                Write(buffer);
            }
            else
            {
                Write(data);
            }
        }

        public void Write(byte[] data)
        {
            int size = data.Length;
            byte[] buffer = new byte[size + 2];

            int esc = 0;
            for (int i = 0; i < size; i++)
            {
                if(data[i] == DLM)
                {
                    esc += 1 << i;
                    buffer[i] = ESC;
                }
                else
                {
                    buffer[i] = data[i];
                }
            }

            buffer[size] = (byte)esc;
            buffer[size + 1] = DLM;

            try
            {
                serial.Write(buffer, 0, size + 2);
            }
            catch (System.TimeoutException)
            {
                Debug.Log("WriteError");
            }
            
        }

        public bool Read(out byte[] data,int count,bool crc)
        {
            bool ans = false;
            data = null;
            if (crc)
            {
                byte[] buffer = new byte[count + 1];
                ans = Read(out buffer, count + 1);
                if (ans)
                {
                    data = new byte[count];
                    for (int i = 0; i < count; i++)
                    {
                        data[i] = buffer[i];
                    }
                    ans = (division(data) == buffer[count]);
                }
            }
            else
            {
                ans = Read(out data, count);
            }
            return ans;
        }

        public bool Read(out byte[] data,int count)
        {
            data = null;
            byte[] buffer = new byte[count+2];

            try
            {
                if (serial.ReadByte() != DLM) return false;
            }
            catch (System.TimeoutException)
            {
                Debug.Log("ReadTimeout");
            }


            try
            {
                int len = serial.Read(buffer, 0, count + 2);
            }
            catch (System.TimeoutException)
            {
                Debug.Log("ReadTimeout");
            }
            serial.DiscardInBuffer();

            data = new byte[count];


            for (int i = 0; i < count; i++)
            {
                if (((buffer[count] >> i) & 0x01) == 1) data[i] = DLM;
                else data[i] = buffer[i];
            }

            return true;
        }

        private byte division(byte[] data)
        {
            int count = data.Length;

            int crc = 0xFF;
            for (int i = 0; i < count; i++)
            {
                crc ^= data[i];
                for (int bit = 8; bit > 0 ; --bit)
                {
                    if((crc & 0x80) > 0)
                    {
                        crc = (crc << 1) ^ CRC;
                    }
                    else
                    {
                        crc = (crc << 1);
                    }
                }
            }

            return (byte)crc;
        }
    }
}