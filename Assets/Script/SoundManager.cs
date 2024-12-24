using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SfxType
{
    Hurt,
    SoliderUltimate,
    RamboUltimate,
    ElectroUltimate,
    ShotgunSound,
    ElectroGunSound,
    SoliderGunSound,
    LevelUpSound,
    BossAppear,
    BossDeath,
    Explode,
    ExplodeAlert,
    ButtonHover,
    ButtonConfirm
}
public enum BGMType
{
    MainMenu,
    GamePlay,
    VictoryTheme
}
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] sfxList;
    [SerializeField] private AudioClip[] bgmList;

    // Volume settings
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float backgroundVolume = 1f;

    // Separate audio sources for different sound types
    public AudioSource sfxAudioSource;
    public AudioSource backgroundAudioSource;
    private List<AudioSource> sfxAudioSources = new List<AudioSource>(); // Pool of audio sources for SFX
    private Queue<AudioSource> activeSfxQueue = new Queue<AudioSource>(); // Queue to track active SFX
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            sfxAudioSource = gameObject.AddComponent<AudioSource>();
            backgroundAudioSource = gameObject.AddComponent<AudioSource>();

            // bgm loop
            backgroundAudioSource.loop = true;
            InitializeSFXAudioSources();
            UpdateVolume();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void UpdateVolume()
    {
        sfxVolume = PlayerPrefs.GetInt("SoundVolume", 10) / 20f;
        backgroundVolume = PlayerPrefs.GetInt("MusicVolume", 10) / 20f;
        //Debug.LogWarning(sfxVolume);
        //Debug.LogWarning(backgroundVolume);
        PlayBackgroundMusic(BGMType.MainMenu);
    }
    // Initialize a pool of AudioSource components for SFX
    private void InitializeSFXAudioSources()
    {
        for (int i = 0; i < 10; i++) // You can adjust the size of the pool as needed
        {
            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            newAudioSource.playOnAwake = false;
            sfxAudioSources.Add(newAudioSource);
        }
    }

    // Get an available AudioSource from the pool
    private AudioSource GetAvailableSFXAudioSource()
    {
        if (activeSfxQueue.Count < 10) // If there are less than 5 active sounds, use an available one from the pool
        {
            foreach (var audioSource in sfxAudioSources)
            {
                if (!audioSource.isPlaying)
                {
                    return audioSource;
                }
            }
        }

        // If more than 5 sounds are playing, remove the oldest one (from the front of the queue)
        AudioSource oldestAudioSource = activeSfxQueue.Dequeue();
        oldestAudioSource.Stop(); // Stop it before reusing
        return oldestAudioSource;
    }

    public void PlaySfx(SfxType sound, bool loop = false, float duration = 0f, float volumeScale = 1f)
    {
        if (Instance == null) return;
        AudioClip clip = Instance.sfxList[(int)sound];
        float finalVolume = Instance.sfxVolume * volumeScale;
        //AudioSource audioSource = GetAvailableSFXAudioSource();

        if (loop)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = finalVolume;
            audioSource.loop = true;
            audioSource.Play();
            Destroy(audioSource, duration);
        }
        else
        {
            AudioSource audioSource = GetAvailableSFXAudioSource();
            audioSource.loop = false;
            audioSource.PlayOneShot(clip, finalVolume);
            activeSfxQueue.Enqueue(audioSource);
        }

        // Track active sound in the queue
        
    }
    public void PlayBackgroundMusic(BGMType musicType, bool loop = true)
    {
        AudioClip musicClip = bgmList[(int)musicType];

        backgroundAudioSource.Stop();
        backgroundAudioSource.clip = musicClip;
        backgroundAudioSource.loop = loop;
        backgroundAudioSource.volume = backgroundVolume;
        backgroundAudioSource.Play();
    }

    public void PauseBackgroundMusic()
    {
        backgroundAudioSource.Pause();
    }

    public void ResumeBackgroundMusic()
    {
        backgroundAudioSource.UnPause();
    }

    public void StopBackgroundMusic()
    {
        backgroundAudioSource.Stop();
    }

    // Volume controls
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

    public void SetBackgroundVolume(float volume)
    {
        backgroundVolume = Mathf.Clamp01(volume);

        // Update current background music volume if playing
        if (backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.volume = backgroundVolume;
        }
    }
}