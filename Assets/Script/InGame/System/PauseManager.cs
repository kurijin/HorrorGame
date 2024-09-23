using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ポーズ画面のマネージャー
/// Escapeボタンでポーズ画面を起動する
/// </summary>

public class PauseManager : MonoBehaviour
{
    [SerializeField,Header("遊び方ボタン")] private Button _howtoButton;
    [SerializeField,Header("終了ボタン")] private Button _retireButton;

    [SerializeField,Header("遊び方UI")] private GameObject _howToPlayUI;
    [SerializeField,Header("バックボタン")] private Button _backButton;

    [SerializeField,Header("クリック音")] private AudioClip _okSE;

    private void OnEnable()
    {
        _howtoButton.onClick.AddListener(DisPlayHowto);
        _backButton.onClick.AddListener(Back);

        _retireButton.onClick.AddListener(Finish);
    }

    private void DisPlayHowto()
    {
        SoundManager.Instance.PlaySE(_okSE);
        _howToPlayUI.SetActive(true);
    }

    private void Back()
    {
        SoundManager.Instance.PlaySE(_okSE);
        _howToPlayUI.SetActive(false);
    }

    private void Finish()
    {
        SoundManager.Instance.PlaySE(_okSE);
        InGameFlow.Instance.End();
    }
}
