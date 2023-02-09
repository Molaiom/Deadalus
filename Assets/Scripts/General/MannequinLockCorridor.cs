using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinLockCorridor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EventManager.MannequinLockCorridor += MannequinLock;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void MannequinLock()
    {
        EventManager.MannequinLockCorridor -= MannequinLock;
        DoorManager.instance.LockUnlockDoor(20, true);
        foreach (Transform obj in transform)
        {
            obj.gameObject.SetActive(true);
        }
    }
}
