using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinFall : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EventManager.MannequinFall += EventFunction;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void EventFunction()
    {
        EventManager.MannequinFall -= EventFunction;

        GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -2.5f), ForceMode.Impulse);

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(Sons.instance.EfeitoDramatico2);

        audioSource.clip = Sons.instance.ManequimCaicompleto;
        audioSource.PlayDelayed(.4f);
    }
}
