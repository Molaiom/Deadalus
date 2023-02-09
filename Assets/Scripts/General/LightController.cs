using System.Collections;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public int id = 0;
    public bool flashingLight;
    private bool isFlashing = false;
    private Light lightReference;
    private bool isLitten;
    private AudioSource aSource;

    private Vector3 originalPosition;


    private void Start()
    {
        if (GetComponentInChildren<Light>() != null)
        {
            lightReference = GetComponentInChildren<Light>();
            isLitten = lightReference.isActiveAndEnabled;
            aSource = GetComponent<AudioSource>();
        }
        if (id == 1 && gameObject.GetComponent<Rigidbody>())
        {
            originalPosition = transform.position;
            EventManager.LampFall += FallLamp;
        }
    }

    public void SetLightState(bool state)
    {        
        if (lightReference != null)
        {
            lightReference.enabled = state;
            isLitten = state;
        }
    }

    #region Light Flashing
    private void LateUpdate()
    {
        if(!isFlashing)
        StartCoroutine(FlashingLight());
    }

    private IEnumerator FlashingLight()
    {
        if (flashingLight && isLitten)
        {
            isFlashing = true;
            yield return new WaitForSeconds(Random.Range(5, 8));

            float f = Random.Range(0, 2);

            if (isLitten && f >= 0.3f)
            {
                lightReference.enabled = false;
                aSource.PlayOneShot(Sons.instance.LampadaFalhando);
                yield return new WaitForSeconds(0.05f);

                if (isLitten)
                {
                    lightReference.enabled = true;
                    yield return new WaitForSeconds(0.2f);

                    if (isLitten)
                    {
                        lightReference.enabled = false;
                        yield return new WaitForSeconds(0.05f);

                        if (isLitten)
                        {
                            lightReference.enabled = true;
                            yield return new WaitForSeconds(0.1f);

                            if (isLitten)
                            {
                                lightReference.enabled = false;
                                yield return new WaitForSeconds(0.05f);

                                if (isLitten)
                                {
                                    lightReference.enabled = true;
                                    yield return new WaitForSeconds(0.95f);
                                }
                            }
                        }
                    }
                }
            }

            else if (isLitten && f < 0.3f)
            {
                lightReference.enabled = false;
                aSource.PlayOneShot(Sons.instance.LampadaFalhandorapido);
                yield return new WaitForSeconds(0.05f);

                if (isLitten)
                {
                    lightReference.enabled = true;
                    yield return new WaitForSeconds(0.1f);
                }
            }
            

            isFlashing = false;
        }

    }
    #endregion

    #region FallLamp

    IEnumerator FallLamp()
    {
        EventManager.LampFall -= FallLamp;

        gameObject.GetComponent<Rigidbody>().useGravity = true;

        yield return new WaitForSeconds(.6f);

        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(Sons.instance.LampadaCaindo, audioSource.volume);

        yield return new WaitForSeconds(20);

        gameObject.GetComponent<Rigidbody>().useGravity = false;
        transform.position = originalPosition;
    }

    #endregion

}
