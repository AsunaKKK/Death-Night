using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pistol : MonoBehaviour
{
    public Camera cameraPlayer;
    public float firerate = 0;
    public float gunDamage = 0;
    public float range = 1000f;
    float nextTimeToFire = 0f;
    public float impactForce = 30f;

    public int maxAmmo = 0;
    private int currentAmmo = 0;
    public float reloadTime = 0f;
    private bool isReloading = false;

    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public AudioSource soundPlayer;
    public AudioClip fireSound;
    public AudioSource reloadSound;

    [SerializeField] private Text AmmoShow;
    [SerializeField] private Animator playerAnim;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        currentAmmo = maxAmmo;

    }

    private void OnEnable()
    {
        isReloading = false;
        if(playerAnim != null)
        {
            playerAnim.SetBool("Reloading", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
        {
            return;
        }
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetButtonDown("Reload"))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            soundPlayer.clip = fireSound;
            soundPlayer.Play();

            nextTimeToFire = Time.time + 1f / firerate;
            shoot();
            playerAnim.Play("Frie");
        }
        AmmoShow.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
    }
    //function shooting of player
    void shoot()
    {
        muzzleFlash.Play();
        currentAmmo--;

        RaycastHit hit;
        if (Physics.Raycast(cameraPlayer.transform.position, cameraPlayer.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            if (hit.transform.CompareTag("Enemy"))
            {
                AiController enemy = hit.transform.GetComponent<AiController>();
                if (enemy != null)
                {
                    enemy.takeDamage(gunDamage);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }
            }
            GameObject impactGameobj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGameobj, 2f);
        }
    }

    //function Reloading of player
    IEnumerator Reload()
    {
        Debug.Log("Reloading");
        isReloading = true;
        playerAnim.SetBool("Reloading", true);
        reloadSound.Play();

        yield return new WaitForSeconds(reloadTime - .11f);
        playerAnim.SetBool("Reloading", false);
        yield return new WaitForSeconds(.11f);
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
