using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{

    private void Start()
    {
        AudioManager audioManager = GameManager.Instance.audioManager;


    /*    if (GameManager.Instance.isInside)
        {
            audioManager.Audio.PlayOneShot(audioManager.audioDic["BGM"]);


        }
        else
        {
            audioManager.Audio.PlayOneShot(audioManager.audioDic["BGM"]);
            audioManager.Audio.PlayOneShot(audioManager.audioDic["BGM-2"]);

        }*/
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (GameManager.Instance.isInside)
            {
                ChangeScene("Outside");
                GameManager.Instance.isInside = false;

            }
            else
            { 
                ChangeScene("inside");
                GameManager.Instance.isInside = true;
            }
        }
    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
