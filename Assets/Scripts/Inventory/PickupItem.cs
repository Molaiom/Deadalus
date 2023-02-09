using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour, IInventoryItem {

    #region Leo Stuff
    public int _id;

    public int id
    {
        get
        {
            return _id;
        }
    }

    public TypeItem _type;

    public TypeItem type
    {
        get
        {
            return _type;
        }
    }

    public string _name;

    public string nameItem {
        get
        {
            return _name;
        }
    }

    public Sprite _image;

    public Sprite image
    {
        get
        {
            return _image;
        }
    }

    public GameObject _model;

    public GameObject model
    {
        get
        {
            return _model;
        }
    }

    public Transform _position;

    public Transform position
    {
        get
        {
            return GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
    #endregion

    public void OnPickup()
    {
        if(type == TypeItem.Tape)
        {
            if (id == 1)
            {
                SequenceManager.instance.Sequence3Trigger();
            }
            
            if(id == 3)
            {
                
            }
        }

        gameObject.SetActive(false);
    }

    #region Events
    public void OnEnable()
    {
        if (type == TypeItem.Lantern) 
            SequenceManager.FlashlightEvent += RollingEvent;


        if (id == 1 && type == TypeItem.Tape)
        {
            SequenceManager.RadioRoomOpening += FirstTapeEvent;            
        }

    }

    public void RollingEvent()
    {
        if (this != null)
        {
            if (type == TypeItem.Lantern)
            {
                // PLAY SOUND
                gameObject.GetComponent<AudioSource>().PlayOneShot(Sons.instance.LanternaArrastando, gameObject.GetComponent<AudioSource>().volume);

                gameObject.GetComponent<Rigidbody>().AddForce(65, 25, 700, ForceMode.Force);

                DoorManager.instance.OpenCloseDoorALittle(21, true);
                SequenceManager.FlashlightEvent -= RollingEvent;
            }
        }
    }

    public void FirstTapeEvent()
    {
        if (this != null)
        {
            if (id == 1 && type == TypeItem.Tape)
            {
                SequenceManager.RadioRoomOpening -= FirstTapeEvent;                

                gameObject.GetComponent<AudioSource>().PlayOneShot(Sons.instance.PortaNormalAbrindo, gameObject.GetComponent<AudioSource>().volume);
                LightManager.instance.SetLightState(0, false);
                LightManager.instance.SetLightState(2, true);

                DoorManager.instance.OpenCloseDoor(1, true);
                DoorManager.instance.LockUnlockDoor(1, true);
                
            }
        }
    }
    #endregion
}
