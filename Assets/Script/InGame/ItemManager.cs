using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }  

    public List<string> itemList = new List<string>();  

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  
        }
    }

    // アイテムをリストに追加するメソッド
    public void GetItem(string _item)
    {
        itemList.Add(_item);
        Debug.Log(_item + " を取得しました。");
    }
}
