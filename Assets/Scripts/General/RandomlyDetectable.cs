using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomlyDetectable : DetectableByJanitor
{
    private void Start()
    {
        InvokeRepeating("ChangeValue", 1, Random.Range(4, 7));
    }

    private void ChangeValue()
    {
        DetectionValue = Mathf.RoundToInt(Random.Range(0, 2));
    }
}
