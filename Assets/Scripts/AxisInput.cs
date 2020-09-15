using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisInput : MonoBehaviour
{
    public delegate void ChangeAction(Axis axis, float value);
    public static event ChangeAction OnChange;

    public Axis axis;

    public void Change(string value)
    {
        float newValue = 0;
        if(float.TryParse(value, out newValue))
        {
            if (OnChange != null)
                OnChange(axis, newValue);
        }
    }
}
