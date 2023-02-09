using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelFall : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EventManager.ShovelFall += Fall;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Fall()
    {
        EventManager.ShovelFall -= Fall;
        
        GetComponent<Rigidbody>().AddForce(new Vector3(-1f, 0, -1f), ForceMode.Impulse);

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = Sons.instance.PaCaindo;
        audioSource.PlayDelayed(.7f);
    }
}
