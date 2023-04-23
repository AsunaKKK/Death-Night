using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInventory : MonoBehaviour
{
    public GameObject inventory;
    public InventoryManager inventoryManager;


    // Start is called before the first frame update
    void Start()
    {
        inventory.SetActive(false);
        inventoryManager.GetComponent<InventoryManager>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Openinventory();
    }

    public void Openinventory()
    {
        if(Input.GetKey(KeyCode.T))
        {
            if(Time.timeScale == 1)
            {
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                inventory.SetActive(true);
                inventoryManager.ListItems();
            }
        }
    }

    public void CloseInventory()
    {
        Time.timeScale = 1;
        inventory.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
