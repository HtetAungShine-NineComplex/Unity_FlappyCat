using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager my_Instance;
    public static SoundManager GetInstance { get { return my_Instance; } }

    private AudioSource audioSource;
    public enum audioClipEnum
    {
        birdJump,score,dead
    }
    private void Awake()
    {
        my_Instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound(audioClipEnum audioClip)
    {
        audioSource.PlayOneShot(GetAudioClip(audioClip));
    }

    public AudioClip GetAudioClip(audioClipEnum audioClip)
    {
        foreach (GameAssets.SoundClip soundClip in GameAssets.GetInstance.soundclips)
        {
            if(soundClip.sound == audioClip) return soundClip.audioClip;
        }
        return null;
    }
}
