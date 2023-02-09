using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerKeys
{
    public static string playerPositionXKey = "playerPositionX";
    public static string playerPositionYKey = "playerPositionY";
    public static string playerPositionZKey = "playerPositionZ";
    public static string tapesListenedKey = "tapesListened";
}

public class SaveManager : MonoBehaviour
{
    public void SaveGame()
    {
        PlayerInfo playerInfo = FindObjectOfType<PlayerInfo>();   
        
        PlayerPrefs.SetFloat(PlayerKeys.playerPositionXKey, playerInfo.GetPosition(0));
        PlayerPrefs.SetFloat(PlayerKeys.playerPositionYKey, playerInfo.GetPosition(1));
        PlayerPrefs.SetFloat(PlayerKeys.playerPositionZKey, playerInfo.GetPosition(2));
        PlayerPrefs.SetInt(PlayerKeys.tapesListenedKey, playerInfo.GetTapesListened());
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        PlayerInfo playerInfo = FindObjectOfType<PlayerInfo>();

        switch (playerInfo.GetTapesListened())
        {
            case 0:
                break;

            case 1:
                LoadPlayerPosition();
                break;

            case 2:
                LoadPlayerPosition();
                break;

            case 3:
                LoadPlayerPosition();
                break;

            case 4:
                LoadPlayerPosition();
                break;

            case 5:
                break;

            default:
                LoadPlayerPosition();
                break;
        }
    }

    private void LoadPlayerPosition()
    {
        PlayerInfo playerInfo = FindObjectOfType<PlayerInfo>();

        float posX = PlayerPrefs.GetFloat(PlayerKeys.playerPositionXKey);
        float posY = PlayerPrefs.GetFloat(PlayerKeys.playerPositionYKey);
        float posZ = PlayerPrefs.GetFloat(PlayerKeys.playerPositionZKey);
        Vector3 position = new Vector3(posX, posY, posZ);

        FindObjectOfType<PlayerController>().gameObject.transform.position = position;
    }
}
