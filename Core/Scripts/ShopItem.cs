using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public int m_skinNum;
    public int m_priceSkin;

    public Image[] m_skins;

    public TMP_Text m_priceText;

    public GameObject m_gamePlayManagerPrefab;

    private void Start() {
        if(PlayerPrefs.GetInt("Skin1" + "buy") == 0){
            foreach(Image image in m_skins){
                if("Skin1" == image.name){
                    PlayerPrefs.SetInt("defaultSkin" + "buy", 1);
                }
                else{
                    PlayerPrefs.SetInt(GetComponent<Image>().name + "bought", 0);
                }
            }
        }
    }

    private void Update() {
        if(PlayerPrefs.GetInt(GetComponent<Image>().name + "buy") == 0){
            m_priceText.text = m_priceSkin.ToString();
        }
        else if(PlayerPrefs.GetInt(GetComponent<Image>().name + "buy") == 1){
            if(PlayerPrefs.GetInt(GetComponent<Image>().name + "equip") == 1){
                m_priceText.text = "EQUIPPED";
            }
            else if(PlayerPrefs.GetInt(GetComponent<Image>().name + "equip") == 0){
                m_priceText.text = "EQUIP";
            }
        }
    }

    public void Buy(){
        if(PlayerPrefs.GetInt(GetComponent<Image>().name + "buy") == 0){
            if(PlayerPrefs.GetInt("PlayerMoney", m_gamePlayManagerPrefab.GetComponent<GamePlayManager>().m_allCoins) >= m_priceSkin){
                m_priceText.text = "EQUIPPED";
                PlayerPrefs.SetInt("PlayerMoney", PlayerPrefs.GetInt("PlayerMoney") - m_priceSkin);
                PlayerPrefs.SetInt(GetComponent<Image>().name + "buy", 1);
                PlayerPrefs.SetInt("skinNum", m_skinNum);
                foreach(Image image in m_skins){
                    if(GetComponent<Image>().name == image.name){
                        PlayerPrefs.SetInt(GetComponent<Image>().name + "equip", 1);
                    }
                    else{
                        PlayerPrefs.SetInt(image.name + "equip", 0);
                    }
                }
            }
        }
        else if(PlayerPrefs.GetInt(GetComponent<Image>().name + "buy") == 1){
                m_priceText.text = "EQUIPPED";
                PlayerPrefs.SetInt(GetComponent<Image>().name + "equip", 1);
                PlayerPrefs.SetInt("skinNum", m_skinNum);
                foreach(Image image in m_skins){
                    if(GetComponent<Image>().name == image.name){
                        PlayerPrefs.SetInt(GetComponent<Image>().name + "equip", 1);
                    }
                    else{
                        PlayerPrefs.SetInt(image.name + "equip", 0);
                    }
                }
            }
    }
}
