using UnityEngine;

/// <summary>
/// SoundManager全体を支えるもの、SoundManagerにはSEとBGM用にSourceをわけている
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource audioSourceSE;  
    public AudioSource audioSourceBGM;  

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); 

            AudioSource[] audioSources = GetComponents<AudioSource>();
            audioSourceSE = audioSources[1];
            audioSourceBGM = audioSources[0];
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    //BGMに関するメソッド
    public void PlayBGM(AudioClip _bgmClip)
    {
        audioSourceBGM.clip = _bgmClip;  
        audioSourceBGM.Play();         
    }
    public void StopBGM()
    {
        audioSourceBGM.Stop();         
    }

    //SEに関するメソッド
    public void PlaySE(AudioClip _seClip)
    {
         audioSourceSE.PlayOneShot(_seClip);
    }
    public void StopSE()
    {
         audioSourceSE.Stop();
    }
}
