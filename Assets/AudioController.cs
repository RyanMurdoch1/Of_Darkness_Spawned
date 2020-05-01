using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource pitchedAudioSource;
    [SerializeField] private List<AudioClip> audioClips;

    public delegate void PlayAudioClip(int index);
    public static PlayAudioClip playAudioClip;
    
    public delegate void PlayAudioClipWithPitch(int index, float pitch);
    public static PlayAudioClipWithPitch playAudioClipWithPitch;

    private void OnEnable()
    {
        playAudioClip += PlayAudio;
        playAudioClipWithPitch += PlayPitchedAudio;
    }

    private void OnDisable()
    {
        playAudioClip -= PlayAudio;
        playAudioClipWithPitch -= PlayPitchedAudio;
    }

    private void PlayAudio(int clipIndex) => audioSource.PlayOneShot(audioClips[clipIndex]);

    private void PlayPitchedAudio(int clipIndex, float pitch)
    {
        pitchedAudioSource.pitch = pitch;
        pitchedAudioSource.PlayOneShot(audioClips[clipIndex]);
    }
}

