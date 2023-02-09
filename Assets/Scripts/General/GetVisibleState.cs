using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetVisibleState : MonoBehaviour
{
    public int id = 0;
    bool tape;
    bool seen = false;

    private void Awake()
    {
        if (id == 2)
        SequenceManager.ListenToFifthTape += AfterFifthTape;
    }

    private void LateUpdate()
    {
        if (tape && GetComponent<Renderer>().isVisible)
        {
            JanitorSpawner.instance.SpawnJanitorOnDarkRoom();
            print("Janitor Spawned");
            SequenceManager.LastSequence += FindObjectOfType<JanitorController>().DarkRoomEvent;
            FindObjectOfType<PlayerController>().ChangeRootState(false);
            tape = false;
        }
    }

    public void OnBecameInvisible()
    {        
        SequenceManager.instance.Sequence26Trigger();     
    }

    public void OnBecameVisible()
    {
        if(id == 1)
        {            
            SequenceManager.instance.Sequence34Trigger();
        }        

        if(id == 4 && !seen)
        {
            FindObjectOfType<PlayerController>().gameObject.GetComponent<AudioSource>().PlayOneShot(Sons.instance.EfeitoDramatico1);
            seen = true;
        }
    }

    public IEnumerator AfterFifthTape()
    {
        if (id == 2)
        {
            SequenceManager.ListenToFifthTape -= AfterFifthTape;
            FindObjectOfType<PlayerController>().ChangeRootState(true);            

            yield return new WaitForSeconds(Sons.instance.Fita5.length);

            tape = true;
        }
    }
}
