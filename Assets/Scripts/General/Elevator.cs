using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EventManager.ElevatorFall += ElevatorFall;
    }

    void ElevatorFall()
    {
        EventManager.ElevatorFall -= ElevatorFall;

        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayDelayed(1f);
    }
}
