using UnityEngine;

public class Player : MonoBehaviour
{
    private void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "DeathZone"){
            GamePlayManager.m_instance.PlayerDeath();
        }
    }
}
