using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コレクトアイテムのランダムリスポーン
/// </summary>
public class ItemSpawnManager : MonoBehaviour
{
    [SerializeField, Header("出現させるアイテムのプレハブ")] private GameObject[] itemPrefabs;  
    [SerializeField, Header("アイテムが出現するポイント")] private Transform[] spawnPoints;  

    private void Awake()
    {
        SpawnRandomItems();
    }

    /// <summary>
    /// アイテムと出現可能性スポットからランダムに2つずつ選んで生成する
    /// </summary>
    private void SpawnRandomItems()
    {
        // アイテムプレハブからランダムに2つ
        List<GameObject> availableItems = new List<GameObject>(itemPrefabs);
        GameObject selectedItem1 = GetRandomItem(availableItems);
        GameObject selectedItem2 = GetRandomItem(availableItems);

        // 出現地点可能性から2つ選択する
        List<Transform> availablePoints = new List<Transform>(spawnPoints);
        Transform selectedPoint1 = GetRandomSpawnPoint(availablePoints);
        Transform selectedPoint2 = GetRandomSpawnPoint(availablePoints);

        // 選ばれた地点に選ばれた選ばれたアイテムを出現させる
        Instantiate(selectedItem1, selectedPoint1.position, Quaternion.Euler(-90f, 0f, 0f));
        Instantiate(selectedItem2, selectedPoint2.position, Quaternion.Euler(-90f, 0f, 0f));
    }

    private GameObject GetRandomItem(List<GameObject> availableItems)
    {
        int randomIndex = Random.Range(0, availableItems.Count);
        GameObject selectedItem = availableItems[randomIndex];
        //選んだアイテムはリストから1回消す
        availableItems.RemoveAt(randomIndex); 
        return selectedItem;
    }

    private Transform GetRandomSpawnPoint(List<Transform> availablePoints)
    {
        int randomIndex = Random.Range(0, availablePoints.Count);
        Transform selectedPoint = availablePoints[randomIndex];
        // 選んだ地点をリストから削除
        availablePoints.RemoveAt(randomIndex);  
        return selectedPoint;
    }
}
