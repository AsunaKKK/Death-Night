using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public CamereController setSensitivity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetSensitivity (float sen)
    {
        setSensitivity.sensitivity = sen;
        
    }
}
