using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamereController : MonoBehaviour
{
    [Header("LookProperties")]
    [SerializeField, Range(0, 200f)]
    public float sensitivity;
    [SerializeField, Range(0, 1f)]
    float slerpIndex;
    public Transform parentBody;
    public float viewLimit;

    float Xrot, Yrot;

    // Start is called before the first frame update
    void Start()
    {
        //lock Cursor Mouse remove from desktop
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Control Speed Mouse
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime;
        Xrot += mouseX * sensitivity;
        Yrot = Mathf.Clamp(Yrot, -viewLimit, viewLimit);
        Yrot -= mouseY * sensitivity;
        Quaternion from = parentBody.transform.localRotation;
        Quaternion to = Quaternion.Euler(0, Xrot, 0);
        Quaternion fromY = transform.localRotation;
        Quaternion toY = Quaternion.Euler(Yrot, 0, 0);
        transform.localRotation = Quaternion.Slerp(fromY, toY, slerpIndex);
        parentBody.transform.localRotation = Quaternion.Slerp(from, to, slerpIndex);

    }
}
