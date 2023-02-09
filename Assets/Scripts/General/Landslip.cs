using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Landslip : MonoBehaviour {

    public GameObject thirdFloor;
    public GameObject darkRoom;

    public GameObject player;

    public Image blackScreenOfTheDeath;

    // Use this for initialization
    void Start () {
        EventManager.Landslip += FloorFall;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator FloorFall()
    {
        //blackScreenOfTheDeath.gameObject.SetActive(true);
        //blackScreenOfTheDeath.GetComponent<Image>().CrossFadeAlpha(100, 1f, true);

        EventManager.Landslip -= FloorFall;
        AudioSource audioSource = GetComponent<AudioSource>();

        player.GetComponent<PlayerController>().lanternActive = false;
        player.GetComponent<PlayerController>().zippoActive = false;

        audioSource.PlayOneShot(Sons.instance.Chaodesabando);

        foreach (Transform obj in transform)
        {
            obj.gameObject.GetComponent<Collider>().enabled = false;
        }

        thirdFloor.SetActive(false);
        darkRoom.SetActive(true);

        //blackScreenOfTheDeath.GetComponent<Image>().CrossFadeAlpha(0, 5f, true);

        yield return new WaitForSeconds(5);

        audioSource.PlayOneShot(Sons.instance.LampadaAcender);
        LightManager.instance.SetLightState(9, true);
    }
}
