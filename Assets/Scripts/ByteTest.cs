using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ByteTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        byte dlm = 0xFF;
        int _dlm = (int)dlm;
        Debug.Log(dlm);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
