using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Tornado : MonoBehaviour
{
    float tornadoVelocity;
    float accelStrengthMax = 20.0f;
    float velStrengthMax = 10.0f;
    private Vector3 centripedalAcceleration;
    private Vector3 tangentVelocity;
    private float tornadoAcceleration;
    private float timeSinceTriggered = 0.0f;
    private bool insideTornado = false;
    private float tornadoRadius;
    private bool incr = false;
    float accelStrength = 1.72956f;
    float velStrength = 1.475855f;
    CapsuleCollider cylinder;
    string name;

    void OnTriggerEnter(Collider o)
    {
        Vector3 prefabPos;
        if (!o.gameObject.name.Contains("Player"))
        {
            name = o.gameObject.name;
            insideTornado = true;
            prefabPos = o.transform.position;
            prefabPos.y = transform.position.y;
            tornadoRadius = Mathf.Abs((o.transform.position - transform.position).magnitude);

            o.gameObject.GetComponent<Person>().Scream();
          
        }
    }

    void OnTriggerExit(Collider o)
    {
        //if (o.gameObject.name.Contains(MainMenu.playerNameInput))
        //{
            insideTornado = false;
        //name = "";
        //}
    }


    // Start is called before the first frame update
    void Start()
    {
        insideTornado = false;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 eyeOfTornado; // center of tornado				

        if (insideTornado)
        {
            //foreach (Transform go in FindObjectsOfType(GameObject))
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Box"))
            {

                if (go.gameObject.name ==name)
                {
                    eyeOfTornado = transform.position;
                    eyeOfTornado.y = go.transform.position.y;
                    //centripedalAcceleration = go.transform.position - eyeOfTornado;
                    centripedalAcceleration = eyeOfTornado - go.transform.position;
                    //var quat : Quaternion = Quaternion.AngleAxis(90,Vector3.up);			
                    //tangentVelocity = quat * centripedalAcceleration;				

                    tangentVelocity = Vector3.Cross(centripedalAcceleration, Vector3.up);

                    if (tornadoRadius > 100)
                    {
                        tornadoRadius -= 100;// = centripedalAcceleration.magnitude--;
                                             //accelStrength = Mathf.Lerp(accelStrengthMax, 1, accelStrength);
                                             //velStrength = Mathf.Lerp(velStrengthMax, 1, velStrength);
                    }
                    else
                    {
                        tornadoRadius = 100;
                    }
                    /*	else if (tornadoRadius < 100)
                        {
                            incr = true;
                        }
                        else if (tornadoRadius > centripedalAcceleration.magnitude)
                        {
                            incr = false;
                        }
                        else if (incr == true)
                        {
                            tornadoRadius+=100;
                            accelStrength = Mathf.Lerp(1,accelStrengthMax, accelStrength);
                            velStrength = Mathf.Lerp(1,velStrengthMax, velStrength);
                        }*/
                    tornadoAcceleration = tornadoVelocity * tornadoVelocity / tornadoRadius; // (a = v^2)/r
                    go.gameObject.GetComponent<Rigidbody>().freezeRotation = false;                    
                    Debug.Log("centripedalAcceleration: " + centripedalAcceleration);
                    Debug.Log("tornadoAcceleration: " + tornadoAcceleration);
                    Debug.Log("accelStrength: " + accelStrength);

                    go.gameObject.GetComponent<Rigidbody>().AddForce(centripedalAcceleration.normalized * tornadoAcceleration * accelStrength, ForceMode.Acceleration);
                    go.gameObject.GetComponent<Rigidbody>().AddForce(tangentVelocity.normalized * tornadoVelocity * velStrength, ForceMode.VelocityChange);
                    go.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.VelocityChange);
                }
            }
        }
        else
        {
            //foreach (Transform go in FindObjectsOfType(Character))
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Box"))
            {                
                if (!string.IsNullOrEmpty(name))
                {
                    
                    if (go.gameObject.name == name)
                    {                        
                        go.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
                        Destroy(go);
                        name = "";
                    }
                }
            }
        }
    }

}
