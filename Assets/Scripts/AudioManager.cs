using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioSource Audio;
    public AudioSource AudioBGM;
    public AudioSource FlamePoint;
    public AudioSource RepairPoint;
    public AudioSource DrillPoint;
    public Dictionary<string, AudioClip> audioDic;
    public AudioClip[] audioList;


    private void Awake()
    {
        
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
            AudioBGM.PlayOneShot(audioDic["BGM"]);


        }
        else
        {
            AudioBGM.PlayOneShot(audioDic["BGM"]);
            AudioBGM.PlayOneShot(audioDic["BGM-2"]);

        }
    }

}
