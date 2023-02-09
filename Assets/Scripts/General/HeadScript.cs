using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadScript : MonoBehaviour
{
    private GameObject player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().gameObject;        
    }

    private void LateUpdate()
    {
        transform.LookAt(player.transform);
    }


}
