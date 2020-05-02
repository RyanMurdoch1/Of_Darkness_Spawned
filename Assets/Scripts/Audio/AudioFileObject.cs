using UnityEngine;

[CreateAssetMenu (fileName = "Audio File", menuName = "Audio/New Audio File")]
public class AudioFileObject : ScriptableObject
{
    public string fileName;
    public AudioType audioType;
    public AudioClip clip;
    [Range(0, 1)]
    public float volume;
    [Range(0, 2)]
    public float pitch;
}

public enum AudioType
{
    SFX,
    Music
}
