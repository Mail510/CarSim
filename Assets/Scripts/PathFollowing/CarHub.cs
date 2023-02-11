using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHub : MonoBehaviour,ICar,IModule
{
    public float Tread => carView.Tread;

    public Vector2 WheelSpeed => carPlayer.WheelSpeed;

    public Vector2 Position => getPosition();
    private Vector2 getPosition()
    {
        var pos = Vector2.zero;
        pos = carView.Pos;
        return pos;
    }

    public float Angle => carView.Angle;

    public void SetInput(float ur, float ul)
    {
        carPlayer.SetInput(ur, ul);
    }

    public void SetArmInput(int deg1,int deg2)
    {
        carPlayer.SetArmInput(deg1, deg2);
    }

    public int Arm_State { get { return carPlayer.Arm_state; } }

    public int Temperture { get { return carPlayer.Temperture; } }

    private CarView carView;
    private CarPlayer carPlayer;
    // Start is called before the first frame update
    void Awake()
    {
        carView = GetComponent<CarView>();
        carPlayer = GetComponent<CarPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
