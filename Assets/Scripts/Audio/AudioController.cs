using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioFileObject[] audioFiles;
    private Dictionary<string, AudioFileObject> _audioFileDictionary;

    public delegate void PlayAudioFile(string fileName);
    public static PlayAudioFile playAudioFile;

    private void OnEnable()
    {
        playAudioFile += PlayAudioFileObject;
        CreateAudioDictionary();
    }
    
    private void CreateAudioDictionary()
    {
        _audioFileDictionary = new Dictionary<string, AudioFileObject>(audioFiles.Length);
        for (var i = 0; i < audioFiles.Length; i++)
        {
            _audioFileDictionary.Add(audioFiles[i].fileName, audioFiles[i]);
        }
    }

    private void PlayAudioFileObject(string fileName)
    {
        if (!_audioFileDictionary.ContainsKey(fileName))
        {
            Debug.LogError($"Audio file with name: {fileName} does not exist");
            return;
        }
        var audioFile = _audioFileDictionary[fileName];
        audioSource.pitch = audioFile.pitch;
        audioSource.PlayOneShot(audioFile.clip, audioFile.volume);
    }

    private void OnDisable() => playAudioFile -= PlayAudioFileObject;
}

