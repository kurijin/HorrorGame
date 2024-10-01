using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;


public class Lock : MonoBehaviour
{
    [SerializeField,Header("必要なアイテム名")] private string _needItem;
    [SerializeField,Header("鍵開ける音")] private AudioClip _unlockSE;
    [SerializeField,Header("鍵開けた時のメッセージ")] private string _unlockMessage;
    [SerializeField,Header("鍵開けなかった音")] private AudioClip _lockSE;
    [SerializeField,Header("鍵開けれなかった時のメッセージ")] private string _lockMessage;
    private Animator _doorAnima;

    void Start()
    {
        _doorAnima = GetComponent<Animator>();
    }

    public void ClearLock()
    {
        if(ItemManager.Instance.itemList.Contains(_needItem))
        {
            SoundManager.Instance.PlaySE3(_unlockSE);
            InGameFlow.Instance.ShowMessage(_unlockMessage).Forget();
            Destroy(this.gameObject);
        }
        else
        {
            SoundManager.Instance.PlaySE3(_lockSE);
            InGameFlow.Instance.ShowMessage(_lockMessage).Forget();
        }
    }
}
