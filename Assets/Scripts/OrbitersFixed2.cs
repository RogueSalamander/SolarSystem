using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitersFixed2 : MonoBehaviour
{
    // Newtons Law
    // F = G ((m1*m2) / r squared)
    // F = mA

    public int sphereCount = 500;
    public int maxRadius = 200;

    public float gravity = 6.73f;
    public float sunMass = 100; // maybe too much? 
    public float sphereMassScale = 2; // depends on actual scale of spheres 
    public float initForceScale = 100;

    public GameObject[] spheres;
    public Material[] mats;
    public Material trailMat;

    private void Awake()
    {
        spheres = new GameObject[sphereCount];
    }

    // Start is called before the first frame update
    void Start()
    {
        spheres = CreateSpheres(sphereCount, maxRadius);

        foreach (GameObject s in spheres)
        {
            s.GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere * initForceScale, ForceMode.Acceleration);
        }
    }

    public GameObject[] CreateSpheres(int count, int radius)
    {
        var sphs = new GameObject[count];
        var sphereToCopy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Rigidbody rb = sphereToCopy.AddComponent<Rigidbody>();
        rb.useGravity = false;

        for (int i = 0; i < count; i++)
        {
            var sp = GameObject.Instantiate(sphereToCopy);
            sp.transform.position = this.transform.position +
                                        new Vector3(Random.Range(-maxRadius, maxRadius),
                                                    Random.Range(-10, 10),
                                                    Random.Range(-maxRadius, maxRadius));
            sp.transform.localScale *= Random.Range(0.5f, 1);
            sp.GetComponent<Renderer>().material = mats[Random.Range(0, mats.Length)];
            TrailRenderer tr = sp.AddComponent<TrailRenderer>();
            tr.time = 1.0f;
            tr.startWidth = 0.1f;
            tr.endWidth = 0;
            tr.material = trailMat;
            tr.startColor = new Color(1, 1, 0, 0.1f);
            tr.endColor = new Color(0, 0, 0, 0);
            spheres[i] = sp;
        }

        GameObject.Destroy(sphereToCopy);

        return spheres;

    }

    void FixedUpdate()
    { // Loop through each sphere totaling up and applying gravitational forces 
        foreach (GameObject s in spheres)
        {
            Vector3 totalForce = new Vector3(0, 0, 0), forceDirection;
            float forceAmount; float sphereMass = s.transform.localScale.x * sphereMassScale;

            // first calculate sun's gravity by multiplying direction and force amount 
            forceDirection = (transform.position - s.transform.position).normalized;
            forceAmount = getForce(sphereMass, sunMass, Vector3.Distance(transform.position, s.transform.position));
            totalForce += forceDirection * forceAmount;

            // this is optional, use if you want sphere gravity to affect each other (moons) 
            foreach (GameObject p in spheres)
            {
                if (!p.Equals(s))
                { // don't check itself 
                    forceDirection = (p.transform.position - s.transform.position).normalized;
                    forceAmount = getForce(sphereMass, p.transform.localScale.x * sphereMassScale, Vector3.Distance(p.transform.position, s.transform.position));
                    totalForce += forceDirection * forceAmount;
                }
            }
            //apply all forces to sphere 
            s.GetComponent<Rigidbody>().AddForce(totalForce, ForceMode.Acceleration);
        }
    }

    // return F=(Gm2m2)/d^2
    float getForce(float mass1, float mass2, float distance)
    {

        return (gravity * mass1 * mass2) / Mathf.Pow(distance, 2);

    }
}
