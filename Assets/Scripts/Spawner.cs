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
    public int levelUpPoints = 500;
    public bool playing = true;
    public float cooldown = 0.5f;
    public float animationTime = 1f;

    public Text textHeight;
    public Transform height;

    int currentGen = 0;
    int score;
    float maxHeight;
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
        maxHeight = 0f;
        bonus = transform.Find("Bonus");
        textHeight.text = Mathf.FloorToInt(maxHeight) + "cm";
        score = 0;
        currentGen = 0;
        Time.timeScale = 1f;

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
        // Check Tower height
        if (current != null)
        {
            scoreAdd += 10;
            if (current.transform.position.y + (current.scale.y / 2f) >= bonus.position.y)
            {
                // TODO: height Bonus
                StartCoroutine(NextHeightAnimation());
            }
            if (current.transform.position.y + (current.scale.y / 2f) > maxHeight)
            {
                float old = maxHeight;
                maxHeight = current.transform.position.y + (current.scale.y / 2f);
                height.position = current.transform.position + Vector3.up * (current.scale.y / 2f);
                textHeight.text = Mathf.FloorToInt(maxHeight * 10) + "cm";
                scoreAdd += Mathf.FloorToInt(100 * (maxHeight - old));
            }
        }

        score += scoreAdd;
        if (score >= (levelUpPoints * currentGen) + levelUpPoints)
        {
            currentGen++;
            StartCoroutine(NextCameraColorAnimation());
        }

        gui.UpdateScore(score, scoreAdd);
        inPlay = false;
        nextTime = Time.time + cooldown;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playing && inPlay && other.gameObject.CompareTag("Player"))
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        StartCoroutine(CameraZoomOutAnimation());
        playing = false;
        gui.GameOver();
    }

    IEnumerator NextHeightAnimation()
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

    IEnumerator CameraZoomOutAnimation()
    {
        float time = 0f;
        float i = 0f;

        while (i < 1f)
        {
            time += Time.deltaTime;
            i = time / (animationTime * 15f);
            cam.orthographicSize = Mathf.Lerp(5, 20, i);
            yield return null;
        }

        cam.orthographicSize = 20;
        playing = true;
    }
}
