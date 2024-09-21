using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームの設定画面での音量の変更
/// </summary>
public class SoundSlider : MonoBehaviour
{
    [SerializeField,Header("BGMのスライダー")] private Slider _bgmSlider;
    [SerializeField,Header("SEのスライダー")] private Slider _seSlider;
    private float _currentBGMvalue;
    private float _currentSEvalue;


    void OnEnable()
    {
        _bgmSlider.value = SoundManager.Instance.audioSourceBGM.volume;
        _seSlider.value  = SoundManager.Instance.audioSourceSE.volume;
    }

    void Update()
    {
        SoundManager.Instance.audioSourceBGM.volume = _bgmSlider.value;
        SoundManager.Instance.audioSourceSE.volume = _seSlider.value;
    }
}
