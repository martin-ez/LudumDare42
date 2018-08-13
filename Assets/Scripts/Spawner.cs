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
    public GameObject audioManagerPrefab;
    public bool playing = false;
    public bool inPlay;
    public float cooldown = 0.5f;

    public Text textHeight;
    public Transform height;

    int currentGen = 0;
    int score;
    float maxHeight;
    float nextTime;
    public bool rare;
    Console current;
    Transform bonus;
    GUIManager gui;
    AudioManager audioManager;
    Camera cam;

    int[] gameStats;

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
        gui = FindObjectOfType<GUIManager>();
        bonus = transform.Find("Bonus");
        audioManager = FindObjectOfType<AudioManager>();

        if (audioManager == null)
        {
            GameObject am = Instantiate(audioManagerPrefab);
            audioManager = am.GetComponent<AudioManager>();
        }

        StartGame();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            gui.Exit();
        }

        if (playing && !inPlay && Time.time > nextTime)
        {
            GameObject[] consoles = generations[currentGen].consoles;
            GameObject toSpawn = consoles[Random.Range(0, consoles.Length)];
            float chance = Random.value;
            if (chance < 0.02)
            {
                toSpawn = rareConsole;
                rare = true;
            }
            current = Instantiate(toSpawn, Vector3.up * 100, Quaternion.identity).GetComponent<Console>();
            current.SetSpawner(this, currentGen);
            inPlay = true;
        }
    }

    public void Next()
    {
        int scoreAdd = 0;
        // Check Tower height
        if (playing && current != null)
        {
            scoreAdd += 10;
            gameStats[1] += 1;
            if (rare)
            {
                scoreAdd += 90;
                gameStats[4] += 1;
                rare = false;
            }
            if (current.transform.position.y + (current.scale.y / 2f) >= bonus.position.y)
            {
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
        if (score >= (levelUpPoints * currentGen) + levelUpPoints && currentGen < 6)
        {
            currentGen++;
            gui.UpdateGeneration(currentGen);
            audioManager.ChangeLevel(currentGen);
            StartCoroutine(NextCameraColorAnimation());
        }

        gui.UpdateScore(score, scoreAdd);
        inPlay = false;
        nextTime = Time.time + cooldown;
    }

    public void StartGame()
    {
        playing = false;
        maxHeight = 0f;
        textHeight.text = Mathf.FloorToInt(maxHeight * 10) + "cm";
        score = 0;
        currentGen = 0;
        rare = false;

        gameStats = new int[5];

        StartCoroutine(StartGameAnimation());
    }

    public void GameOver()
    {
        audioManager.ChangeLevel(5);
        StartCoroutine(CameraZoomOutAnimation());
        playing = false;

        gameStats[0] = score;
        gameStats[2] = currentGen + 3;
        gameStats[3] = Mathf.FloorToInt(maxHeight * 10);

        gui.GameOver(gameStats);
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
            i = time / 1f;
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
            i = time / 4f;
            cam.backgroundColor = Color.Lerp(startColor, endColor, i);
            yield return null;
        }

        cam.backgroundColor = endColor;
    }

    IEnumerator CameraZoomOutAnimation()
    {
        float time = 0f;
        float i = 0f;

        while (i < 1f)
        {
            time += Time.deltaTime;
            i = time / 15f;
            cam.orthographicSize = Mathf.Lerp(5, 20, i);
            yield return null;
        }

        cam.orthographicSize = 20;
    }

    IEnumerator StartGameAnimation()
    {
        gui.UpdateGeneration(currentGen);
        gui.ShowInstructions();
        audioManager.ChangeLevel(currentGen);

        float time = 0f;
        float i = 0f;

        while (i < 1f)
        {
            time += Time.deltaTime;
            i = time / 4f;
            cam.orthographicSize = Mathf.Lerp(10, 5, i);
            yield return null;
        }

        cam.orthographicSize = 5;
        playing = true;
        Next();
    }
}
