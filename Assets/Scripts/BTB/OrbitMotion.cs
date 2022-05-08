﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitMotion : MonoBehaviour
{
    public Transform orbitingObject;
    public Ellipse orbitPath;

    [Range(0f, 1f)]
    public float orbitProgress = 0;
    public float orbitPeriod = 3f;

    public bool orbitActive = true;

    public Material trailMat;

    // Start is called before the first frame update
    void Start()
    {
        if(orbitingObject == null)
        {
            orbitActive = false;
            return;
        }

        SetOrbitingObjectPosition();

        StartCoroutine(AnimateOrbit());

        CreateTrail();
       
    }

    void CreateTrail()
    {
        TrailRenderer tr = orbitingObject.gameObject.AddComponent<TrailRenderer>();
        tr.time = 1.0f;
        tr.startWidth = 0.1f;
        tr.endWidth = 0;
        tr.material = trailMat;
        tr.startColor = new Color(1, 1, 0, 0.1f);
        tr.endColor = new Color(0, 0, 0, 0);
    }

    void SetOrbitingObjectPosition()
    {
        Vector2 orbitPos = orbitPath.Evaluate(orbitProgress);
        orbitingObject.localPosition = new Vector3(orbitPos.x, 0, orbitPos.y);
    }

    /*void SetOrbitingObjectPositionRelative()
    {
        Vector2 orbitPos = orbitPath.Evaluate(orbitProgress);
        orbitingObject.localPosition = new Vector3(orbitPos.x, 0, orbitPos.y);
    }*/

    IEnumerator AnimateOrbit()
    {
        if(orbitPeriod < 0.1f)
        {
            orbitPeriod = 0.1f;
        }

        float orbitSpeed = 1f / orbitPeriod;
        while (orbitActive)
        {
            orbitProgress += Time.deltaTime * orbitSpeed;
            orbitProgress %= 1f;
            SetOrbitingObjectPosition();
            yield return null;
        }


    }

}
