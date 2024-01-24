using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer mixer;
    //private float volumLevel;
    public Slider volumSlider;

    private void Start()
    {
        //float value;
      //  mixer.GetFloat("Volume", out value);
       // volumSlider.value = value;


    }

    public void SetVolum(float volume)
    {
        mixer.SetFloat("Volume", volume);
    }
}
