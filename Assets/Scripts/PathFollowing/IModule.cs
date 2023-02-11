using PathUnit;
using UnityEngine;

public interface IModule
{
    void SetArmInput(int deg1,int deg2);

    int Arm_State { get; }

    int Temperture { get; }
}
