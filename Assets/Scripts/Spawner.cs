using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject consoles;
    public int currentGen = 0;
    public bool playing = true;
    public float cooldown = 0.5f;

    bool inPlay;
    float nextTime;

    private void Start()
    {
        Next();
    }

    private void Update()
    {
        if (playing && !inPlay && Time.time > nextTime)
        {
            //GameObject toSpawn = consoles[currentGen][Random.Range(0, consoles[0].Length)];
            GameObject toSpawn = consoles;
            Console current = Instantiate(toSpawn, Vector3.up * 100, Quaternion.identity).GetComponent<Console>();
            current.transform.localScale = new Vector3(Random.Range(0.5f, 2f), Random.Range(0.5f, 1f), Random.Range(0.5f, 2f));
            current.SetSpawner(this);
            inPlay = true;
        }
    }

    public void Next()
    {
        inPlay = false;
        nextTime = Time.time + cooldown;
    }
}
