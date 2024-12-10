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
    LevelUpSound
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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySfx(SfxType sound, bool loop = false, float volumeScale = 1f)
    {
        if (Instance == null) return;
        AudioClip clip = Instance.sfxList[(int)sound];
        float finalVolume = Instance.sfxVolume * volumeScale;

        if (loop)
        {
            Instance.sfxAudioSource.clip = clip;
            Instance.sfxAudioSource.volume = finalVolume;
            Instance.sfxAudioSource.loop = true;
            Instance.sfxAudioSource.Play();
        }
        else
        {
            Instance.sfxAudioSource.loop = false; // Ensure no loop
            Instance.sfxAudioSource.PlayOneShot(clip, finalVolume);
        }
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