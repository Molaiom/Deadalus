using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AwakingController : MonoBehaviour {

    public GameObject player;
    public Image blackScreenOfTheDeath;

    //private byte alpha = 100;

	// Use this for initialization
	void Start () {
        player.GetComponent<PlayerController>().ChangeRootState(true);
        SequenceManager.AwakingEvent += SceneStart;
    }
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<Animator>().IsInTransition(0))
        {
            player.transform.position = GetComponent<Camera>().transform.position;
            player.GetComponent<PlayerController>().playerCamera.SetActive(true);
            player.GetComponent<PlayerController>().ChangeRootState(false);
            player.GetComponent<PlayerController>().GetComponents<AudioSource>()[3].Play();
            blackScreenOfTheDeath.gameObject.SetActive(false);
            SequenceManager.AwakingEvent -= SceneStart;
            Destroy(gameObject);
        }
    }

    void SceneStart()
    {
        //AudioSource audioSource = player.GetComponent<AudioSource>();

        GetComponent<Animator>().Play("InicialCamAnimation");
        ModifyAlpha();
    }

    void ModifyAlpha()
    {
        blackScreenOfTheDeath.GetComponent<Image>().CrossFadeAlpha(0, 14f, true);
    }
}
