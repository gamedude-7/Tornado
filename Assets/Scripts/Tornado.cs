using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Tornado : MonoBehaviour
{
    
    public TMP_Text text;
    float num = 5;
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
    public float accelStrength = 50f;
    public float velStrength = 20f;
    CapsuleCollider cylinder;
   // string name;
    List<string> names;
    bool flag = false;
    public float liftForce = 500.0f;
    public GameObject centerOfTornado;

    void OnTriggerEnter(Collider o)
    {
        Vector3 prefabPos;
        if (!o.gameObject.name.Contains("Player"))
        {
            if (names.IndexOf(o.gameObject.name) == -1) 
            {
                names.Add(o.gameObject.name);
            }
            //name = o.gameObject.name;
            insideTornado = true;
            prefabPos = o.transform.position;
            prefabPos.y = transform.position.y;
            tornadoRadius = Mathf.Abs((o.transform.position - transform.position).magnitude);
            //o.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            o.gameObject.GetComponent<Rigidbody>().freezeRotation = false;            
            o.gameObject.GetComponent<Person>().Scream();
            num = 5 - names.Count;
            //}
            //flag = !flag;
            if (num == 0)
            {
                text.text = "YOU WIN";
            }
            else
            {
                text.text = num.ToString() + " Left";
            }
        }
    }

    void OnTriggerExit(Collider o)
    {
        //foreach (string name in names)
        //{
        //    if (o.gameObject.name.Contains(name))
        //    {
        //        if (insideTornado)
        //        {
        //            //if (flag)
        //            //{ 
        //            num--;
        //            //}
        //            //flag = !flag;
        //            if (num == 0)
        //            {
        //                text.text = "YOU WIN";
        //            }
        //            else
        //            {
        //                text.text = num.ToString();
        //            }
        //            //insideTornado = false;
        //            //names.Remove(name);
        //            //name = "";
        //        }
        //    }
        //}
    }


    // Start is called before the first frame update
    void Start()
    {
        //  insideTornado = false;
        names = new List<string>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 eyeOfTornado; // center of tornado				        
        
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Box"))
        {  
            eyeOfTornado = centerOfTornado.transform.position;
            //eyeOfTornado.y = go.transform.position.y;
            centripedalAcceleration = eyeOfTornado - go.transform.position;
            float radius = centerOfTornado.transform.localScale.z;
            if (centripedalAcceleration.magnitude < radius)
                insideTornado = true;
            else
                insideTornado = false;

            if (insideTornado)
            {
                tornadoRadius = Mathf.Abs((go.transform.position - transform.position).magnitude);
                foreach (string name in names)
                { 
                    if (go.gameObject.name == name)
                    {                 
                        tangentVelocity = Vector3.Cross(centripedalAcceleration, Vector3.up);                 
                        tornadoVelocity = velStrength;
                        tornadoAcceleration = tornadoVelocity * tornadoVelocity / tornadoRadius; // (a = v^2)/r
                        go.gameObject.GetComponent<Rigidbody>().freezeRotation = false;
                        Debug.Log("centripedalAcceleration: " + centripedalAcceleration);
                        Debug.Log("tornadoAcceleration: " + tornadoAcceleration);
                        Debug.Log("accelStrength: " + accelStrength);
                        Debug.DrawRay(go.gameObject.transform.position, centripedalAcceleration.normalized * tornadoAcceleration);
                        Debug.DrawRay(go.gameObject.transform.position, tangentVelocity.normalized * tornadoVelocity);
                        go.gameObject.GetComponent<Rigidbody>().AddForce(centripedalAcceleration.normalized * tornadoAcceleration * Time.fixedDeltaTime, ForceMode.Acceleration);
                        go.gameObject.GetComponent<Rigidbody>().AddForce(tangentVelocity.normalized * velStrength * Time.fixedDeltaTime, ForceMode.Acceleration);
                        go.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * liftForce * Time.fixedDeltaTime, ForceMode.Acceleration);
                    }
                }
            }
            else
            {
                foreach (string name in names)
                {
                    if (go.gameObject.name == name)
                    {
                        centripedalAcceleration = centerOfTornado.transform.position - go.transform.position;
                        Debug.DrawRay(go.gameObject.transform.position, centripedalAcceleration.normalized * accelStrength);
                        go.gameObject.GetComponent<Rigidbody>().AddForce(centripedalAcceleration.normalized * accelStrength, ForceMode.Acceleration);
                    }
                }
            }
        }        
        //else
        //{
        //    //foreach (Transform go in FindObjectsOfType(Character))
        //    foreach (GameObject go in GameObject.FindGameObjectsWithTag("Box"))
        //    {                
        //        if (!string.IsNullOrEmpty(name))
        //        {
                    
        //            if (go.gameObject.name == name && go.gameObject.transform.position.y > 1000)
        //            {                        
        //                go.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        //                Destroy(go);
        //                name = "";

        //                break;
        //            }
        //        }
        //    }
        //}
    }

}
