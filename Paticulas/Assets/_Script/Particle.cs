using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Particle : MonoBehaviour
{
    public float r;
    public float g;
    public float m;
    public Vector3 f;
    public Vector3 a;
    public Vector3 prevPos;
    public Vector3 currPos;
    public Vector3 airForce;
    public float restitution;
    public Vector3 color;

    // Start is called before the first frame update
    void Start()
    {

    }
    void CheckFloor()
    {
        if (currPos.y < r)
        {
            prevPos.y = currPos.y;
            currPos.y = r;
            f.y = -f.y * restitution;
            a = f / m;
        }
    }

    void CheckWall()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(currPos.y - prevPos.y) < 0.00001f && Mathf.Abs(currPos.y - r) < 0.00001f)
        {
            currPos.y = r;
            prevPos.y = r;
            f.y = 0;
        }
        else
        {
            f.y = -m * g;
            if(currPos.y != prevPos.y)
            {
                Vector3 vel = (currPos - prevPos) / Time.deltaTime;
                if(currPos.y > prevPos.y)
                {
                    f.y = f.y - r * 0.001f * vel.magnitude;
                }else if(currPos.y < prevPos.y)
                {
                    f.y = f.y + r * 0.001f * vel.magnitude;
                }
            }
        }

        a = f / m;
        Vector3 temp = currPos;
        float dt = Time.deltaTime;
        if(Time.frameCount > 100)
        {
            currPos = 2 * currPos - prevPos + a * dt * dt;
            prevPos = temp;
            CheckFloor();
            CheckWall();
        }
        transform.localPosition = currPos;
    }

    public bool InCollision(Particle anotherParticle)
    {
        float sumR2 = Mathf.Pow(r + anotherParticle.r, 2);
        Vector3 p1 = transform.localPosition;
        Vector3 p2 = anotherParticle.transform.localPosition;
        float dx = p2.x - p1.x;
        float dy = p2.y - p1.y;
        float dz = p2.z - p1.z;
        float d2 = dx * dx + dy * dy + dz * dz;
        return d2 < sumR2;
    }

}