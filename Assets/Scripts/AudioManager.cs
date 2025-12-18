using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioSource Audio;
    public Dictionary<string, AudioClip> audioDic;
    public AudioClip[] audioList;
    StringBuilder sb;


    private void Awake()
    {
        Audio = GetComponent<AudioSource>();
    }
    private void Start()
    {
        GameManager.Instance.audioManager = this;
        audioDic = new Dictionary<string, AudioClip>();
        for(int i = 0; i < audioList.Length;i++)
        audioDic.Add(audioList[i].name, audioList[i]);
        Audio.PlayOneShot(audioDic["BGM"]);
    }

}
