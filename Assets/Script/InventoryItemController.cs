using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    public Button removeButton;
    Item item;
    public void RemoveItem()
    {
        InventoryManager.Instance.Remove(item);
        Destroy(gameObject);
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
    }

    public void UseItem()
    {
        if (item == null)
        {
            return;
        }

        switch (item.itemType)
        {
            case Item.ItemType.Hp:
                PlayerController.instance.Healing(item.hpValue);
                break;
            case Item.ItemType.Bullet:
                GunSooter.instance.AddAmmo(item.bulletValue);
                break ;
            default:
                break;
        }
        RemoveItem();
    }
}
