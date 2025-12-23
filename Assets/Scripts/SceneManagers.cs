using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    public Animator anim;
    AudioManager audioManager;
    string sceneName;
    private void Start()
    {
        audioManager = GameManager.Instance.audioManager;


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
                GetComponent<Collider>().enabled = false;
                sceneName = "Outside";
                ChangeScene();
                GameManager.Instance.isInside = false;
                
            }
            else
            {
                GetComponent<Collider>().enabled = false;
                sceneName = "Inside";
                ChangeScene();
                GameManager.Instance.isInside = true;
            }
        }
    }

    public void ChangeScene()
    {
        StartCoroutine(OpenDoor());
    }

    IEnumerator OpenDoor()
    {
        GameManager.Instance.audioManager.Audio.PlayOneShot(GameManager.Instance.audioManager.audioDic["Door"]);
        anim.Play("New Animation");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(sceneName);
       
    }
}
