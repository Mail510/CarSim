using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarView : MonoBehaviour
{
    [SerializeField] public float Tread;
    [SerializeField] public float Length;
    [SerializeField] public float Scale;
    //[System.NonSerialized]
    public Vector2 Pos;
    //[System.NonSerialized]
    public float Angle;

    // Start is called before the first frame update
    void Start()
    {
       // transform.localScale = new Vector3(Length, (Tread + Length) / 2, Tread) * Scale;
    }

    // Update is called once per frame
    void Update()
    {
        var npos = new Vector3(Pos.x, 0, Pos.y) * Scale;
        npos.y = transform.position.y;
        transform.position = npos;
        transform.rotation = Quaternion.Euler(0, -Angle * Mathf.Rad2Deg, 0);
    }
}
