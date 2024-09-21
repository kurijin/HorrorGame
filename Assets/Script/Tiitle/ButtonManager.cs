using UnityEngine;
using UnityScreenNavigator.Runtime.Core.Page;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class ButtonManager : MonoBehaviour
{
    //[SerializeField, Header("サウンドマネージャーのオブジェクト")] private SoundManager _soundManager;
    //[SerializeField, Header("ボタンのクリック音")] private AudioClip _buttonSE;
    [SerializeField, Header("シーン管理のCanvasのスクリプト参照(page)")] private PageContainer _pageContainer;
    [SerializeField, Header("クリック時のパーティクル")] private GameObject _clickParticle; 

    // 最初の画面のPlay
    public void OnClickPlay()
    {
        PlaySoundAndPushPageAsync("CreditPage").Forget();
    }

    // クレジットボタン
    public void OnClickCredit()
    {
        PlaySoundAndPushPageAsync("CreditPage").Forget();
    }

    // 収集ボタン
    public void OnClickCollection()
    {
        PlaySoundAndPushPageAsync("CollectionPage").Forget();
    }

    // 設定ボタン
    public void OnClickSettings()
    {
        PlaySoundAndPushPageAsync("SettingsPage").Forget();
    }

    // 共通のメソッド(ページ)
    private async UniTask PlaySoundAndPushPageAsync(string pageName)
    {
        //_soundManager.PlaySE(_buttonSE);
        ClickParticle(_clickParticle);
        // ページ遷移
        await _pageContainer.Push(pageName, true);
    }

    // 共通のBackボタン
    public void OnClickBacktoTitle()
    {
        BackToTitleAsync().Forget();
    }


    private async UniTask BackToTitleAsync()
    {
        //_soundManager.PlaySE(_buttonSE);
        ClickParticle(_clickParticle);
        await _pageContainer.Pop(true);
    }

    /// <summary>
    /// クリックして流したいParicleを流す.引数は流したいparicle
    /// <param name="_particleName"></param> 
    /// <summary>
    private void ClickParticle(GameObject _particleName)
    {
        Vector3 clickPosition = Input.mousePosition;
        clickPosition.z = 10f;
        clickPosition = Camera.main.ScreenToWorldPoint(clickPosition);
        Instantiate(_particleName,clickPosition, Quaternion.identity); 
    }
}
