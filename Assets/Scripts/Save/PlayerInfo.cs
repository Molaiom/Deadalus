using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private float[] playerPosition = new float[3];
    private int tapesListened = 0;
    //MAKE INVENTORY    


    public void SetPosition(GameObject player)
    {
        playerPosition[0] = player.transform.position.x;
        playerPosition[1] = player.transform.position.y;
        playerPosition[2] = player.transform.position.z;
    }

    public void SetTapesListened(int i)
    {
        if (i >= 0 && i <= 5)
            tapesListened = i;
    }

    public float GetPosition(int index)
    {
        if (index >= 0 && index <= 2)
            return playerPosition[index];
        else
        {
            Debug.LogError("Invalid Index.");
            return 0;
        }

    }

    public int GetTapesListened()
    {
        return tapesListened;
    }

}
