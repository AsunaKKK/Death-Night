using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPicup : MonoBehaviour
{
    public Item item;
    public GameObject picUpText;

    private void Start()
    {
        picUpText.SetActive(false);
    }
    void PickUp()
    {
        if (item == null)
        {
            return;
        }
        if (item != null)
        {
            InventoryManager.Instance.Add(item);
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (Input.GetKey(KeyCode.F))
            {
                PickUp();
                picUpText.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            picUpText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            picUpText.SetActive(false);
        }
    }



}
