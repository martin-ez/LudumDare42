using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public float MasterVolume { get; private set; }
    public float SfxVolume { get; private set; }
    public float MusicVolume { get; private set; }

    AudioSource sfx;
    AudioSource level1Source;
    AudioSource level2Source;
    AudioSource level3Source;

    public enum Sound
    {
        Hit,
        Nice,
        Great,
        Perfect
    }

    [Header("Clips")]
    public AudioClip level1;
    public AudioClip level2;
    public AudioClip level3;

    public AudioClip hit;
    public AudioClip nice;
    public AudioClip great;
    public AudioClip perfect;

    private int currentLevel;
    private bool fading = false;
    private int newLevel;
    private float timeChange;

    private static bool created = false;

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }

        GameObject sfx2DS = new GameObject("SFX_Source");
        sfx = sfx2DS.AddComponent<AudioSource>();
        sfx2DS.transform.parent = transform;
        DontDestroyOnLoad(sfx2DS.gameObject);

        GameObject source1 = new GameObject("MusicSource Level1");
        level1Source = source1.AddComponent<AudioSource>();
        level1Source.volume = 0;
        DontDestroyOnLoad(source1.gameObject);
        GameObject source2 = new GameObject("MusicSource Level2");
        level2Source = source2.AddComponent<AudioSource>();
        level2Source.volume = 0;
        DontDestroyOnLoad(source2.gameObject);
        GameObject source3 = new GameObject("MusicSource Level3");
        level3Source = source3.AddComponent<AudioSource>();
        level3Source.volume = 1;
        DontDestroyOnLoad(source3.gameObject);

        currentLevel = 2;
        PlayMusic();
    }

    void Update()
    {
        if (fading)
        {
            timeChange += Time.deltaTime;
            float i = timeChange / 4f;

            level1Source.volume = 0;
            level2Source.volume = 0;
            level3Source.volume = 0;

            if (i < 1)
            {
                switch (currentLevel)
                {
                    case 0:
                        level1Source.volume = Mathf.Lerp(1, 0, i);
                        break;
                    case 1:
                        level2Source.volume = Mathf.Lerp(1, 0, i);
                        break;
                    case 2:
                        level3Source.volume = Mathf.Lerp(1, 0, i);
                        break;
                }

                switch (newLevel)
                {
                    case 0:
                        level1Source.volume = Mathf.Lerp(0, 1, i);
                        break;
                    case 1:
                        level2Source.volume = Mathf.Lerp(0, 1, i);
                        break;
                    case 2:
                        level3Source.volume = Mathf.Lerp(0, 1, i);
                        break;
                }
            }
            else
            {
                switch (currentLevel)
                {
                    case 0:
                        level1Source.volume = 0;
                        break;
                    case 1:
                        level2Source.volume = 0;
                        break;
                    case 2:
                        level3Source.volume = 0;
                        break;
                }

                switch (newLevel)
                {
                    case 0:
                        level1Source.volume = 1;
                        break;
                    case 1:
                        level2Source.volume = 1;
                        break;
                    case 2:
                        level3Source.volume = 1;
                        break;
                }

                fading = false;
                currentLevel = newLevel;
            }
        }
    }

    public void ChangeLevel(int level)
    {
        if (level < 2)
        {
            newLevel = 0;
        }
        else if (level < 4)
        {
            newLevel = 1;
        }
        else
        {
            newLevel = 2;
        }

        if (newLevel != currentLevel)
        {
            fading = true;
            timeChange = 0f;
        }
    }

    public void PlayMusic()
    {
        level1Source.clip = level1;
        level1Source.loop = true;
        level1Source.Play();
        level1Source.time = 0;

        level2Source.clip = level2;
        level2Source.loop = true;
        level2Source.Play();
        level2Source.time = 0;

        level3Source.clip = level3;
        level3Source.loop = true;
        level3Source.Play();
        level3Source.time = 0;
    }

    public void PlaySound(Sound clipName)
    {
        AudioClip clip = null;
        switch (clipName)
        {
            case Sound.Hit:
                clip = hit;
                break;
            case Sound.Nice:
                clip = nice;
                break;
            case Sound.Great:
                clip = great;
                break;
            case Sound.Perfect:
                clip = perfect;
                break;
        }
        if (clip != null)
        {
            sfx.clip = clip;
            sfx.time = 0f;
            sfx.loop = false;
            sfx.Play();
        }
    }
}
