using System;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets my_Instance;
    public static GameAssets GetInstance { get { return my_Instance; } }

    private void Awake()
    {
        my_Instance = this;
    }

    public Transform PipeHead;
    public Transform PipeBody;

    public Transform SpaceObj;

    public SoundClip[] soundclips;

    [Serializable]
    public class SoundClip
    {
        public SoundManager.audioClipEnum sound;
        public AudioClip audioClip;
    }
}
