using UnityEngine;

/// <summary>
/// SoundManager全体を支えるもの、SoundManagerにはSEとBGM用にSourceをわけている
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource audioSourceSE;  
    public AudioSource audioSourceBGM; 

    //何か別の音量で流す予備のSE
    public AudioSource audioSourceSE2;  

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); 

            AudioSource[] audioSources = GetComponents<AudioSource>();
            audioSourceSE = audioSources[0];
            audioSourceBGM = audioSources[1];
            audioSourceSE2 = audioSources[2];
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// BGMに関するメソッド
    /// </summary>
    /// <param name="_bgmClip">BGMの名前(他スクリプトから渡す)</param>
    public void PlayBGM(AudioClip _bgmClip)
    {
        audioSourceBGM.clip = _bgmClip;  
        audioSourceBGM.Play();         
    }
    public void StopBGM()
    {
        audioSourceBGM.Stop();         
    }

    /// <summary>
    ///  SEに関するメソッド
    /// </summary>
    /// <param name="_seClip">SEの名前(他スクリプトから渡す)</param>
    public void PlaySE(AudioClip _seClip)
    {
         audioSourceSE.PlayOneShot(_seClip);
    }
    public void StopSE()
    {
         audioSourceSE.Stop();
    }
    public void PlaySE2(AudioClip _seClip)
    {
         audioSourceSE2.PlayOneShot(_seClip);
    }
    public void StopSE2()
    {
         audioSourceSE2.Stop();
    }
}
