using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public RectTransform gameOverPanel;
    public Text score;
    public Text scoreAdd;
    public Text currentGen;
    public RectTransform slider;
    public Text[] statsText;

    Color scoreAddColorS;
    Color scoreAddColorE;

    int[] stats;

    private void Start()
    {
        scoreAddColorS = scoreAdd.color;
        scoreAddColorE = scoreAddColorS;
        scoreAddColorE.a = 0f;
        scoreAdd.color = scoreAddColorE;
    }

    public void GameOver(int[] gameStats)
    {
        stats = gameStats;
        StartCoroutine(GameOverAnimation());
    }

    public void UpdateScore(int newScore, int add)
    {
        score.text = "" + newScore + " ";
        scoreAdd.text = "+" + add + "";
        StartCoroutine(ScoreUpdateAnimation());
    }

    public void UpdateGeneration(int newGen)
    {
        int gen = newGen + 3;
        currentGen.text = "" + gen + " ";
    }


    public void Restart()
    {
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    IEnumerator ScoreUpdateAnimation()
    {
        scoreAdd.color = scoreAddColorS;
        float time = 0f;
        float i = 0f;

        while (i < 1f)
        {
            time += Time.deltaTime;
            i = time;
            scoreAdd.color = Color.Lerp(scoreAddColorS, scoreAddColorE, i);
            yield return null;
        }

        scoreAdd.color = scoreAddColorE;
    }

    IEnumerator GameOverAnimation()
    {
        transform.Find("Score").gameObject.SetActive(false);
        transform.Find("Gen").gameObject.SetActive(false);

        Color startColor = statsText[0].color;
        Color endColor = statsText[0].color;
        startColor.a = 0f;

        for (int j = 0; j < statsText.Length; j++)
        {
            statsText[j].color = startColor;
        }

        float time = 0f;
        float i = 0f;

        while (i < 1f)
        {
            time += Time.deltaTime;
            i = time / 1f;
            gameOverPanel.localPosition = Vector3.up * Mathf.Lerp(900, 0, i);
            yield return null;
        }

        transform.localPosition = Vector3.zero;

        Vector2 startSlider = new Vector2(10f, 2.5f);
        Vector2 endSlider = new Vector2(800f, 2.5f);

        statsText[0].text = statsText[0].text + " " + stats[0];
        time = 0f;
        i = 0;
        float t = 0;

        int current = 0;
        while (current < statsText.Length)
        {
            time += Time.deltaTime;
            i += Time.deltaTime;
            t = time / statsText.Length;

            slider.sizeDelta = Vector2.Lerp(startSlider, endSlider, t);
            statsText[current].color = Color.Lerp(startColor, endColor, i);

            if (i > 1f)
            {
                current++;
                if (current < statsText.Length)
                {
                    statsText[current].text = statsText[current].text + " " + stats[current];
                    i = 0;
                }
            }
            yield return null;
        }
    }
}
