using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    public AudioClip otherClip;
    public AudioSource audioSource;
    
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
         
    }
}
