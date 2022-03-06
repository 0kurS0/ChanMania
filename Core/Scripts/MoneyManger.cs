using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManger : MonoBehaviour
{ 
    public AudioSource m_coinTakesSrc;

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            GamePlayManager.m_instance.m_moneyValue++;
            GamePlayManager.m_instance.m_maxMoneyToWin++;
            gameObject.SetActive(false);
            m_coinTakesSrc.Play();
        }
    }
}
