using UnityEngine;

[CreateAssetMenu (fileName = "Audio File", menuName = "Audio/New Audio File")]
public class AudioFileObject : ScriptableObject
{
    public string fileName;
    public AudioType audioType;
    public AudioClip clip;
    [Range(0, 1)]
    public float volume = 1;
    [Range(0, 2)]
    public float pitch = 1;
}

public enum AudioType
{
    SFX,
    Music
}
