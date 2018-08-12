using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{

    public float animationTime = 2f;

    public RectTransform gameOverPanel;
    public Text score;
    public Text scoreAdd;

    Color scoreAddColorS;
    Color scoreAddColorE;

    private void Start()
    {
        scoreAddColorS = scoreAdd.color;
        scoreAddColorE = scoreAddColorS;
        scoreAddColorE.a = 0f;
        scoreAdd.color = scoreAddColorE;
    }

    public void GameOver()
    {
        StartCoroutine(GameOverAnimation());
    }

    public void UpdateScore(int newScore, int add)
    {
        score.text = "" + newScore + " ";
        scoreAdd.text = "+" + add + "";
        StartCoroutine(ScoreUpdateAnimation());
    }

    public void Restart()
    {
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    IEnumerator GameOverAnimation()
    {
        float time = 0f;
        float i = 0f;

        while (i < 1f)
        {
            time += Time.deltaTime;
            i = time / animationTime;
            gameOverPanel.localPosition = Vector3.up * Mathf.Lerp(750, 0, i);
            yield return null;
        }

        transform.localPosition = Vector3.zero;
    }

    IEnumerator ScoreUpdateAnimation()
    {
        scoreAdd.color = scoreAddColorS;
        float time = 0f;
        float i = 0f;

        while (i < 1f)
        {
            time += Time.deltaTime;
            i = time * 2f / animationTime;
            scoreAdd.color = Color.Lerp(scoreAddColorS, scoreAddColorE, i);
            yield return null;
        }

        scoreAdd.color = scoreAddColorE;
    }
}
