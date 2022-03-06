using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class ShowBanner : MonoBehaviour
{
    private BannerView _bannerAd;

    private const string _bannerUnitID = "ca-app-pub-2176948028854320/4414036543";

    void OnEnable(){
        _bannerAd = new BannerView(_bannerUnitID, AdSize.SmartBanner, AdPosition.Bottom);
        AdRequest _adRequest = new AdRequest.Builder().Build();
        _bannerAd.LoadAd(_adRequest);
        StartCoroutine(ShowBannerAd());
    }

    IEnumerator ShowBannerAd(){
        yield return new WaitForSeconds(1);
        _bannerAd.Show();
    }
}
