using System.Collections;
using UnityEngine;

public class Door2 : DetectableByJanitor
{

    public int id;
    public Types type;
    public bool isLocked;
    public bool hasAlarm;
    public bool inverseDoor;
    
    private bool animateDoor = false;
    private Vector3 localAnimateDoor;
    private float timeAnimate = 2f;
    private float startTime;
    private bool doorOpening;

    private Vector3 inicialRotation;
    private bool glassBroken = false;
    private AudioSource aSource;

    // Use this for initialization
    void Start()
    {
        aSource = GetComponent<AudioSource>();

        inicialRotation = transform.localRotation.eulerAngles;

        DetectionValue = 0;

        if (id == 17)
        {
            EventManager.OpenFireDoorTape3 += OpenFireDoorALittleTape3;
            EventManager.CloseFireDoor += CloseFireDoor;
            EventManager.OpenFireDoor += OpenFireDoorALittle;
        }

        if (id == 21)
        {
            EventManager.CloseInicialDoor += CloseNormalDoor;
        }
    }

    void FixedUpdate()
    {
        if (animateDoor)
        {
            AnimateDoorOpen();
        }
    }

    public bool DoorIsOpen()
    {
        float angle = Quaternion.Angle(Quaternion.Euler(inicialRotation), Quaternion.Euler(transform.localRotation.eulerAngles));

        if (angle < 65 && !animateDoor)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public float GetAngle()
    {
        return Quaternion.Angle(Quaternion.Euler(inicialRotation), Quaternion.Euler(transform.localRotation.eulerAngles));
    }

    public void LockDoor()
    {
        if (Types.Wardrobe != type)
        {
            isLocked = true;
        }
    }

    public void OpenDoor()
    {
        glassBroken = true;

        switch (type)
        {
            case Types.Key:
                aSource.PlayOneShot(Sons.instance.PortaNormalAbrindo, aSource.volume);
                break;

            case Types.Glass:
                aSource.PlayOneShot(Sons.instance.PortaVidroAbrindo, aSource.volume);
                break;

            case Types.Fire:
                aSource.PlayOneShot(Sons.instance.PortaCortafogoAbrindo, aSource.volume);
                break;

            case Types.Eletronic:
                aSource.PlayOneShot(Sons.instance.PortaEletronicaAbrindo, aSource.volume);
                break;
        }

        var rotation = Quaternion.Euler(transform.localRotation.eulerAngles);
        rotation *= Quaternion.Euler(80, 0, 0);
        
        isLocked = false;
        animateDoor = true;

        startTime = Time.time;

        doorOpening = true;
        localAnimateDoor = rotation.eulerAngles;

    }

    public void OpenDoorALittle()
    {
        isLocked = false;
        animateDoor = true;

        var rotation = Quaternion.Euler(transform.localRotation.eulerAngles);
        rotation *= Quaternion.Euler(30, 0, 0);

        startTime = Time.time;
        doorOpening = true;

        localAnimateDoor = rotation.eulerAngles;
    }

    public void OpenFireDoorALittleTape3()
    {
        EventManager.OpenFireDoorTape3 -= OpenFireDoorALittleTape3;
        aSource.PlayOneShot(Sons.instance.PortaCortafogoLentamente, aSource.volume);
        isLocked = false;
        animateDoor = true;

        var rotation = Quaternion.Euler(transform.localRotation.eulerAngles);
        rotation *= Quaternion.Euler(15, 0, 0);

        startTime = Time.time;
        doorOpening = true;

        localAnimateDoor = rotation.eulerAngles;
    }

    public void OpenFireDoorALittle()
    {
        EventManager.OpenFireDoor -= OpenFireDoorALittle;
        aSource.PlayOneShot(Sons.instance.PortaCortafogoLentamente, aSource.volume);
        isLocked = false;
        animateDoor = true;

        var rotation = Quaternion.Euler(transform.localRotation.eulerAngles);
        rotation *= Quaternion.Euler(15, 0, 0);

        startTime = Time.time;
        doorOpening = true;

        localAnimateDoor = rotation.eulerAngles;
    }

    private void AnimateDoorOpen()
    {
        //float distCovered = (Time.time - startTime) * .01f;
        //float fracJourney = distCovered / timeAnimate;
        float distCovered = (Time.time - startTime);
        float fracJourney = distCovered / timeAnimate;

        if (doorOpening)
        {
            transform.localRotation = Quaternion.Euler(Vector3.Lerp(inicialRotation, localAnimateDoor, fracJourney));
        } else
        {
            transform.localRotation = Quaternion.Euler(Vector3.Lerp(transform.localRotation.eulerAngles, localAnimateDoor, fracJourney));
        }

        if (distCovered > timeAnimate)
        {
            animateDoor = false;
        }
    }

    public void CloseFireDoor()
    {
        EventManager.CloseFireDoor -= CloseFireDoor;

        aSource.PlayOneShot(Sons.instance.PortaCortafogoBatendoforte, aSource.volume);
        isLocked = true;
        
        DoorManager.instance.OpenCloseDoor(18, true);
        DoorManager.instance.LockUnlockDoor(19, true);
        LightManager.instance.SetLightState(8, true);
        JanitorSpawner.instance.SpawnJanitorOnGroundFloor();

        FindObjectOfType<PlayerController>().gameObject.GetComponent<AudioSource>().clip = Sons.instance.MusicaPreenchimento3;
        FindObjectOfType<PlayerController>().gameObject.GetComponent<AudioSource>().Play();

        animateDoor = true;

        startTime = Time.time;
        doorOpening = false;

        localAnimateDoor = inicialRotation;
    }

    public void CloseNormalDoor()
    {
        EventManager.CloseInicialDoor -= CloseNormalDoor;

        aSource.PlayOneShot(Sons.instance.PortaNormalFechando, 1);
        isLocked = true;
        animateDoor = true;

        startTime = Time.time;
        doorOpening = false;

        localAnimateDoor = inicialRotation;
    }

    public void CloseDoor()
    {
        if (transform.localRotation != Quaternion.Euler(inicialRotation))
        {
            switch (type)
            {
                case Types.Key:
                    aSource.PlayOneShot(Sons.instance.PortaNormalFechando, aSource.volume);
                    break;

                case Types.Glass:
                    aSource.PlayOneShot(Sons.instance.PortaVidroFechando, aSource.volume);
                    break;

                case Types.Fire:
                    aSource.PlayOneShot(Sons.instance.PortaCortafogoFechando, aSource.volume);
                    break;

                case Types.Eletronic:
                    aSource.PlayOneShot(Sons.instance.PortaEletronicaFechando, aSource.volume);
                    break;

                    
            }

            animateDoor = true;
            doorOpening = false;

            startTime = Time.time;
            localAnimateDoor = inicialRotation;
        }
    }

    public void RotateDoor(float rotDoor)
    {
        if (!isLocked)
        {
            if (type != Types.Fire)
            {
                float clampRotation = Mathf.Clamp(rotDoor, -2, 2);
                var rotation = Quaternion.Euler(transform.localRotation.eulerAngles);

                float rotateY = transform.localRotation.eulerAngles.y;
                float initialRotateY = inicialRotation.y;

                if (rotateY > 270)
                    rotateY -= 360;

                if (initialRotateY > 270)
                    initialRotateY -= 360;
                
                if (inverseDoor)
                {
                    //clampRotation *= -1;

                    if (rotateY > initialRotateY + 3 && rotateY <= initialRotateY + 75)
                    {
                        rotation *= Quaternion.Euler(clampRotation, 0, 0);
                        transform.localRotation = rotation;
                    }
                    else
                    {
                        if (rotateY > initialRotateY + 3 && rotateY <= initialRotateY + 80)
                        {
                            if (clampRotation < 0)
                            {
                                rotation *= Quaternion.Euler(clampRotation, 0, 0);
                                transform.localRotation = rotation;
                            }
                        }
                        else if (rotateY >= initialRotateY && rotateY <= initialRotateY + 75)
                        {
                            if (clampRotation > 0)
                            {
                                rotation *= Quaternion.Euler(clampRotation, 0, 0);
                                transform.localRotation = rotation;
                            }
                        }
                    }

                }
                else
                {
                    if (rotateY > initialRotateY - 75 && rotateY <= initialRotateY - 3)
                    {
                        rotation *= Quaternion.Euler(clampRotation * -1, 0, 0);
                        transform.localRotation = rotation;
                    }
                    else
                    {
                        if (rotateY >= initialRotateY - 80 && rotateY <= initialRotateY - 3)
                        {
                            if (clampRotation < 0)
                            {
                                rotation *= Quaternion.Euler(clampRotation * -1, 0, 0);
                                transform.localRotation = rotation;
                            }
                        }
                        else if (rotateY > initialRotateY - 75 && rotateY <= initialRotateY)
                        {
                            if (clampRotation > 0)
                            {
                                rotation *= Quaternion.Euler(clampRotation * -1, 0, 0);
                                transform.localRotation = rotation;
                            }
                        }
                    }
                }
            }
            else
            {
                float clampRotation = Mathf.Clamp(rotDoor, -3, 3);
                var rotation = Quaternion.Euler(transform.localRotation.eulerAngles);

                float angle = Quaternion.Angle(Quaternion.Euler(inicialRotation), Quaternion.Euler(transform.localRotation.eulerAngles));

                float rotateY = transform.localRotation.eulerAngles.y;
                float initialRotateY = inicialRotation.y;

                if (angle < 75)
                {
                    rotation *= Quaternion.Euler(clampRotation, 0, 0);
                    transform.localRotation = rotation;
                }
                else
                {
                    if (angle < 80 && rotateY > initialRotateY)
                    {
                        if (inverseDoor)
                        {
                            if (clampRotation > 0)
                            {
                                rotation *= Quaternion.Euler(clampRotation, 0, 0);
                                transform.localRotation = rotation;
                            }
                        } else
                        {
                            if (clampRotation < 0)
                            {
                                rotation *= Quaternion.Euler(clampRotation, 0, 0);
                                transform.localRotation = rotation;
                            }
                        }
                    }
                    else if (angle < 80 && rotateY < initialRotateY)
                    {
                        if (inverseDoor)
                        {
                            if (clampRotation < 0)
                            {
                                rotation *= Quaternion.Euler(clampRotation, 0, 0);
                                transform.localRotation = rotation;
                            }
                        }
                        else
                        {
                            if (clampRotation > 0)
                            {
                                rotation *= Quaternion.Euler(clampRotation, 0, 0);
                                transform.localRotation = rotation;
                            }
                        }
                    }

                }


            }
        }
    }

    public IEnumerator PlayAlarm()
    {
        aSource.PlayOneShot(Sons.instance.Alarme, aSource.volume);

        hasAlarm = false;
        DetectionValue = 5;

        yield return new WaitForSeconds(10f);

        DetectionValue = 0;
    }

    public IEnumerator BreakGlass()
    {
        aSource.PlayOneShot(Sons.instance.VidroquebrandoPorta, aSource.volume);
        glassBroken = true;
        hasAlarm = false;
        DetectionValue = 5;

        yield return new WaitForSeconds(7.5f);

        if (id == 22)
        {
            AudioSource playerSource = FindObjectOfType<PlayerController>().gameObject.GetComponent<AudioSource>();
            playerSource.clip = Sons.instance.MusicaTensão3;
            playerSource.Play();
        }
        DetectionValue = 0;
    }

    public void SoundLocked()
    {
        switch (type)
        {
            case Types.Key:
                aSource.PlayOneShot(Sons.instance.PortaNormalTrancada, aSource.volume);
                break;
            case Types.Glass:
                aSource.PlayOneShot(Sons.instance.PortaVidroTrancada, aSource.volume);
                break;
            case Types.Fire:
                aSource.PlayOneShot(Sons.instance.PortaCortafogoTrancada, aSource.volume);
                break;
        }
    }

    public void UnlockDoor()
    {
        switch (type)
        {
            case Types.Key:
                isLocked = false;
                aSource.PlayOneShot(Sons.instance.ChaveDestrancar, aSource.volume);
                break;
            case Types.Glass:

                if (glassBroken)
                {
                    isLocked = false;
                    aSource.PlayOneShot(Sons.instance.PortaVidroAbrindo, aSource.volume);
                }
                else
                {
                    StartCoroutine(BreakGlass());
                }
                break;
            default:
                isLocked = false;
                break;
        }
    }

    IEnumerator AnimateOpen()
    {
        yield return new WaitForEndOfFrame();
        float rotY = 1 * 250 * Mathf.Deg2Rad;

        gameObject.transform.Rotate(new Vector3(0, rotY, 0));

        StopCoroutine("AnimateOpen");
    }
}

public enum Types { Key, Glass, Eletronic, Fire, Wardrobe }