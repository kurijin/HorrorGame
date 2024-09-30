using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーがチェックポイントに訪れた時の管理をするもの
/// 進行度0:初期
/// 進行度1:チュートリアル終了
/// </summary>
public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager Instance { get; private set; }

    [SerializeField, Header("ゲームの進行度")]
    private int checkPoint = 0;

    public int Check
    {
        get { return checkPoint; }
        set
        {
            checkPoint = value;
            ItemManager.Instance.SaveItemList();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
