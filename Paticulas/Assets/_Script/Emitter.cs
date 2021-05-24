using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Emitter : MonoBehaviour
{
    public GameObject ParticlePrefab;
    public int numParticles;
    private GameObject[] particles;

    // Start is called before the first frame update
    void Start()
    {
        particles = new GameObject[numParticles];

        for (int p = 0; p < numParticles; p++)
        {
            GameObject part = Instantiate(ParticlePrefab);
            //Each particle will have a random radius in the range[0.3, 0.9].
            float diam = Random.Range(0.6f, 1.8f); 
            part.transform.localScale = new Vector3(diam, diam, diam);
            //Each particle will have a random material color.
            //The red diffuse channel in the range[0.2, 0.7](thefull red color will be reservedfor collisions). The green and blue diffuse channelsin the range[0.2, 1.0].
            Color c = new Color(Random.Range(0.2f, 0.7f), Random.Range(0.2f, 1f), Random.Range(0.2f, 1f));
            Renderer rend = part.GetComponent<Renderer>();
            rend.material.SetColor("_Color", c);
            particles[p] = part;

            // Get a reference to the Particle.cs script inside the prefab:
            Particle pScript = part.GetComponent<Particle>();
            pScript.color = new Vector3(c.r, c.g, c.b);
            pScript.r = diam / 2f;
            pScript.g = 9.81f;
            pScript.currPos = new Vector3(Random.Range(-5f, 5f), 10f, Random.Range(-5f, 5f));
            pScript.prevPos = pScript.currPos;
            //Each particle’s mass will equal the radius times 2.
            pScript.m = pScript.r * 2;
            pScript.restitution = 0.9f;
            pScript.f = Vector3.zero;

            
        }
    }
    // Update is called once per frame
    void Update()
    {
        //When two particles collide, they will change theirdiffuse color tored.
        if (Time.realtimeSinceStartup > 1.5f)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                bool didTouch = false;
                Particle p1 = particles[i].GetComponent<Particle>();
                for (int j = i + 1; j < particles.Length; j++)
                {
                    Particle p2 = particles[j].GetComponent<Particle>();
                    if (p1.InCollision(p2))
                    {
                        didTouch = true;
                        particles[i].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                        particles[j].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                    }
                }
                if (!didTouch)
                {
                    Color c = new Color(p1.color.x, p1.color.y, p1.color.z);
                    particles[i].GetComponent<Renderer>().material.SetColor("_Color", c);
                }
            }
        }
    }
}