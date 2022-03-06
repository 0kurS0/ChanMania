using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private AudioSource _audioSrc;  

    private void Start() {
        _audioSrc = gameObject.GetComponent<AudioSource>();
        if(!PlayerPrefs.HasKey("MusicValue"))
            _audioSrc.volume = 0.7f;

        // Весь звук на сцене - AudioListener.volume
    } 

    private void Update() {
        _audioSrc.volume = PlayerPrefs.GetFloat("MusicValue");
    }
}
