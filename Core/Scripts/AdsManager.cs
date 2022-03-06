using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{
    private RewardedAd _rewardedAd;
    private string _rewardedUnitID = "test";

    private void Awake() {
        MobileAds.Initialize(initStatus => { });
    }

    private void OnEnable(){
        _rewardedAd = new RewardedAd(_rewardedUnitID);
        AdRequest _adRequest = new AdRequest.Builder().Build();
        _rewardedAd.LoadAd(_adRequest);
        _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
    }

    public void HandleUserEarnedReward(object sender, System.EventArgs args){
        int _coins = PlayerPrefs.GetInt("PlayerMoney");
        _coins += 15;
        PlayerPrefs.SetInt("PlayerMoney", _coins);
    }

    public void ShowAd(){
        if(_rewardedAd.IsLoaded()){
            _rewardedAd.Show();
        }
    }
}
