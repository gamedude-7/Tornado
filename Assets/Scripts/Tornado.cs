using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using FMODUnity;


public class Tornado : MonoBehaviour
{
    
    public TMP_Text text;
    float num = 5;
    float tornadoVelocity;

    private Vector3 centripedalAcceleration;
    private Vector3 tangentVelocity;
    private float tornadoAcceleration;
    
    private bool insideTornado = false;
    private float tornadoRadius;

    public float accelStrength = 50f;
    public float velStrength = 20f;
    CapsuleCollider cylinder;
   // string name;
    List<string> names;
    
    public float liftForce = 500.0f;
    public GameObject centerOfTornado;
    public GameObject music;
    public float time = 0;

    private StudioEventEmitter eventEmitter;

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
            num = 10 - names.Count;
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

        if (text.text == "YOU WIN")
        {
            time += Time.deltaTime;
            if (time > 10)
            {
                StopMusic();
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

        // Get the StudioEventEmitter component attached to the GameObject
        eventEmitter = music.GetComponent<StudioEventEmitter>();
        eventEmitter.Play();
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
                        //Debug.Log("centripedalAcceleration: " + centripedalAcceleration);
                        //Debug.Log("tornadoAcceleration: " + tornadoAcceleration);
                        //Debug.Log("accelStrength: " + accelStrength);
                        UnityEngine.Debug.DrawRay(go.gameObject.transform.position, centripedalAcceleration.normalized * tornadoAcceleration);
                        UnityEngine.Debug.DrawRay(go.gameObject.transform.position, tangentVelocity.normalized * tornadoVelocity);
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
                        UnityEngine.Debug.DrawRay(go.gameObject.transform.position, centripedalAcceleration.normalized * accelStrength);
                        go.gameObject.GetComponent<Rigidbody>().AddForce(centripedalAcceleration.normalized * accelStrength, ForceMode.Acceleration);
                    }
                }
            }
        }
        if (text.text == "YOU WIN")
        {
            time += Time.deltaTime;
            if (time > 10)
            {
                StopMusic();
              //  Destroy(music);
                SceneManager.LoadScene(2);
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            StopMusic();
        }
    }

    public void StopMusic()
    {
        if (eventEmitter != null)
        {
            eventEmitter.EventInstance.setPaused(true);
            //eventEmitter.Stop();
        }
    }
}
