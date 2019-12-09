using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Resources")]
    [SerializeField] private List<AudioClip> bgms;

    [Header("Holder")]
    [SerializeField] private AudioSource BGM;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        ChangeBGM();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            gameObject.AddComponent<AudioSource>();
        }
    }

    private void ChangeBGM()
    {
        if (BGM == null) return;

        AudioClip targetClip;

        if (SceneManager.GetActiveScene().name.Contains("Dungeon"))
            targetClip = bgms[1];
        else
            targetClip = bgms[0];

        if (targetClip != BGM.clip)
        {
            BGM.clip = targetClip;
            BGM.Play();
        }
    }

    public void PlaySE(AudioClip clip)
    {
        if (clip == null) return;

        AudioSource source = gameObject.AddComponent<AudioSource>();

        source.clip = clip;
        source.Play();

        Destroy(source, clip.length);
    }

    public void PlaySE_Volume(AudioClip clip, float volume)
    {
        if (clip == null) return;

        AudioSource source = gameObject.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;
        source.Play();

        Destroy(source, clip.length);
    }

    public IEnumerator PlaySE(AudioClip clip, float delay)
    {
        if (clip == null) yield break;

        yield return new WaitForSeconds(delay);

        AudioSource source = gameObject.AddComponent<AudioSource>();

        source.clip = clip;
        source.Play();

        Destroy(source, clip.length);
    }
}
