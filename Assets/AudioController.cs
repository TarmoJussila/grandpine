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
}

[System.Serializable]
public class Sound
{
    public SoundType SoundType;
    public AudioClip AudioClip;
}

public class AudioController : Singleton<AudioController>
{
    [Header("References")]
    [SerializeField] private AudioSource ambientAudioSource;
    [SerializeField] private AudioSource soundAudioSource;

    [Header("Settings")]
    [SerializeField] private List<Sound> sounds = new List<Sound>();

    private Dictionary<SoundType, AudioClip> soundsDictionary = new Dictionary<SoundType, AudioClip>();

    private void Start()
    {
        soundsDictionary = sounds.ToDictionary(x => x.SoundType, x => x.AudioClip);
    }

    public void PlaySound(SoundType soundType)
    {
        soundAudioSource.PlayOneShot(soundsDictionary[soundType]);
    }
}
