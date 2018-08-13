using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Waterfall : MonoBehaviour
{

    public GameObject[] consoles;
    public float interval = 1f;
    float lastTime;

    void Start()
    {
        lastTime = Time.time;
    }

    void Update()
    {
        if (Time.time > lastTime + interval)
        {
            GameObject c = Instantiate(consoles[Random.Range(0, consoles.Length)]);
            Destroy(c.GetComponent<Console>());
            Destroy(c.transform.Find("GL").gameObject);
            c.GetComponent<Rigidbody>().useGravity = true;
            c.AddComponent<DieSoon>();
            c.transform.position = new Vector3(Random.Range(-4f, 4f), 25f, Random.Range(-1.5f, 1.5f));

            lastTime = Time.time;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
