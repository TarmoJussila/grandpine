using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SoundType
{
    Walk1,
    Walk2,
    AxeHit,
    TreeHit1,
    TreeHit2,
    TreeFall,
    TwigCollect,
    Sob,
    Fire,
}

[System.Serializable]
public class Sound
{
    public SoundType SoundType;
    public AudioClip AudioClip;
    [Range(0f, 1f)]
    public float Volume = 1f;
    public bool IsRandomPitch;
}

public class AudioController : Singleton<AudioController>
{
    [Header("References")]
    [SerializeField] private AudioSource ambientAudioSource;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource soundAudioSource;

    [Header("Settings")]
    [SerializeField] private List<Sound> sounds = new List<Sound>();
    [SerializeField] private float minPitch = 0.8f;
    [SerializeField] private float maxPitch = 1.2f;
    [SerializeField] private float ambientTargetVolume = 0.5f;
    [SerializeField] private float musicTargetVolume = 1f;
    [SerializeField] private float musicRaiseVolume = 0.1f;

    private Dictionary<SoundType, Sound> soundsDictionary = new Dictionary<SoundType, Sound>();

    private void Start()
    {
        soundsDictionary = sounds.ToDictionary(x => x.SoundType, x => x);

        ambientAudioSource.volume = 0f;
        musicAudioSource.volume = 0f;

        StartCoroutine(RaiseAmbientVolume());
    }

    private IEnumerator RaiseAmbientVolume()
    {
        while (ambientAudioSource.volume < ambientTargetVolume)
        {
            ambientAudioSource.volume = Mathf.Min(ambientAudioSource.volume + Time.deltaTime * 0.1f, ambientTargetVolume);

            yield return null;
        }
    }

    public void MinimizeAmbientVolume()
    {
        StartCoroutine(LowerAmbientVolume());
    }

    private IEnumerator LowerAmbientVolume()
    {
        while (ambientAudioSource.volume > 0f)
        {
            ambientAudioSource.volume = Mathf.Max(ambientAudioSource.volume - Time.deltaTime * 0.1f, 0f);

            yield return null;
        }
    }

    public void IncreaseMusicVolume()
    {
        if (!musicAudioSource.isPlaying)
        {
            musicAudioSource.Play();
        }

        musicAudioSource.volume = Mathf.Min(musicAudioSource.volume + musicRaiseVolume, musicTargetVolume);
    }

    public void MaximizeMusicVolume()
    {
        StartCoroutine(RaiseMusicVolume());
    }

    private IEnumerator RaiseMusicVolume()
    {
        while (musicAudioSource.volume < musicTargetVolume)
        {
            musicAudioSource.volume = Mathf.Min(musicAudioSource.volume + Time.deltaTime * 0.1f, musicTargetVolume);

            yield return null;
        }
    }

    public void PlaySound(SoundType soundType)
    {
        soundAudioSource.PlayOneShot(soundsDictionary[soundType].AudioClip, soundsDictionary[soundType].Volume);

        if (soundsDictionary[soundType].IsRandomPitch)
        {
            soundAudioSource.pitch = Random.Range(minPitch, maxPitch);
        }
        else
        {
            soundAudioSource.pitch = 1f;
        }
    }
}