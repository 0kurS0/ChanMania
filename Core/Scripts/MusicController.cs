using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public Slider m_musicSlider;
    [HideInInspector]
    public float m_musicValue;

    private void Start() {
        m_musicValue = m_musicSlider.value;
        if(!PlayerPrefs.HasKey("MusicValue"))
            m_musicSlider.value = 0.7f;
        else{
            m_musicSlider.value = PlayerPrefs.GetFloat("MusicValue");
        }
    }
    private void Update() {
        if(m_musicValue != m_musicSlider.value){
            PlayerPrefs.SetFloat("MusicValue", m_musicSlider.value);
            PlayerPrefs.Save();
            m_musicValue = m_musicSlider.value;
        }
    }
}
