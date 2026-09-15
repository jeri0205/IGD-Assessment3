using UnityEngine;

public class BGM_Manager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip introBGM;
    public AudioClip loopBGM;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource.clip = introBGM;
        audioSource.loop = false;


        audioSource.Play();

        Invoke("PlayLoopBGM", 3f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayLoopBGM()
    {
        audioSource.clip = loopBGM;
        audioSource.loop = true;
        audioSource.Play();
    }
}
