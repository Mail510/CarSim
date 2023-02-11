using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmOffset : MonoBehaviour
{
    [SerializeField] Vector2 Offset;
    [SerializeField] Vector2 Target;
    [SerializeField] Vector2 Result;
    ICar Car;

    // Start is called before the first frame update
    void Start()
    {
        Car = GetComponent<ICar>();
    }

    // Update is called once per frame
    void Update()
    {
        var pos = Car.Position;

        var c = Vector2.Distance(pos, Target);
        var a = Mathf.Abs(Offset.x);
        var b = Mathf.Sqrt(Mathf.Pow(c, 2) - Mathf.Pow(a, 2));

        var c_theta = (Mathf.Pow(a, 2) + Mathf.Pow(b, 2) - Mathf.Pow(c, 2)) / 2 / a / b;
        var theta = Mathf.Acos(c_theta);

        var direction = Target - pos;
        direction.Normalize();

       Result = Quaternion.Euler(0, 0, theta) * direction * b;
    }
}
