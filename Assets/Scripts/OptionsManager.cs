using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    AudioManager audioManager;

    [SerializeField]private Slider musicSlider;
    [SerializeField]private Slider soundSlider;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instence;

        InitializeSliders();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSliders();
    }

    public void InitializeSliders()
    {
        musicSlider.value = audioManager.musicSource.volume;
        soundSlider.value = audioManager.soundSource.volume;
    }

    public void UpdateSliders()
    {
        audioManager.musicSource.volume = musicSlider.value;
        audioManager.soundSource.volume = soundSlider.value;
    }
}
