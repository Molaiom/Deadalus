using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

enum JanitorState { Neutral, Chasing, Idle, Attacking, Cutscene };

public class JanitorController : MonoBehaviour
{
    #region Attributes    
    JanitorState janitorState;
    private NavMeshAgent agent;
    private Vector3 currentDestination;    
    private GameObject lastDoorOpened;
    private Animator anim;    
    private AudioSource aSourceGeneral;
    private AudioSource aSourceVoice;
    private AudioSource aSourceSteps;
    private float startingSpeed;
    private float attackDelay = 0.1f; 
    private bool breakCutscene = true;    

    [Header("Detection Settings")]
    private float startingDetectionRange;
    public float detectionRange;    
    public float minDistanceToTarget;
    public float minDistanceToDoor;
    public LayerMask targetsLayers;
    #endregion

    #region Main Methods
    private void Awake()
    {
        // GETTING COMPONENTS
        agent = GetComponent<NavMeshAgent>();                
        anim = GetComponent<Animator>();

        AudioSource[] aSources = GetComponents<AudioSource>();        
        aSourceGeneral = aSources[0];
        aSourceVoice = aSources[1];
        aSourceSteps = aSources[2];


        // SETTING VALUES
        startingSpeed = agent.speed;
        SequenceManager.JanitorOnDuct += DuctEvent;        
    }

    private void FixedUpdate() // MAKES THE JANITOR DO SOMETHING BASED ON ITS STATE
    {
        anim.SetFloat("Speed", agent.speed);

        switch (janitorState)
        {
            case JanitorState.Neutral:
                
                FindTarget(0);
                InteracWithDoors();

                break;

            case JanitorState.Chasing:

                FindTarget(0);
                InteracWithDoors();

                break;

            case JanitorState.Idle:

                FindTarget(0);
                InteracWithDoors();

                break;

            case JanitorState.Attacking:
                                            
                FindTarget(0);
                AttackPlayer();

                break;

            case JanitorState.Cutscene:


                if(breakCutscene)
                    FindTarget(4);

                break;

            default:
                Debug.LogError("Janitor State not set");
                Destroy(gameObject);
                break;

        }
    }
    #endregion

    #region A.I Methods
    private void MoveJanitor() // MOVES THE JANITOR
    {
        agent.SetDestination(currentDestination);
    }

    public void SetCurrentDestination(Vector3 destination) // SETS THE CURRENT DESTINATION VARIABLE
    {
        if (destination != currentDestination)
        {
            currentDestination = destination;
            MoveJanitor();
        }
    }

    public void FindTarget(float minValueToDetect) // SEEKS FOR A NEW TARGET EACH TIME IT'S CALLED
    {

        Vector3 offsetPosition = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        GameObject selectedTarget = null;


        // DETECT AND STORES ALL AVAILABLE OBJECTS WITHIN RANGE
        Collider[] hitColliders = Physics.OverlapCapsule(transform.position, offsetPosition, detectionRange, targetsLayers, QueryTriggerInteraction.Collide);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            // FINDS OUT IF THE TARGET HAS THE DETECTABLE SCRIPT
            if (hitColliders[i].gameObject.GetComponent<DetectableByJanitor>() != null)
            {
                GameObject o = hitColliders[i].gameObject;
                DetectableByJanitor obj = o.GetComponent<DetectableByJanitor>();

                // SELECTING THE BEST TARGET OBJECT
                if (i == 0)
                {
                    selectedTarget = obj.gameObject;
                }

                else if (selectedTarget != null && obj.DetectionValue > selectedTarget.GetComponent<DetectableByJanitor>().DetectionValue)
                {
                    selectedTarget = obj.gameObject;
                }

                // IF THE TARGET IS THE PLAYER
                else if (selectedTarget != null && obj.DetectionValue == selectedTarget.GetComponent<DetectableByJanitor>().DetectionValue
                    && obj.tag == "Player")
                {
                    selectedTarget = obj.gameObject;

                }

                else if (selectedTarget != null && obj.DetectionValue == selectedTarget.GetComponent<DetectableByJanitor>().DetectionValue
                    && obj.tag != "Player")
                {
                    float newDistance = 0;
                    float oldDistance = 0;

                    newDistance = Vector3.Distance(transform.position, obj.transform.position);
                    oldDistance = Vector3.Distance(transform.position, selectedTarget.transform.position);

                    if (newDistance < oldDistance)
                    {
                        selectedTarget = obj.gameObject;
                    }
                }

            }
        }

        // IF THE TARGET IS VALID, UPDATE IT
        if (selectedTarget != null)
        {
            if (selectedTarget.GetComponent<DetectableByJanitor>().DetectionValue >= minValueToDetect)
            {                
                SetCurrentDestination(selectedTarget.transform.position);

                // IF THE TARGET IS THE PLAYER
                if (selectedTarget.tag == "Player")
                {
                    float f = Vector3.Distance(transform.position, selectedTarget.transform.position);

                    // IF THE PLAYER IS CLOSE THEN ATTACK IT
                    if (f <= minDistanceToTarget)
                    {
                        SetJanitorState("Attacking");
                        transform.LookAt(selectedTarget.transform);
                    }
                    else
                    {
                        SetJanitorState("Chasing");
                    }
                }
                else // IF THE TARGET ISN'T THE PLAYER
                {
                    float j = Vector3.Distance(transform.position, selectedTarget.transform.position);

                    // IF THE TARGET IF CLOSE THEN STOP
                    if (j <= minDistanceToTarget)
                    {
                        SetJanitorState("Idle");
                    }
                    else
                    {
                        SetJanitorState("Neutral");
                    }
                }
            }

            // IF THE TARGET HAS A DETECTION VALUE < THAN REQUESTED
            else
            {
                SetCurrentDestination(transform.position);
                if (janitorState == JanitorState.Idle)
                    SetJanitorState("Idle");
            }

        }

        // IF THE TARGET ISN'T VALID
        else
        {
            SetCurrentDestination(transform.position);

            if (janitorState == JanitorState.Idle)
                SetJanitorState("Idle");

        }
    }

    public void SetJanitorState(string state) // SETS THE JANITOR ENUM STATE
    {
        switch (state)
        {
            // NEUTRAL STATE ----- ----- ----- ----- -----
            case "Neutral":
                janitorState = JanitorState.Neutral;

                aSourceVoice.clip = Sons.instance.ZeladorGrunhidosNormal;
                if (!aSourceVoice.isPlaying)
                    aSourceVoice.Play();

                aSourceSteps.clip = Sons.instance.ZeladorPisoconcretoAndando;
                if (!aSourceSteps.isPlaying)
                    aSourceSteps.Play();

                aSourceGeneral.clip = Sons.instance.PaArrastandoandando;
                if (!aSourceGeneral.isPlaying)
                    aSourceGeneral.Play();

                agent.speed = startingSpeed;
                anim.SetInteger("State", 1);                
                break;

            // CHASING STATE ----- ----- ----- ----- -----
            case "Chasing":
                if (janitorState == JanitorState.Chasing)
                    anim.SetBool("Levantando", false);
                else
                    anim.SetBool("Levantando", true);

                janitorState = JanitorState.Chasing;

                aSourceVoice.clip = Sons.instance.ZeladorGrunhidosCorrendo;
                if (!aSourceVoice.isPlaying)
                    aSourceVoice.Play();

                aSourceSteps.clip = Sons.instance.ZeladorPisoconcretoCorrendo;
                if (!aSourceSteps.isPlaying)
                    aSourceSteps.Play();

                aSourceGeneral.Stop();

                agent.speed = startingSpeed + (startingSpeed * 0.9f);
                anim.SetInteger("State", 2);  

                break;

            // IDLE STATE ----- ----- ----- ----- -----
            case "Idle":
                janitorState = JanitorState.Idle;

                aSourceVoice.clip = Sons.instance.ZeladorGrunhidosNormal;
                if (!aSourceVoice.isPlaying)
                    aSourceVoice.Play();

                aSourceSteps.Stop();

                aSourceGeneral.Stop();

                agent.speed = 0;
                anim.SetInteger("State", 0);                
                break;

            // ATTACKING STATE ----- ----- ----- ----- -----
            case "Attacking":
                janitorState = JanitorState.Attacking;

                aSourceVoice.clip = null;                        

                aSourceSteps.Stop();

                aSourceGeneral.clip = null;

                agent.speed = startingSpeed - (startingSpeed * 0.7f);                
                anim.SetBool("Attacking", true);                
                break;

            // CUTSCENE STATE ----- ----- ----- ----- -----
            case "Cutscene":
                janitorState = JanitorState.Cutscene;

                aSourceVoice.Stop();
                agent.speed = startingSpeed;

                aSourceSteps.Stop();

                aSourceGeneral.clip = Sons.instance.PaVarrendo;
                if (!aSourceGeneral.isPlaying)
                    aSourceGeneral.Play();

                anim.SetInteger("State", 4);
                break;

            // INVALID STATE ----- ----- ----- ----- -----
            default:
                Debug.LogWarning("Invalid State. Janitor is set to 'Idle'");
                janitorState = JanitorState.Idle;

                aSourceVoice.clip = Sons.instance.ZeladorGrunhidosNormal;
                if (!aSourceVoice.isPlaying)
                    aSourceVoice.Play();

                aSourceSteps.Stop();

                aSourceGeneral.Stop();

                agent.speed = 0;
                anim.SetInteger("State", 0);                
                break;
        }
    }

    public void InteracWithDoors() // OPENS DOORS
    {
        // DETECTS ALL DOORS AROUND
        Vector3 offsetPosition = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        Collider[] hitColliders = Physics.OverlapCapsule(transform.position, offsetPosition, minDistanceToDoor, targetsLayers, QueryTriggerInteraction.Collide);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if(hitColliders[i].gameObject.GetComponent<Door2>() != null)
            {
                if(!hitColliders[i].gameObject.GetComponent<Door2>().DoorIsOpen() && !hitColliders[i].gameObject.GetComponent<Door2>().isLocked)
                {
                    hitColliders[i].gameObject.GetComponent<Door2>().OpenDoor();
                    anim.SetBool("Door", true);
                    lastDoorOpened = hitColliders[i].gameObject;
                }
            }
        }
        
        if (lastDoorOpened != null)
        {
            float distanceToLastDoor = Vector3.Distance(transform.position, lastDoorOpened.transform.position);

            if (distanceToLastDoor > 1.8)
            {
                if (lastDoorOpened.GetComponent<Door2>().DoorIsOpen() && !lastDoorOpened.GetComponent<Door2>().isLocked)
                {
                    lastDoorOpened.GetComponent<Door2>().CloseDoor();
                    anim.SetBool("Door", false);
                    lastDoorOpened = null;
                }
            }
        }

    }

    public void SetBreakCutscene(bool state)
    {
        breakCutscene = state;
    }

    public void AttackPlayer() // KILLS THE PLAYER IF IN RANGE
    {
        RaycastHit hit;
        float offsetY = 0.4f;
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);

        attackDelay -= 1 * Time.deltaTime;
        attackDelay = Mathf.Clamp(attackDelay, 0, Mathf.Infinity);        

        if(attackDelay <= 0)
        {
            attackDelay = 0.3f;
            anim.SetBool("Attacking", false);
            if(!aSourceVoice.isPlaying)
                aSourceVoice.PlayOneShot(Sons.instance.ZeladorGrunhidosGolpedepá);

            if (Physics.Raycast(origin, transform.forward, out hit, 3, targetsLayers))
            {                
                if(hit.transform.gameObject.GetComponent<PlayerController>() != null)
                {
                    PlayerController player = hit.transform.gameObject.GetComponent<PlayerController>();

                    if (!player.isDead)
                    {                     
                        player.Die();                        
                    }
                }
         
            }   
        }                       
    }

    private void OnDrawGizmosSelected() // DISPLAYS HOW FAR HE CAN DETECT A TARGET
    {
        // DETECTION RANGE
        Gizmos.color = new Color(1, 0.5f, 0);
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // ATTACK RANGE
        Gizmos.color = Color.red;
        float offsetY = 0.4f;
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);

        Ray ray = new Ray(origin, transform.forward * 3);
        Gizmos.DrawRay(ray);
    }    
    #endregion

    #region Cutscene Methods
    public IEnumerator CorridorEvent() // TRIGGERS WHEN THE PLAYER LEAVES THE SPAWN ROOM AND SPAWNS THE JANITOR ON THE CORRIDOR 
    {
        SequenceManager.JanitorCorridorEvent -= CorridorEvent;        
        aSourceGeneral.Stop();
        aSourceVoice.Stop();
        aSourceSteps.Stop();

        // JANITOR TURNS
        anim.SetBool("Turning", true);

        AudioSource aSourcePlayer = FindObjectOfType<PlayerController>().GetComponent<AudioSource>();
        yield return new WaitForSeconds(1);

        // LIGHTS OFF

        LightManager.instance.SetLightState(0, false);
        LightManager.instance.SetSunState(false);
        LightManager.instance.SetFlashlightState(false);
        aSourcePlayer.PlayOneShot(Sons.instance.LampadaApagar, 0.7f);
        yield return new WaitForSeconds(2.5f);

        // LIGHTS ON
                
        LightManager.instance.SetLightState(1, true);
        LightManager.instance.SetSunState(true);
        LightManager.instance.SetFlashlightState(true);                

        aSourcePlayer.clip = Sons.instance.MusicaPreenchimento2;
        aSourcePlayer.GetComponent<AudioSource>().PlayDelayed(0.15f);
        aSourcePlayer.PlayOneShot(Sons.instance.LampadaAcender, 0.7f);

        // INSTANTIATE STUFF HERE
        Destroy(gameObject);
    }

    public IEnumerator DuctEvent()
    {
        SequenceManager.JanitorOnDuct -= DuctEvent;
        aSourceGeneral.Stop();
        aSourceVoice.Stop();
        aSourceSteps.Stop();

        // DUCT DROPS
        SequenceManager.instance.firstRoomDuct.GetComponent<Cover>().PushCover();                

        // PLAYER GETS FROZEN
        DoorManager.instance.OpenCloseDoor(1, false);
        DoorManager.instance.LockUnlockDoor(1, false);
        FindObjectOfType<PlayerController>().ChangeRootState(true);   
        yield return new WaitForSeconds(1.55f);

        // DOOR OPENS
        DoorManager.instance.OpenCloseDoor(2, true);
        DoorManager.instance.LockUnlockDoor(2, false);
        yield return new WaitForSeconds(0.35f);

        // JANITOR GOES TO THE DUCT        
        SetCurrentDestination(SequenceManager.instance.ductSceneFirstDestination.position);
        FindObjectOfType<PlayerController>().gameObject.GetComponent<AudioSource>().clip = Sons.instance.MusicaTensão1;
        FindObjectOfType<PlayerController>().gameObject.GetComponent<AudioSource>().Play();        
        anim.SetInteger("State", 1);   
        yield return new WaitForSeconds(2.2f);

            // AUDIOS
            aSourceVoice.clip = Sons.instance.ZeladorGrunhidosNormal;
            aSourceVoice.Play();

            aSourceSteps.clip = Sons.instance.ZeladorPisoconcretoAndando;
            aSourceSteps.Play();

            aSourceGeneral.clip = Sons.instance.PaArrastandoandando;
            aSourceGeneral.Play();

        DoorManager.instance.OpenCloseDoor(2, false);
        yield return new WaitForSeconds(5f);

        // JANITOR INSPECTS THE DUCT   
        anim.SetBool("Inspecionando", true);

            // AUDIOS
            aSourceGeneral.Pause();
            aSourceSteps.Pause();

        yield return new WaitForSeconds(5f);

        // JANITOR GOES AWAY
        SetCurrentDestination(SequenceManager.instance.ductSceneSecondDestination.position);
        anim.SetBool("Inspecionando", false);

            // AUDIOS
            aSourceGeneral.UnPause();
            aSourceSteps.UnPause();

        // DOOR OPENS AND CLOSES
        yield return new WaitForSeconds(0.6f);
        DoorManager.instance.OpenCloseDoor(3, true);
        DoorManager.instance.LockUnlockDoor(3, false);

        yield return new WaitForSeconds(1.2f);
        DoorManager.instance.OpenCloseDoor(3, false);
        StartCoroutine(FindObjectOfType<PlayerController>().AudioSourceFade(false, 6, 0));
        FindObjectOfType<PlayerController>().ChangeRootState(false);

        yield return new WaitForSeconds(6);
        FindObjectOfType<PlayerController>().gameObject.GetComponent<AudioSource>().volume = FindObjectOfType<PlayerController>().OriginalVolume;
        FindObjectOfType<PlayerController>().gameObject.GetComponent<AudioSource>().Stop();
        Destroy(gameObject);

    } // TRIGGERS AFTER THE FIRST TAPE
    
    public IEnumerator DarkRoomEvent()
    {
        SequenceManager.LastSequence -= DarkRoomEvent;
        aSourceGeneral.Stop();
        aSourceVoice.Stop();
        aSourceSteps.Stop();

        FindObjectOfType<PlayerController>().gameObject.GetComponent<AudioSource>().PlayOneShot(Sons.instance.EfeitoDramatico5);
        FindObjectOfType<PlayerController>().ChangeRootState(true);
        print("Janitor Attacked");

        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Levantando", true);
        anim.SetBool("Attacking", true);
        anim.SetInteger("State", 2);        
        yield return new WaitForSeconds(0.75f);

        FindObjectOfType<PlayerController>().gameObject.GetComponent<AudioSource>().PlayOneShot(Sons.instance.PaBatendojogador, 1);
        aSourceVoice.PlayOneShot(Sons.instance.ZeladorGrunhidosGolpedepá);
        yield return new WaitForSeconds(0.5f);
        SequenceManager.instance.blackScreen.gameObject.SetActive(true);
        SequenceManager.instance.blackScreen.CrossFadeAlpha(0, 0, false);
        SequenceManager.instance.blackScreen.CrossFadeAlpha(1, 0.2f, false);

        yield return new WaitForSeconds(1.5f);
        AudioSource[] playerAudios = FindObjectOfType<PlayerController>().gameObject.GetComponents<AudioSource>();
        for (int i = 0; i < playerAudios.Length; i++)
        {
            playerAudios[i].volume = 0;
            playerAudios[i].Stop();
        }

        Application.Quit();
    } // TRIGGERS AFTER THE LAST TAPE
    #endregion
}
