using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Handles all audio in the game, including music and sound effects.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    public AudioClip jumpSfx;
    public AudioClip slideSfx;
    public AudioClip crashSfx;
    public AudioClip coinSfx;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        ApplySavedVolumes();
    }

    private void ApplySavedVolumes()
    {
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        float sfxVol = PlayerPrefs.GetFloat("SfxVolume", 0.75f);
        SetMusicVolume(musicVol);
        SetSfxVolume(sfxVol);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(Mathf.Clamp(volume, 0.001f, 1f)) * 20f);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat("SfxVol", Mathf.Log10(Mathf.Clamp(volume, 0.001f, 1f)) * 20f);
        PlayerPrefs.SetFloat("SfxVolume", volume);
    }
}
