using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider volumSlider;

    private void Start()
    {
        // Cuando el juego inicia, obtén el valor actual del volumen del Audio Mixer
        float value;
        mixer.GetFloat("Volum", out value);

        // Convierte el valor del Audio Mixer al rango del Slider
        float sliderValue = MapToSliderValue(value);
        volumSlider.value = sliderValue;
    }

    public void SetVolum(float sliderValue)
    {
        // Convierte el valor del Slider al rango del Audio Mixer
        float mixerValue = MapToMixerValue(sliderValue);
        mixer.SetFloat("Volum", mixerValue);
    }

    private float MapToSliderValue(float mixerValue)
    {
        // Mapea el valor del Audio Mixer al rango del Slider (0 a 1)
        return (mixerValue + 80f) / 100f;
    }

    private float MapToMixerValue(float sliderValue)
    {
        // Mapea el valor del Slider al rango del Audio Mixer (-80 a 20)
        return sliderValue * 100f - 80f;
    }
}
