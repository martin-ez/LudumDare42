using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallbackCollider : MonoBehaviour
{
    Spawner sp;

    private void Start()
    {
        sp = FindObjectOfType<Spawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (sp.playing && other.gameObject.CompareTag("Player"))
        {
            sp.GameOver();
        }
    }
}
