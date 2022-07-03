using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private void Awake()
    {
        instance = this;
    }

    public AudioSource Click;
    public AudioSource buttonClick;
    public AudioSource sellFX;
    public AudioSource buyFX;
    public AudioSource error;
    public AudioSource woodChop;
    public void PlayButtonClickFX()
    {
        buttonClick.Play();
    }
}
