using UnityEngine;

public class LightManager : MonoBehaviour
{
    public static LightManager instance;
    public GameObject sunSource;
    public GameObject flashLight;
    public GameObject zippo;

    private void Awake()
    {
        instance = this;
    }

    public void SetLightState(int id, bool state)
    {
        LightController[] lights = FindObjectsOfType<LightController>();
        for (int i = 0; i < lights.Length; i++)
        {
            GameObject o = lights[i].gameObject;
            LightController lightObj = o.GetComponent<LightController>();

            if(id <= 0)
            {
                lightObj.SetLightState(state);
            }
            else if(lightObj.id == id)
            {
                lightObj.SetLightState(state);
            }
            
        }
    }

    public void SetSunState(bool state)
    {
        sunSource.SetActive(state);
    }

    public void SetFlashlightState(bool state)
    {
        flashLight.SetActive(state);
    }

    public void SetZippoState(bool state)
    {
        zippo.SetActive(state);
    }
}
