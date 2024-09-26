using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField, Header("アイテム名")] private string itemName;
    [SerializeField,Header("アイテム取得音")] private AudioClip _itemGetSE;

    public void OnItemPickedUp()
    {
        ItemManager.Instance.GetItem(itemName);
        SoundManager.Instance.PlaySE3(_itemGetSE);
        Destroy(this.gameObject);
    }
}
