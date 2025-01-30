using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AI;

public class Person : MonoBehaviour
{


    public NavMeshAgent agent;

    public AudioClip otherClip;
    public AudioSource audioSource;
    public GameObject tornado;
    public float moveSpeed;

    public List<GameObject> safePts;
    private float distanceToTornado = 0;

    public void Scream()
    {
        audioSource.clip = otherClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (!transform.GetComponent<Rigidbody>().freezeRotation)
        {
            agent.enabled = false;
            this.enabled = false;            
            return;
        }
      
        if (tornado != null)
        {            
            distanceToTornado = (transform.position - tornado.transform.position ).magnitude;
            float distance = distanceToTornado;
            GameObject safestPt = safePts[0];
            foreach (GameObject safePt in safePts)
            {
                if (safePt != null)
                {
                    Vector3 distTornadoToSafePt = safePt.transform.position - tornado.transform.position;
                    Vector3 distToTornado = tornado.transform.position - transform.position;

                    if (distTornadoToSafePt.magnitude > distance && Vector3.Dot(distTornadoToSafePt,distToTornado) < 0)
                    {
                        safestPt = safePt;
                        distance = distTornadoToSafePt.magnitude;
                    }
                }
            }
            agent.SetDestination(safestPt.transform.position);
        }
    }
}
