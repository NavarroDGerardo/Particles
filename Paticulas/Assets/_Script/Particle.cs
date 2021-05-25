using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Particle : MonoBehaviour
{
    public float r;
    public float g;
    public float m;
    public float lastfx, lastfz, lastfy;
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
        //The particles will explode from the emitter at(0,0, 0), with random forces in ±X ±Y and ±Z.
        

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

    void CheckRightWall()
    {
        
        //The simulation runs inside an imaginary cube of side20, centered at the origin.
        if (currPos.x > 10.0f - r)
        {
            /*prevPos.x = currPos.x;
            Vector3 vel = (currPos - prevPos) / Time.deltaTime;
            float speed = vel.magnitude;
            Vector3 direction = Vector3.Reflect(speed.normalized, Vector3.right)*/
            prevPos.x = currPos.x;
            currPos.x = 10.0f - r;
            f.x = -f.x * restitution;
            a = f / m;
            Debug.Log("Right Bounce");
            //f.x = -1 * restitution;;
            //f.x = -lastfx * restitution;
        }

    }

    void CheckLeftWall()
    {

        
        //The simulation runs inside an imaginary cube of side20, centered at the origin.
        if (currPos.x < -10.0f + r)
        {
            prevPos.x = currPos.x;
            currPos.x = -10.0f + r;
            //f.x = (vector reflejado) * rest
            f.x = -f.x * restitution;
            a = f / m;
            Debug.Log("Left Bounce");
            //f.x = 1 * restitution;;
            //f.x = -lastfx * restitution;
        }

    }

    void CheckTopWall()
    {
        
        //The simulation runs inside an imaginary cube of side20, centered at the origin.
        if (currPos.y > 20.0f - r && f.y > 0)
        {
            Debug.Log("Top Bounce");
            prevPos.y = currPos.y;
            currPos.y = 20.0f - r;
            //f.x = (vector reflejado) * rest
            f.y = -f.y * restitution;
            a = f / m;
      
            //f.x = 1 * restitution;;
        }

    }

    void CheckFrontWall()
    {
        
        //The simulation runs inside an imaginary cube of side20, centered at the origin.
        if (currPos.z > 10.0f - r)
        {
            
            prevPos.z = currPos.z;
            currPos.z = 10.0f - r;
            //f.x = (vector reflejado) * rest
            f.z = -f.z * restitution;
            a = f / m;
            Debug.Log("Front Bounce");
            //f.x = 1 * restitution;;
            //f.x = -lastfx * restitution;
            
        }

    }

    void CheckBackWall()
    {
        
        //The simulation runs inside an imaginary cube of side20, centered at the origin.
        if (currPos.z < -10.0f + r)
        {
            prevPos.z = currPos.z;
            currPos.z = -10.0f + r;
            //f.x = (vector reflejado) * rest
            f.z = -f.z * restitution;
            a = f / m;
            Debug.Log("Back Bounce");
            f.z =  1 * restitution;
            //f.z = -lastfz * restitution;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(currPos);
        
        
        if(Mathf.Abs(currPos.y - prevPos.y) < 0.00001f && Mathf.Abs(currPos.y - r) < 0.00001f)
        {
            currPos.y = r;
            prevPos.y = r;
            f.y = Random.Range(-15f, 15f);;
        }
        else
        {
            if (Time.realtimeSinceStartup > 7f){
                f.y = -m * g;
            }
            else{
                //Probar diferentes fuerzas
                f.y = -m * g * Random.Range(-3.5f, 3.5f);
                f.x = -m * g *  Random.Range(-0.2f, 0.2f);
                f.z = -m * g * Random.Range(-0.2f, 0.2f);
            }
          
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
            CheckLeftWall();
            CheckRightWall();
            CheckFrontWall();
            CheckBackWall();
            CheckTopWall();
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