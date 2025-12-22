using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioSource Audio;
    public AudioSource FlamePoint;
    public AudioSource RepairPoint;
    public AudioSource DrillPoint;
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
        for(int i = 0; i < audioList.Length; i++)
        {
            audioDic.Add(audioList[i].name, audioList[i]);
        }
        
        if (GameManager.Instance.isInside)
        {
            Audio.PlayOneShot(audioDic["BGM"]);


        }
        else
        {
            Audio.PlayOneShot(audioDic["BGM"]);
            Audio.PlayOneShot(audioDic["BGM-2"]);

        }
    }

}
