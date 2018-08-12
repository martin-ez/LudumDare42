using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public struct ConsoleGen { public GameObject[] consoles; }

    public ConsoleGen[] generations;
    public GameObject rareConsole;
    public Color[] colorLevels;
    public int currentGen = 0;
    public bool playing = true;
    public float cooldown = 0.5f;
    public float animationTime = 1f;

    public Text textHeigth;
    public Transform heigth;

    int score;
    float maxHeigth;
    bool inPlay;
    float nextTime;
    Console current;
    Transform bonus;
    GUIManager gui;
    Camera cam;

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
        gui = FindObjectOfType<GUIManager>();
        maxHeigth = 0f;
        bonus = transform.Find("Bonus");
        textHeigth.text = Mathf.FloorToInt(maxHeigth) + "cm";
        score = 0;

        Next();
    }

    private void Update()
    {
        if (playing && !inPlay && Time.time > nextTime)
        {
            GameObject[] consoles = generations[currentGen].consoles;
            GameObject toSpawn = consoles[Random.Range(0, consoles.Length)];
            float chance = Random.value;
            if (chance < 0.02) toSpawn = rareConsole;
            current = Instantiate(toSpawn, Vector3.up * 100, Quaternion.identity).GetComponent<Console>();
            current.SetSpawner(this, currentGen);
            inPlay = true;
        }
    }

    public void Next()
    {
        int scoreAdd = 0;
        // Check Tower heigth
        if (current != null)
        {
            scoreAdd += 10;
            if (current.transform.position.y + (current.scale.y / 2f) >= bonus.position.y)
            {
                // TODO: Heigth Bonus
                StartCoroutine(NextHeigthAnimation());
            }
            if (current.transform.position.y + (current.scale.y / 2f) > maxHeigth)
            {
                float old = maxHeigth;
                maxHeigth = current.transform.position.y + (current.scale.y / 2f);
                heigth.position = current.transform.position + Vector3.up * (current.scale.y / 2f);
                textHeigth.text = Mathf.FloorToInt(maxHeigth * 10) + "cm";
                scoreAdd += Mathf.FloorToInt(10 * (maxHeigth - old));
                if (maxHeigth >= (5 * currentGen) + 5)
                {
                    // TODO: Change level
                    currentGen++;
                    scoreAdd += 100;
                    StartCoroutine(NextCameraColorAnimation());
                }
            }
        }

        score += scoreAdd;
        gui.UpdateScore(score, scoreAdd);
        inPlay = false;
        nextTime = Time.time + cooldown;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playing && inPlay && other.gameObject.CompareTag("Player"))
        {
            playing = false;
            Destroy(other.gameObject);
            // TODO: Game Over
            gui.GameOver();
        }
    }

    IEnumerator NextHeigthAnimation()
    {
        playing = false;
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + Vector3.up * 1f;
        float time = 0f;
        float i = 0f;

        while (i < 1f)
        {
            time += Time.deltaTime;
            i = time / animationTime;
            transform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }

        transform.position = endPos;
        playing = true;
    }

    IEnumerator NextCameraColorAnimation()
    {
        Color startColor = colorLevels[currentGen - 1];
        Color endColor = colorLevels[currentGen];
        float time = 0f;
        float i = 0f;

        while (i < 1f)
        {
            time += Time.deltaTime;
            i = time / (animationTime * 2f);
            cam.backgroundColor = Color.Lerp(startColor, endColor, i);
            yield return null;
        }

        cam.backgroundColor = endColor;
        playing = true;
    }
}
