using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    const float G = 667.4f;
    float planetMass;
    public float initialFormationForce = 10;

    public static List<Attractor> Attractors;

    public Rigidbody rb;

    void Start()
    {
        GameObject sun = GameObject.FindGameObjectWithTag("Sun");
        planetMass = GetComponent<Rigidbody>().mass;

        if(this.gameObject.tag != "Sun")
        {
            this.gameObject.GetComponent<Rigidbody>().AddForce(sun.transform.forward * planetMass * initialFormationForce * GetComponent<PlanetStats>().density, ForceMode.Impulse);
        }
        
    }

    void OnEnable()
    {
        if (Attractors == null)
            Attractors = new List<Attractor>();

        Attractors.Add(this);
    }

    void OnDisable()
    {
        Attractors.Remove(this);
    }

    void FixedUpdate()
    {
        foreach (Attractor attractor in Attractors)
        {
            if(attractor != this)
                Attract(attractor);
        }
    }

    void Attract (Attractor objToAttract)
    {
        Rigidbody rbToAttract = objToAttract.rb;

        Vector3 direction = rb.position - rbToAttract.position;
        float distance = direction.magnitude;

        if (distance == 0f)
            return;

        float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);

    }
}
