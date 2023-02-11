using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICar 
{
    public float Tread { get; }
    public void SetInput(float ur, float ul);

    public Vector2 WheelSpeed { get; }

    public Vector2 Position { get; }

    public float Angle { get; }
}
