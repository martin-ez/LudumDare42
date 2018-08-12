using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallbackCollider : MonoBehaviour
{
    Spawner sp;
    GUIManager gui;

    private void Start()
    {
        sp = FindObjectOfType<Spawner>();
        gui = FindObjectOfType<GUIManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (sp.playing && other.gameObject.CompareTag("Player"))
        {
            sp.playing = false;
            Destroy(other.gameObject);
            gui.GameOver();
        }
    }
}
