using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Person : MonoBehaviour
{
    public AudioClip otherClip;
    public AudioSource audioSource;
    public GameObject tornado;
    public float moveSpeed;
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
            this.enabled = false;
            return;
        }
      
        if (tornado != null)
        {
            
            Vector3 dir = transform.position - tornado.transform.position;
            
            dir.Normalize();
            Vector3 direction = dir;
            //dir = dir / 100;
            dir.y = 0;
            RaycastHit raycastHit;
            Debug.DrawRay(transform.position, direction  );
            if (Physics.Raycast(transform.position, direction , out raycastHit, direction.magnitude)) 
            {             
               //if (raycastHit.collider.name != "Plane")
               // { 
               
                    Debug.Log(this.name + "collided with " + raycastHit.collider.name);
                Debug.DrawRay(transform.position + direction, raycastHit.normal);
                //Debug.DrawRay(transform.position , direction + raycastHit.collider.transform.right);
                 //Debug.Break();
                // }               
                dir = direction + raycastHit.normal;
                dir.Normalize();
               // dir = dir / 100;
                dir.y = 0;
            }                
            this.gameObject.GetComponent<Rigidbody>().velocity = dir* moveSpeed;
           // this.gameObject.transform.Translate(dir);
            

           
        }
    }
}
