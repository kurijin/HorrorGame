using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音楽を鳴らすためのもの,他スクリプトからこのオブジェクトを参照して、ClipNameで鳴らしたい音を引数で渡す
/// </summary>
public class SoundManager : MonoBehaviour
{
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySE(AudioClip clipName)
    {
        _audioSource.PlayOneShot(clipName);
    }

    public void StopSE()
    {
        _audioSource.Stop();
    }
}
