using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep : MonoBehaviour
{
    public AudioSource walking;
    public AudioSource runing;
    public Pistol gunOne;
    public GunSooter gunTwo;

    [SerializeField] private Animator playerAni;
    private enum State { idle, walk, run}
    private State state = State.idle;

    private void Awake()
    {
        playerAni = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        gunOne.enabled = true;
        gunTwo.enabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            walking.enabled = true;
            state = State.walk;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                walking.enabled = false;
                runing.enabled = true;
                state=State.run;

                gunOne.enabled = false;
                gunTwo.enabled = false;
            }
            else
            {
                runing.enabled = false;
                gunOne.enabled = true;
                gunTwo.enabled = true;
                state = State.walk;

            }
        }
        else
        {
            walking.enabled = false;
            runing.enabled = false;
            gunOne.enabled = true;
            gunTwo.enabled = true;
            state = State.idle;
        }

        playerAni.SetInteger("state", (int)state);
    }
}
