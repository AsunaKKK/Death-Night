using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletItem : MonoBehaviour
{
    public int bullet = 30;
    public GameObject picUpText;
    // Start is called before the first frame update
    void Start()
    {
        picUpText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            picUpText.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other == null) return;

        if (other.tag == "Player")
        {
            if (Input.GetKey(KeyCode.F))
            {
                GunSooter addBullet = other.GetComponent<GunSooter>();

                if (addBullet != null)
                {
                    addBullet.AddAmmo(bullet);
                    picUpText.SetActive(false);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.LogError("GunSooter component not found on player!");
                }
            }
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
