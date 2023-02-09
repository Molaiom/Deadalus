using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour {

    public Vector3 pushDirection;

    public void PushCover()
    {
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Rigidbody>().AddForce((pushDirection * 70), ForceMode.Force);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(Sons.instance.DutoTampaCai, audioSource.volume);
    }
}
