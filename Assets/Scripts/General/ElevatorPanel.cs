using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPanel : MonoBehaviour
{
    #region Attributes
    public Material buttonMaterial;
    public ElevatorDoor elevator;
    public int id;

    public bool activated = false;
    private AudioSource aSource;
    private int timesClicked = 0;
    #endregion

    #region Methods
    private void Awake() 
    {
        aSource = GetComponent<AudioSource>();
    }

    public void ChangeActiveState(bool activatedState) // ACTIVATES AND DEACTIVATES THE PANEL 
    {
        activated = activatedState;
    }

    public void OpenElevator() // OPENS THE ELEVATOR 
    {
        if (elevator != null && activated)
        {
            if (!aSource.isPlaying)
            {                
                aSource.PlayOneShot(Sons.instance.ElevadorAbre, aSource.volume);
            }
            
            elevator.activated = true;
        }       
    }

    public void CloseElevator() // CLOSES THE ELEVATOR 
    {
        if (elevator != null)
        {
            aSource.PlayOneShot(Sons.instance.ElevadorBotao, aSource.volume);
            aSource.PlayOneShot(Sons.instance.ElevadorFecha, aSource.volume);
            elevator.activated = false;

            if(id == 2)
            {
                SequenceManager.instance.secondFloorElevatorPanel2.gameObject.SetActive(true);
            }
            if(id == 5)
            {
                FindObjectOfType<PlayerController>().AudioSourceFade(false, 3, 0);
                ChangeActiveState(false);
            }
        }
    }

    public int TimesClicked
    {
        get
        {
            return timesClicked;
        }

        set
        {
            timesClicked = value;
            aSource.PlayOneShot(Sons.instance.ElevadorBotao, aSource.volume);
        }
    }
    #endregion

}
