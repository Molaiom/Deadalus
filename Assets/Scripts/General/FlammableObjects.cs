using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammableObjects : DetectableByJanitor {

    private Material shader;
    public GameObject[] fires;
    public int id = 0;
    public float burningTimeLimit;
    [Range(1, 10)]
    public int detectionValueWhenBurning = 4;
    public bool isOnFire = false;   

    private float burningTime = 0f;
    private float timeStopLoop = 0f;

    // Use this for initialization
    void Start () {
        shader = GetComponent<MeshRenderer>().material;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isOnFire) {
            //Debug.Log(burningTime);

            DetectionValue = detectionValueWhenBurning;
            burningTime += Time.deltaTime / burningTimeLimit;
            shader.SetFloat("_SliceAmount", burningTime);
            
            if (timeStopLoop - 4.5f < Time.time)
            {
                foreach (GameObject fire in fires)
                {
                    ParticleSystem.MainModule main = fire.GetComponent<ParticleSystem>().main;
                    if (main.loop != false)
                    {
                        main.loop = false;
                    }
                }
            }

            if (burningTime >= 1)
            {
                if(id == 1)
                {
                    SequenceManager.instance.Sequence13Trigger();
                }
                Destroy(gameObject);
            }
        }
    }

    public void BurnMaterial()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = Sons.instance.ObjetosinflamaveisPilhadejornal;
        audioSource.Play();     
        isOnFire = true;

        timeStopLoop = Time.time + burningTimeLimit;

        foreach (GameObject fire in fires)
        {
            fire.SetActive(true);
        }

        if(id == 1)
        {
            SequenceManager.instance.Sequence12Trigger();
        }

        if(id == 2)
        {
            SequenceManager.instance.Sequence17Trigger();
        }
    }
}
