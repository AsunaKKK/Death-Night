using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public CharacterController playerController;
    public float speed = 0f;
    public float Run = 0f;
    public float jumpHeight = 3f;
    public float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    public float MaxHealth = 100f;
    public float currenHp;
    public Image healthbar;

    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currenHp = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        MoveMent();
        Jump();
        BarHp();
    }

    //make the player have movement with the press of a button
    public void MoveMent()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        playerController.Move(move * speed * Time.deltaTime);

        playerController.Move(velocity * Time.deltaTime);

        //Player will run if hold down the button Shift
        if (Input.GetButton("Fire3"))
        {
            print("RUN");
            playerController.Move(move * Run * Time.deltaTime);
        }
    }


    //make the player have Jump with the press of a button Specba
    public void Jump()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        //Create Sphere to check jumping
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // set keybord to have jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void enemyTakeDamage(float edmg)
    {

        if (currenHp <= 0f)
        {
            Die();
        }
        currenHp -= edmg;

    }
    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BarHp()
    {
        healthbar.fillAmount = currenHp / MaxHealth;
    }

    public void Healing (int value)
    {
        if(currenHp >= MaxHealth)
        {
            currenHp = MaxHealth;
        }
        else
        {
            currenHp = currenHp + value;
            Debug.Log("Heal");
        }
    }
}
