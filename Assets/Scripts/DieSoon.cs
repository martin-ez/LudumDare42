using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieSoon : MonoBehaviour
{

    float bornTime;
    float timeToLive = 20f;


    void Start()
    {
        bornTime = Time.time;
    }

    void Update()
    {
        if (Time.time > bornTime + timeToLive)
        {
            Destroy(this.gameObject);
        }
    }
}
