using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテムの所持状況を考えるもの
/// リストでアイテム管理
/// </summary>
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }  

    public List<string> itemList = new List<string>();  

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);  
        }
    }

    /// <summary>
    /// アイテムを取得した時Itemのスクリプトからここにアイテム名を引数にして飛ばす
    /// </summary>
    /// <param name="_item">取得するアイテム名</param>
    public void GetItem(string _item)
    {
        itemList.Add(_item);
        Debug.Log(_item + " を取得しました。");
    }

    /// <summary>
    /// チェックポイントが更新されるたびにその時点でのPlayerPrefsにアイテム情報を保存
    /// </summary>
    public void SaveItemList()
    {
        string items = string.Join(",", itemList);  
        PlayerPrefs.SetString("ItemList", items);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// チェックポイントから呼ばれた時その時時点で、以前保存してたアイテムリストから取得
    /// もしこの時インベントリにアイテムを持ってるならシーン内のアイテムを削除
    /// </summary>
    public void LoadItemList()
    {
        if (PlayerPrefs.HasKey("ItemList"))
        {
            string savedItems = PlayerPrefs.GetString("ItemList");
            itemList = new List<string>(savedItems.Split(','));  
        }
        else
        {
            itemList = new List<string>();
        }
         
        GameObject[] itemsInScene = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in itemsInScene)
        {
            Item itemComponent = item.GetComponent<Item>();
            if (itemComponent != null && itemList.Contains(itemComponent.itemName))
            {
                Destroy(item);
            }
        }
    }

    /// <summary>
    /// アイテムリストを消す
    /// 必要性?????
    /// </summary>
    public void ClearItemList()
    {
        itemList.Clear();
    }
}
