using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : DetectableByJanitor
{
    #region Attributes
    [Header("Player Proporties")]
    public float movementSpeed;
    public float runSpeed;
    public float stamina;
    public float breath;
    public float camSensitivity;
    public float interactionDistance;
    public GameObject playerCamera;
    public GameObject otherCamera;
    private AudioSource aSource;
    private float originalVolume;
    public bool isDead = false;

    private float startingMovementSpeed;
    private float maximumStamina;
    private float maximumBreath;
    private bool isItHide = false;
    private bool isRooted = false;

    private bool rechargingStamina = false;
    private bool rechargingBreath = false;
    private float cameraMaxAngle = 80f;
    private float verticalRotation = 0;

    public bool openingDoor = false;
    public GameObject door;

    private float timer;
    private float cacheTimer = 3;
    private float holdButton = 0.4f;

    [Header("Lantern Proporties")]
    //public int timesToRecharge;
    public bool lanternActive = false;
    //public float battery;
    //public float maximumIntensity;
    public GameObject lantern;
    //public Light spotlight;

    [Header("Zippo Proporties")]
    public bool zippoActive = false;
    public GameObject zippo;

    private float maximumBattery;
    private bool soundPlaying = false;

    //Coletaveis
    public List<int> keys;
    public List<int> tapes;


    //Legendas
    [Header("Subtitles")]
    public Subtitle[] subtitle;

    //Cursor de interação
    [Header("Cursors")]
    public GameObject[] cursor;

    //Inventario
    [Header("Inventory Proporties")]
    public Inventory inventory;
    public bool inventoryOpen = false;
    public bool isInspecting = false;
    public bool isReproducingTape = false;
    public Canvas hud;
    public Image blackScreen;
    public GameObject inspectedItem;
    public IInventoryItem inspectedInterface;
    private float itemRotationSpeed = 50f;

    private CharacterController cc;

    [Header("Sonic Mode")]
    public bool sonicMode = false;
    public AudioSource sonicModeAudio;
    public Image sonicModeImage1;
    public Image sonicModeImage2;

    public float OriginalVolume
    {
        get
        {
            return originalVolume;
        }

        set
        {
            originalVolume = value;
        }
    }
    #endregion

    #region Main Methods
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        aSource = GetComponent<AudioSource>();
        OriginalVolume = aSource.volume;
        startingMovementSpeed = movementSpeed;
        maximumStamina = stamina;
        maximumBreath = breath;
        //maximumBattery = battery;

        ClearSubtitles(0);

        tapes = new List<int>();
        keys = new List<int>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Cursors
        UpdateCursor();

        // SONIC MODE

        if (Input.GetKeyDown(KeyCode.F12))
        {
            if (sonicMode)
            {
                sonicMode = false;

                startingMovementSpeed = 2.2f;
                runSpeed = 4;
                Time.timeScale = 1;

                sonicModeAudio.Pause();
                sonicModeImage1.enabled = false;
                sonicModeImage2.enabled = false;

            }
            else
            {
                sonicMode = true;

                startingMovementSpeed = 10;
                runSpeed = 25;
                Time.timeScale = 2;

                if (!sonicModeAudio.isPlaying)
                    sonicModeAudio.Play();
                else
                    sonicModeAudio.UnPause();

                sonicModeImage1.enabled = true;
                sonicModeImage2.enabled = true;
            }
        }

        if (sonicMode)
        {
            sonicModeImage1.enabled = !sonicModeImage1.enabled;
            sonicModeImage2.enabled = !sonicModeImage1.enabled;

            if (Camera.main != null)
            {
                float tempSpeed = cc.velocity.x - cc.velocity.z;
                if (tempSpeed < 0)
                {
                    tempSpeed *= -1;
                }
                Camera.main.fieldOfView = 60 + tempSpeed;
                Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 60, 120);
            }
        }
        else
        {
            if (Camera.main != null)
                Camera.main.fieldOfView = 60;
        }

        // SONIC MODE

        if (Input.GetKeyDown(KeyCode.F5))
        {
            FindObjectOfType<SaveManager>().SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            FindObjectOfType<SaveManager>().LoadGame();
        }
    }

    private void FixedUpdate()
    {
        // CONDITIONS
        if (true)
        {
            // Save
            FindObjectOfType<PlayerInfo>().SetPosition(gameObject);

            if (!isItHide)
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    //print("Locado");
                    Cursor.visible = false;
                }
                else
                {
                    //print("Não locado");
                    Cursor.visible = true;
                }

                if (Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.Escape))
                {
                    if (isReproducingTape)
                    {
                        Transform tapePanel = hud.transform.Find("TapePanel");
                        tapePanel.gameObject.SetActive(false);
                        Cursor.lockState = CursorLockMode.Locked;
                        isReproducingTape = false;
                    }
                    else
                    {
                        Cursor.lockState = CursorLockMode.Locked;

                    }
                }

                if (inventoryOpen)
                {
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        inventory.nextIndex();
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        inventory.UseItem(this);
                        inventoryOpen = false;
                        inventory.openOrCloseInventory(inventoryOpen);
                        isInspecting = false;
                        inventory.inspectItem(isInspecting);
                    }

                    if (Input.GetMouseButtonDown(1))
                    {
                        if (!isInspecting)
                        {
                            isInspecting = true;
                            inventory.inspectItem(isInspecting);
                        }
                        else
                        {
                            isInspecting = false;
                            inventory.inspectItem(isInspecting);
                        }
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        if (inventory.mItems.Count != 0)
                        {
                            inventoryOpen = true;
                            inventory.openOrCloseInventory(inventoryOpen);

                        }
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        openingDoor = false;
                        //door = null;
                    }

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        InteractObjects();
                    }

                    if (openingDoor && door != null)
                    {
                        InteractDoor();
                    }
                }

                //Se tiver inspecionando trava a camera e  movimento
                if (!isInspecting && !openingDoor && !isReproducingTape)
                {
                    MoveCharacter();
                    RotateCamera();
                }
                else
                {
                    if (inspectedItem != null)
                    {
                        float rotX = Input.GetAxis("Mouse X") * itemRotationSpeed * Mathf.Deg2Rad;
                        float rotY = Input.GetAxis("Mouse Y") * itemRotationSpeed * Mathf.Deg2Rad;

                        inspectedItem.transform.Rotate(Vector3.up, -rotX);
                        inspectedItem.transform.Rotate(Vector3.left, rotY);
                    }
                }
                /*
                if (lanternActive)
                {
                    if (battery <= 0)
                    {
                        battery = 0;
                        spotlight.intensity = 0;

                        if (Input.GetKeyDown(KeyCode.R))
                        {
                            AudioSource audioSource = GetComponent<AudioSource>();
                            audioSource.PlayOneShot(Sons.instance.LanternaGirarmanivela, audioSource.volume);

                            battery += maximumBattery / timesToRecharge;

                            if (spotlight.intensity + maximumBattery / 2 / timesToRecharge < maximumIntensity)
                            {
                                spotlight.intensity += maximumBattery / 2 / timesToRecharge;
                            }
                            else
                            {
                                spotlight.intensity = maximumIntensity;
                            }

                            DetectionValue = 8;
                        }
                    }
                    else if (battery > maximumBattery)
                    {
                        battery = maximumBattery;
                        spotlight.intensity = maximumIntensity;
                    }
                    else
                    {
                        battery -= (Time.fixedDeltaTime / 20);
                        spotlight.intensity -= (Time.fixedDeltaTime / 10);

                        if (Input.GetKeyDown(KeyCode.R))
                        {
                            AudioSource audioSource = GetComponent<AudioSource>();
                            audioSource.PlayOneShot(Sons.instance.LanternaGirarmanivela, audioSource.volume);

                            battery += maximumBattery / timesToRecharge;

                            if (spotlight.intensity + maximumBattery / 2 / timesToRecharge < maximumIntensity)
                            {
                                spotlight.intensity += maximumBattery / 2 / timesToRecharge;
                            }
                            else
                            {
                                spotlight.intensity = maximumIntensity;
                            }

                            DetectionValue = 8;
                        }
                    }
                }*/
            }
            else
            {
                lanternActive = false;
                zippoActive = false;
                DetectionValue = 0;
                playerCamera.GetComponent<Camera>().fieldOfView = 1;


                if (Input.GetMouseButtonDown(0))
                {
                    isItHide = false;
                    lanternActive = true;
                    playerCamera.GetComponent<Camera>().fieldOfView = 60;
                    DetectionValue = 3;
                }
            }

            if (zippoActive)
            {
                zippo.gameObject.SetActive(true);
            }
            else
            {
                zippo.gameObject.SetActive(false);
            }

            if (lanternActive)
            {
                lantern.gameObject.SetActive(true);
            }
            else
            {
                lantern.gameObject.SetActive(false);
            }
        }

    }

    private void LateUpdate()
    {
        AudioSource[] sounds = GetComponents<AudioSource>();
        if (!sounds[2].isPlaying)
        {
            ClearSubtitles(1.5f);
        }
    }
    #endregion

    #region Methods
    private void MoveCharacter() // MOVES THE CHARACTER WITH KEYBOARD
    {
        //Se tiver prendendo respiração não pode correr
        if (!isRooted)
        {
            if (Input.GetMouseButtonDown(1) && !rechargingBreath)
            {
                AudioSource playerAudio = GetComponents<AudioSource>()[3];
                playerAudio.Stop();
                playerAudio.PlayOneShot(Sons.instance.ProtagonistaRespiracaoTrancaAr, playerAudio.volume);
            }
            if (Input.GetMouseButtonUp(1) && !rechargingBreath)
            {
                AudioSource playerAudio = GetComponents<AudioSource>()[3];
                playerAudio.PlayOneShot(Sons.instance.ProtagonistaRespiracaoSoltaAr, playerAudio.volume);
            }

            if (Input.GetMouseButton(1) && !rechargingBreath)
            {
                if (!rechargingBreath)
                {
                    movementSpeed = startingMovementSpeed / 2;
                    breath -= Time.fixedDeltaTime / 5;
                    DetectionValue = 0;

                    if (breath < 0)
                    {
                        breath = 0;
                        rechargingBreath = true;
                    }
                }
            }
            else
            {
                if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && !rechargingStamina)
                {
                    movementSpeed = runSpeed;

                    if (cc.velocity != Vector3.zero)
                    {
                        stamina -= Time.fixedDeltaTime;
                        DetectionValue = 4;
                    }

                    if (stamina < 0)
                    {
                        stamina = 0;
                        rechargingStamina = true;
                    }
                }
                else
                {
                    movementSpeed = startingMovementSpeed;

                    if (stamina >= maximumStamina)
                    {
                        DetectionValue = 2;
                        stamina = maximumStamina;
                    }
                    else
                    {
                        stamina += (Time.fixedDeltaTime);
                        DetectionValue = 6;
                    }

                    if (stamina < 1)
                    {
                        rechargingStamina = true;
                    }
                    else
                    {
                        rechargingStamina = false;
                    }
                }

                breath += Time.fixedDeltaTime / 2;

                if (breath >= maximumBreath)
                {
                    breath = maximumBreath;

                    if (stamina >= maximumStamina)
                    {
                        AudioSource playerAudio = GetComponents<AudioSource>()[3];
                        playerAudio.Stop();
                    }
                }
                else if (breath > 2)
                {
                    rechargingBreath = false;
                    AudioSource playerAudio = GetComponents<AudioSource>()[3];

                    if (playerAudio.clip != Sons.instance.ProtagonistaRespiracaoNormal)
                    {
                        playerAudio.clip = Sons.instance.ProtagonistaRespiracaoNormal;
                        playerAudio.Play();
                    }
                }
                else
                {
                    AudioSource playerAudio = GetComponents<AudioSource>()[3];

                    if (playerAudio.clip != Sons.instance.ProtagonistaRespiracaoCorrendo)
                    {
                        playerAudio.PlayOneShot(Sons.instance.ProtagonistaRespiracaoSoltaAr, playerAudio.volume);
                        playerAudio.clip = Sons.instance.ProtagonistaRespiracaoCorrendo;
                        playerAudio.Play();
                    }
                }
            }

            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                cc.height -= 2f * Time.fixedDeltaTime;

                if (cc.height <= 0.2)
                {
                    cc.height = 0.2f;
                }
            }
            else
            {
                RaycastHit hit;

                cc.height += 2f * Time.fixedDeltaTime;

                if (cc.height >= 1.6)
                {
                    cc.height = 1.6f;
                }
                else
                {
                    if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.up, out hit, .3f))
                    {
                        cc.height = 0.2f;
                    }
                }
            }

            float movement1 = Input.GetAxis("Horizontal");
            float movement2 = Input.GetAxis("Vertical");

            Vector3 Move = new Vector3(movement1, 0, movement2);
            Move = transform.rotation * Move;
            cc.SimpleMove(Move * movementSpeed);

            if (cc.velocity != Vector3.zero)
            {
                AudioSource playerAudio = GetComponents<AudioSource>()[1];

                if (movementSpeed == runSpeed)
                {
                    if (playerAudio.clip != Sons.instance.ProtagonistaPisoconcretoCorrendo)
                    {
                        soundPlaying = false;
                        playerAudio.clip = Sons.instance.ProtagonistaPisoconcretoCorrendo;
                    }

                    AudioSource breathAudio = GetComponents<AudioSource>()[3];

                    if (breathAudio.clip != Sons.instance.ProtagonistaRespiracaoCorrendo)
                    {
                        breathAudio.clip = Sons.instance.ProtagonistaRespiracaoCorrendo;
                        breathAudio.Play();
                    }
                }
                else
                {
                    if (playerAudio.clip != Sons.instance.ProtagonistaPisoconcretoAndando)
                    {
                        soundPlaying = false;
                        playerAudio.clip = Sons.instance.ProtagonistaPisoconcretoAndando;

                        AudioSource breathAudio = GetComponents<AudioSource>()[3];

                        if (breathAudio.clip != Sons.instance.ProtagonistaRespiracaoNormal)
                        {
                            breathAudio.PlayOneShot(Sons.instance.ProtagonistaRespiracaoSoltaAr, playerAudio.volume);
                            breathAudio.clip = Sons.instance.ProtagonistaRespiracaoNormal;
                            breathAudio.Play();
                        }
                    }
                }

                if (!soundPlaying)
                {
                    playerAudio.Play();
                }

                soundPlaying = true;
            }
            else
            {
                AudioSource playerAudio = GetComponents<AudioSource>()[1];
                playerAudio.Stop();
                soundPlaying = false;

                movementSpeed = startingMovementSpeed;

                breath += Time.fixedDeltaTime / 2;
                stamina += (Time.fixedDeltaTime);

                if (stamina >= maximumStamina)
                {
                    stamina = maximumStamina;
                }

                if (breath >= maximumBreath)
                {
                    breath = maximumBreath;

                }
            }
        }
        else
        {
            AudioSource playerAudio = GetComponents<AudioSource>()[1];
            playerAudio.Stop();
            soundPlaying = false;
        }
    }

    private void RotateCamera() // MOVES THE CAMERA WITH MOUSE
    {
        // HORIZONTAL ROTATION
        float rotateHorizontal = Input.GetAxis("Mouse X") * camSensitivity;
        transform.Rotate(0, rotateHorizontal, 0);

        // VERTICAL ROTATION
        verticalRotation -= Input.GetAxis("Mouse Y") * camSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -cameraMaxAngle, cameraMaxAngle);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        otherCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

    }

    private void InteractObjects()
    {
        RaycastHit hit;

        if (!isInspecting)
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance))
            {
                GameObject hitted = hit.collider.gameObject;


                if (hitted.GetComponent<IInventoryItem>() != null)
                {
                    //InspectItem(hitted.gameObject.GetComponent<IInventoryItem>());
                    inventory.AddItem(hitted.gameObject.GetComponent<IInventoryItem>());
                    DestroyObject(hitted.gameObject);

                    switch (hitted.tag)
                    {
                        case "Fita":

                            if (hitted.GetComponent<PickupItem>().id == 3)
                            {
                                SequenceManager.instance.Sequence27Trigger();
                            }

                            tapes.Add(hitted.gameObject.GetComponent<IInventoryItem>().id);

                            break;
                        case "Key":
                            keys.Add(hitted.gameObject.GetComponent<IInventoryItem>().id);
                            break;
                        case "Lanterna":
                            InspectItem(hitted.gameObject.GetComponent<IInventoryItem>());
                            lanternActive = true;
                            zippoActive = false;
                            break;
                        case "Zippo":
                            InspectItem(hitted.gameObject.GetComponent<IInventoryItem>());
                            lanternActive = false;
                            zippoActive = true;
                            break;
                    }
                }
                else
                {
                    switch (hitted.tag)
                    {
                        case "Gravador":
                            Transform tapePanel = hud.transform.Find("TapePanel");
                            tapePanel.gameObject.SetActive(true);
                            Cursor.lockState = CursorLockMode.None;
                            Cursor.visible = true;
                            isReproducingTape = true;

                            for (int i = 0; i < tapes.Count; i++)
                            {
                                Transform tape = tapePanel.GetChild(tapes[i]);

                                Button button = tape.GetComponent<Button>();
                                button.interactable = true;
                            }
                            break;
                        case "Gerador":
                            hitted.gameObject.GetComponent<Generator>().InteractGenerator();
                            break;
                        case "Panel":
                            if (keys.Contains(-8))
                            {
                                DoorManager.instance.LockUnlockDoor(-8, true);
                            }
                            break;
                        case "TV":
                            hitted.gameObject.GetComponent<Television>().ChangeState();
                            break;
                        case "Flammable":
                            if (zippoActive)
                            {
                                if (hitted.gameObject.GetComponent<FlammableObjects>() != null)
                                {
                                    hitted.gameObject.GetComponent<FlammableObjects>().BurnMaterial();
                                }
                            }
                            break;
                        // ELEVADOR
                        case "Elevator Panel":
                            if (hitted.gameObject.GetComponent<ElevatorPanel>() != null)
                            {
                                ElevatorPanel elevator = hitted.gameObject.GetComponent<ElevatorPanel>();

                                if (elevator.activated)
                                {
                                    aSource.PlayOneShot(Sons.instance.ElevadorBotao, aSource.volume);
                                }
                                else
                                {
                                    aSource.PlayOneShot(Sons.instance.IdentificadordesenhaSenhaincorreta, aSource.volume);
                                }

                                if (elevator.id == 2) // THIS ELEVATOR WAS BUGGED
                                {
                                    if (tapes.Contains(3))
                                    {
                                        elevator.activated = true;
                                        elevator.OpenElevator();
                                    }
                                }

                                if (elevator.id == 1 || elevator.id == 4)
                                {
                                    elevator.OpenElevator();
                                }
                                if (elevator.id == 3)
                                {
                                    if (elevator.TimesClicked < 3)
                                    {
                                        elevator.TimesClicked += 1;
                                    }

                                    if (elevator.TimesClicked >= 3)
                                    {
                                        SequenceManager.instance.Sequence28Trigger();
                                    }
                                }
                                if (elevator.id == 5)
                                {
                                    SequenceManager.instance.Sequence31Trigger();
                                }
                            }

                            break;

                        case "Door":
                            if (hitted.gameObject.GetComponent<Door2>() != null)
                            {
                                bool unlock = false;

                                // EVENTO SALA VIVA ---
                                if (hitted.gameObject.GetComponent<Door2>().id == 4)
                                {
                                    SequenceManager.instance.Sequence6Trigger();
                                }
                                // EVENTO SALA VIVA ---

                                if (hitted.gameObject.GetComponent<Door2>().hasAlarm)
                                {
                                    if (hitted.gameObject.GetComponent<Door2>().id == 5)
                                    {
                                        hitted.gameObject.GetComponent<Door2>().StartCoroutine(hitted.gameObject.GetComponent<Door2>().PlayAlarm());
                                        EventManager.instance.Event2Trigger();
                                    }
                                }
                                else
                                {
                                    if (hitted.gameObject.GetComponent<Door2>().isLocked)
                                    {
                                        if (hitted.gameObject.GetComponent<Door2>().type == Types.Key)
                                        {
                                            for (int i = 0; i < keys.Count; i++)
                                            {
                                                if (keys[i] == hitted.gameObject.GetComponent<Door2>().id)
                                                {
                                                    unlock = true;
                                                    hitted.gameObject.GetComponent<Door2>().UnlockDoor();
                                                    keys.RemoveAt(i);
                                                }
                                            }
                                            if (!unlock)
                                            {
                                                hitted.gameObject.GetComponent<Door2>().SoundLocked();
                                            }
                                            else
                                            {
                                                openingDoor = true;
                                                door = hitted.gameObject;
                                                if (keys.Count == 0)
                                                {
                                                    inventory.RemoveItem("Key");
                                                }
                                            }
                                        }
                                        else if (hitted.gameObject.GetComponent<Door2>().type == Types.Glass)
                                        {
                                            hitted.gameObject.GetComponent<Door2>().UnlockDoor();

                                            // EVENT
                                            if (hitted.gameObject.GetComponent<Door2>().id == 12)
                                            {
                                                SequenceManager.instance.Sequence22Trigger();
                                            }
                                            // EVENT
                                        }
                                    }
                                    else
                                    {
                                        openingDoor = true;
                                        door = hitted.gameObject;
                                    }
                                }
                            }
                            else
                            {
                                openingDoor = true;
                                door = hitted.gameObject;
                            }

                            break;
                        case "Gaveta":
                            openingDoor = true;
                            door = hitted.gameObject;
                            break;
                        case "Cover":
                            timer = Time.time;
                            AudioSource audioSource = GetComponent<AudioSource>();
                            audioSource.PlayOneShot(Sons.instance.DutoTampaArrancada, audioSource.volume);
                            openingDoor = true;
                            door = hitted.gameObject;

                            break;
                    }
                }

                //Debug.Log(hitted.tag);
            }
        }
        else
        {
            inventory.AddItem(inspectedInterface);

            DestroyObject(inspectedItem);
            inspectedItem = null;
            isInspecting = false;
        }
    }

    private void InteractDoor()
    {
        //RaycastHit hit;

        //if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance))
        //{
        //GameObject hitted = hit.collider.gameObject;
        GameObject hitted = door;

        if (soundPlaying)
        {
            AudioSource playerAudio = GetComponents<AudioSource>()[1];
            playerAudio.Stop();
            soundPlaying = false;
        }


        switch (hitted.tag)
        {
            case "Door":
                if (hitted.gameObject.GetComponent<Door2>() != null)
                {
                    hitted.gameObject.GetComponent<Door2>().RotateDoor(Input.GetAxis("Mouse Y") * 250 * Mathf.Deg2Rad);
                    openingDoor = true;
                }
                else if (hitted.gameObject.GetComponent<WardrobeDoor>() != null)
                {
                    hitted.GetComponent<WardrobeDoor>().OpenDoor(Input.GetAxis("Mouse Y") * 250 * Mathf.Deg2Rad);
                    openingDoor = true;
                }

                break;
            case "Gaveta":
                float moveObj = Mathf.Clamp(Input.GetAxis("Mouse Y") * 2 * Mathf.Deg2Rad, -.1f, .1f);


                //Debug.Log(moveObj);

                if (hitted.gameObject.transform.localPosition.x >= hitted.gameObject.GetComponent<Chest>().initialPosition.x)
                {
                    if (moveObj > 0)
                    {
                        moveObj = 0;
                    }
                }

                if (Vector3.Distance(hitted.gameObject.transform.localPosition, hitted.gameObject.GetComponent<Chest>().initialPosition) > .3f)
                {
                    if (moveObj < 0)
                    {
                        moveObj = 0;
                    }
                }

                //Debug.Log(hitted.gameObject.transform.localPosition);

                openingDoor = true;

                hitted.gameObject.transform.localPosition += new Vector3(moveObj, 0, 0);

                break;

            case "Cover":
                if (Time.time - timer > holdButton)
                {
                    timer = float.PositiveInfinity;

                    hitted.gameObject.GetComponent<Cover>().PushCover();
                    /*
                    hitted.gameObject.GetComponent<Rigidbody>().useGravity = true;
                    hitted.gameObject.GetComponent<Rigidbody>().AddForce(-70, 150, -70, ForceMode.Force);
                    AudioSource audioSource = GetComponent<AudioSource>();
                    audioSource.PlayOneShot(Sons.instance.DutoTampaCai, audioSource.volume);*/
                }

                break;
        }
        //}
    }

    public void InspectItem(IInventoryItem e)
    {
        //playerCamera.transform.eulerAngles = new Vector3(0f, 0f, 0f);

        // NÃO USADO
        //Transform inspectItem = hud.transform.Find("InspectedItem");

        if (e.model.GetComponent<Rigidbody>() != null)
        {
            e.model.GetComponent<Rigidbody>().isKinematic = true;
        }

        GameObject item = (GameObject)Instantiate(e.model, playerCamera.transform.position + (playerCamera.transform.forward * 1.5f), playerCamera.transform.rotation);

        //item.transform.Rotate(new Vector3(0, 0, 0), Space.Self);
        item.layer = 12;
        item.transform.localScale = new Vector3(.5f, .5f, .5f);
        inspectedItem = item;
        inspectedInterface = e;

        isInspecting = true;
    }

    public void Die()
    {
        if (!sonicMode) // DELETE THIS
        {
            // MAKE THE PLAYER IMMOBILE
            isDead = true;
            ChangeRootState(true);
            GetComponent<CapsuleCollider>().enabled = false;
            cc.enabled = false;

            // FADE
            blackScreen.gameObject.SetActive(true);
            blackScreen.CrossFadeAlpha(0, 0.0001f, true);
            blackScreen.CrossFadeAlpha(1, 1, true);

            AudioSource[] a = GetComponents<AudioSource>();
            a[2].PlayOneShot(Sons.instance.PaBatendojogador);
            StartCoroutine(GameOverScene());
        }
    }

    IEnumerator GameOverScene()
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("GameOver");
    }

    public void ChangeRootState(bool rooted)
    {
        isRooted = rooted;
    }

    public IEnumerator AudioSourceFade(bool volumeUp, float fadeTime, float minVolume)
    {
        float startVolume = aSource.volume;

        if (volumeUp)
        {
            while (aSource.volume < OriginalVolume)
            {
                yield return new WaitForSeconds(startVolume * Time.deltaTime / fadeTime);
                aSource.volume += startVolume * Time.deltaTime / fadeTime;

            }
        }
        else
        {
            while (aSource.volume > minVolume)
            {
                yield return new WaitForSeconds(startVolume * Time.deltaTime / fadeTime);
                aSource.volume -= startVolume * Time.deltaTime / fadeTime;

            }

            if (aSource.volume <= minVolume)
            {
                aSource.Stop();
                aSource.volume = originalVolume;
            }
        }
    }

    public void PlayTape(int i)
    {
        AudioSource[] sounds = GetComponents<AudioSource>();
        PlayerInfo playerInfo = FindObjectOfType<PlayerInfo>();
        sounds[2].Stop();
        StartCoroutine(AudioSourceFade(false, 4, 0));

        if (playerInfo.GetTapesListened() <= i)
        {
            playerInfo.SetTapesListened(i);
        }

        ClearSubtitles(0);

        switch (i)
        {
            case 1:
                sounds[2].PlayOneShot(Sons.instance.Fita1, 0.7f);
                SequenceManager.instance.Sequence4Trigger();
                DisplaySubtitles(0);
                break;
            case 2:
                sounds[2].PlayOneShot(Sons.instance.Fita2, 0.7f);
                SequenceManager.instance.Sequence10Trigger();
                DisplaySubtitles(1);
                break;
            case 3:
                sounds[2].PlayOneShot(Sons.instance.Fita3, 0.7f);
                EventTrigger.listenTape3 = true;
                DisplaySubtitles(2);
                break;
            case 4:
                sounds[2].PlayOneShot(Sons.instance.Fita4, 0.7f);
                EventTrigger.listenTape4 = true;
                EventManager.instance.mannequinCorridor.SetActive(true);
                SequenceManager.instance.Sequence32Trigger();
                DisplaySubtitles(3);
                break;
            case 5:
                sounds[2].PlayOneShot(Sons.instance.Fita5, 0.7f);
                SequenceManager.instance.Sequence33Trigger();
                DisplaySubtitles(4);
                break;
        }
    }

    public void ClearSubtitles(float delay) // MAKE ALL SUBTITLES INVISIBLE
    {
        for (int i = 0; i < subtitle.Length; i++)
        {
            subtitle[i].subtitlePanel.gameObject.SetActive(true);
            subtitle[i].subtitlePanel.CrossFadeAlpha(0, delay, true);
            subtitle[i].subtitleText.CrossFadeAlpha(0, delay, true);
        }

    }

    public void DisplaySubtitles(int subtitleIndex) // MAKE A SUBTITLE VISIBLE
    {
        subtitle[subtitleIndex].subtitlePanel.CrossFadeAlpha(1, 1.5f, false);
        subtitle[subtitleIndex].subtitleText.CrossFadeAlpha(1, 1.5f, false);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Cache")
        {
            if (door != null && door.GetComponent<WardrobeDoor>() != null)
            {
                if (!door.GetComponent<WardrobeDoor>().CheckOpen())
                {
                    DetectionValue = 0;

                    // EVENT ------------

                    cacheTimer -= 1 * Time.deltaTime;
                    cacheTimer = Mathf.Clamp(cacheTimer, 0, 3);

                    if (cacheTimer <= 0)
                    {
                        cacheTimer = 3;
                        SequenceManager.instance.Sequence8Trigger();
                    }

                    // EVENT ------------
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Cache")
        {
            cacheTimer = 3;
        }
    }

    private void UpdateCursor()
    {
        RaycastHit hit;

        if (!isInspecting && !openingDoor)
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance))
            {
                GameObject hitted = hit.collider.gameObject;

                // IF THE CURSOR HITS A DOOR
                if (hitted.gameObject.GetComponent<Door2>() != null)
                {
                    // IF THE DOOR ISN'T A GLASS DOOR
                    if (hitted.gameObject.GetComponent<Door2>().type != Types.Glass)
                    {
                        // IF THE DOOR IS LOCKED
                        if (hitted.gameObject.GetComponent<Door2>().isLocked)
                        {
                            // IF THE PLAYER HAS A KEY
                            if (keys.Count != 0)
                            {
                                for (int i = 0; i < keys.Count; i++)
                                {
                                    // IF THE KEY IS RIGHT
                                    if (keys[i] == hitted.gameObject.GetComponent<Door2>().id)
                                    {
                                        DisplayCursor(cursor[2]);
                                    }
                                    // IF THE KEY ISN'T
                                    else
                                    {
                                        DisplayCursor(cursor[1]);
                                    }
                                }
                            }
                            // IF THE PLAYER DOESN'T HAVE A KEY
                            else
                            {
                                DisplayCursor(cursor[1]);
                            }
                        }
                        // IF THE DOOR ISN'T LOCKED
                        else
                        {
                            DisplayCursor(cursor[3]);
                        }
                    }
                    // IF THE DOOR IS A GLASS DOOR
                    else
                    {
                        DisplayCursor(cursor[3]);
                    }
                }
                // IF THE CURSOR HITS AN INTERACTABLE OBJECT
                else if (hitted.gameObject.GetComponent<PickupItem>() != null || hitted.gameObject.GetComponent<ElevatorPanel>() != null || hitted.gameObject.GetComponent<Generator>() != null || hitted.gameObject.GetComponent<Cover>() != null || hitted.gameObject.GetComponent<Chest>() != null || hitted.gameObject.GetComponent<WardrobeDoor>() != null || hitted.gameObject.CompareTag("Gravador") || hitted.gameObject.CompareTag("TV"))
                {
                    DisplayCursor(cursor[3]);
                }
                // IF THE CURSOR HITS A FLAMMABLE OBJECT
                else if (hitted.gameObject.GetComponent<FlammableObjects>() != null)
                {
                    DisplayCursor(cursor[3]);
                    print("MANDA O MARCUS FAZER ICONE DE INTERAÇÃO PRO JORNAL" + Time.time);
                }
                // IF THE CURSOR HITS NOTHING INTERACTABLE
                else
                {
                    DisplayCursor(cursor[0]);
                }
            }
            else
            {
                DisplayCursor(cursor[0]);
            }
        }
        else if (isInspecting)
        {
            DisplayCursor(null);
        }
        else if (openingDoor)
        {
            DisplayCursor(cursor[4]);
        }
    }

    private void DisplayCursor(GameObject selectedCursor)
    {
        if (selectedCursor != null)
        {
            for (int i = 0; i < cursor.Length; i++)
            {
                if (cursor[i] == selectedCursor)
                {
                    cursor[i].SetActive(true);
                }
                else
                {
                    cursor[i].SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < cursor.Length; i++)
            {
                cursor[i].gameObject.SetActive(false);
            }
        }
    }
    #endregion
}
