using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Television : MonoBehaviour {

    public bool powerOn;

    public GameObject lightTv;

    private Material shader;

    // Use this for initialization
    void Start () {
        powerOn = true;
        shader = GetComponent<MeshRenderer>().materials[1];

        lightTv.GetComponent<Light>().enabled = false;
        ChangeState();
    }
	
	// Update is called once per frame
	void Update () {
    }

    public void ChangeState()
    {
        powerOn = !powerOn;
        AudioSource aSource = GetComponent<AudioSource>();

        if (powerOn)
        {
            aSource.Play();
            shader.SetFloat("_ResX", 1080);
            shader.SetFloat("_ResY", 1080);
            lightTv.GetComponent<Light>().enabled = true;

        } else
        {
            lightTv.GetComponent<Light>().enabled = false;
            aSource.Stop();
            shader.SetFloat("_ResX", 0);
            shader.SetFloat("_ResY", 0);
        }
    }
}
