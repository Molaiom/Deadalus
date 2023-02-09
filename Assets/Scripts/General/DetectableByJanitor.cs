using UnityEngine;

public class DetectableByJanitor : MonoBehaviour
{
    [Range(0, 10)] [SerializeField] private int detectionValue = 0;

    public int DetectionValue
    {
        get
        {
            return detectionValue;
        }

        set
        {
            value = Mathf.Clamp(value, 0, 10);
            detectionValue = value;
        }
    }
}
