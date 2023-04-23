using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangScene : MonoBehaviour
{
    public GameObject win;

    private void Start()
    {
        win.SetActive(false);
        Time.timeScale = 1;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
          win.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
    }

}
