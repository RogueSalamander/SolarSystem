using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ellipse : MonoBehaviour
{
    public enum ELLIPSE_AXIS {y, z};
    public ELLIPSE_AXIS yOrZ_Axis = ELLIPSE_AXIS.y; 

    public float xAxis;
    public float yAxis;
    public float zAxis;

    public Ellipse (float xAxis, float yAxis, float zAxis)
    {
        this.xAxis = xAxis;
        this.yAxis = yAxis;
        this.zAxis = zAxis;
    }

    public Vector2 Evaluate (float t)
    {
        float angle = Mathf.Deg2Rad * 360f * t;
        float x = Mathf.Sin(angle) * xAxis;
        float y = Mathf.Cos(angle) * yAxis;
        float z = Mathf.Tan(angle * zAxis);
        if(yOrZ_Axis == ELLIPSE_AXIS.y)
        {
            return new Vector2(x, y);
        }
        else if (yOrZ_Axis == ELLIPSE_AXIS.z)
        {
            return new Vector2(x, z);
        }
        else
        {
            return new Vector2(x, y);
        }

    }

}
