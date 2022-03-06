using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static MenuManager m_instance;

    [Header("All Player Money")]
    public TMP_Text m_allCoinsText;

    [Space(5)]
    [Header("For Loading First Scene")]
    public Animator m_sceneLoadingCameraAnim;
    public Animator m_menuUIOnLoadAnim;

    [Space(5)]
    [Header("For Setting Window")]
    public Animator m_settingsPanelAnim;

    [Space(5)]
    [Header("Shop Panel Parameters")]
    public Animator m_shopPanelAnim;
    public GameObject m_player;
    public Material[] m_playerSkinsMaterials;

    [Space(5)]
    [Header("DailyRewards")]
    public Animator m_dailyRewardsAnim;
    public Animator m_dailyButtonAnim;
    public TMP_Text m_statusText;
    public Button m_takeBtn;
    public RewardPrefab m_rewardPrefab;
    public Transform m_rewardPrefabGrid;

    private int CurrentDay{
        get => PlayerPrefs.GetInt("currentDay", 0);
        set => PlayerPrefs.SetInt("currentDay", value);
    }

    private DateTime? LastTakesTime{
        get{
            string data = PlayerPrefs.GetString("lastTakesTime", null);

            if(!string.IsNullOrEmpty(data))
                return DateTime.Parse(data);

            return null;
        }
        set{
            if(value != null)
                PlayerPrefs.SetString("lastTakesTime", value.ToString());
            else{
                PlayerPrefs.DeleteKey("lastTakesTime");
            }
        }
    }

    public List<Reward> m_rewards;
    private List<RewardPrefab> m_rewardedPrefabs;

    bool _canTake;
    int _maxDays = 5;
    float _cooldownToTakes = 24f;
    float _deadlineToTakes = 48f;

    [Space(5)]
    [Header("GamePlay Manager")]
    public GameObject m_gamePlayManagerPrefab; 

    private void Awake() {
        if(m_instance==null)
            m_instance=this;
    }

    private void Start() {
        InitPrefabs();
        StartCoroutine(RewardsStateUpdater());
    }

    private void Update() {
        m_player.GetComponent<Renderer>().material = m_playerSkinsMaterials[PlayerPrefs.GetInt("skinNum")];

        if(PlayerPrefs.HasKey("PlayerMoney")){
            m_allCoinsText.text = "YOUR COINS: " + PlayerPrefs.GetInt("PlayerMoney", m_gamePlayManagerPrefab.GetComponent<GamePlayManager>().m_allCoins).ToString();
        }
        else{
            m_allCoinsText.text = "YOUR COINS: 0";
        }
    }

    #region DailyRewards
    public void AddCoins(int value){
        m_gamePlayManagerPrefab.GetComponent<GamePlayManager>().m_allCoins += value;
    }
    public void AddSkin(int value){
        
    }

    public void OpenDailyRewardsPanel(){
        m_dailyRewardsAnim.SetTrigger("Open");
    }
    public void CloseDailyRewardsPanel(){
        m_dailyRewardsAnim.SetTrigger("Close");
    }

    public void InitPrefabs(){
        m_rewardedPrefabs = new List<RewardPrefab>();
        for(int i=0; i<_maxDays; i++){
            m_rewardedPrefabs.Add(Instantiate(m_rewardPrefab, m_rewardPrefabGrid, false));
        }
    }

    private IEnumerator RewardsStateUpdater(){
        while(true){
            UpdateRewardsState();
            yield return new WaitForSeconds(1);
        }
    }

    private void UpdateRewardsState(){
        _canTake = true;
        if(LastTakesTime.HasValue){
            var timeSpan = DateTime.UtcNow - LastTakesTime.Value;
            if(timeSpan.TotalHours > _deadlineToTakes){
                LastTakesTime = null;
                CurrentDay = 0;
            }
            else if(timeSpan.TotalHours < _cooldownToTakes){
                _canTake = false;
            }
        }

        UpdateRewardUI();
    }

    private void UpdateRewardUI(){
        m_takeBtn.interactable = _canTake;
        if(_canTake){
            m_statusText.text = "Take your reward!";
            m_dailyButtonAnim.SetTrigger("NewReward");
        }
        else{
            var nextTakeTime = LastTakesTime.Value.AddHours(_cooldownToTakes);
            var currentTakeCooldown = nextTakeTime - DateTime.UtcNow;

            string cooldown = $"{currentTakeCooldown.Hours:D2}:{currentTakeCooldown.Minutes:D2}:{currentTakeCooldown.Seconds:D2}";

            m_statusText.text = $"Come in {cooldown} to take the daily reward";
        }

        for(int i=0; i<m_rewardedPrefabs.Count; i++){
            m_rewardedPrefabs[i].SetRewardData(i, CurrentDay, m_rewards[i]);
        }
    }

    public void TakeReward(){
        if(!_canTake)
            return;
        
        var reward = m_rewards[CurrentDay];
        switch(reward.m_type){
            case Reward.RewardType.COINS:
                MenuManager.m_instance.AddCoins(reward.m_value);
                break;
            case Reward.RewardType.SKINS:
                MenuManager.m_instance.AddSkin(reward.m_value);
                break;
        }

        LastTakesTime = DateTime.UtcNow;
        CurrentDay = (CurrentDay + 1) % _maxDays;

        UpdateRewardsState();
    }
    #endregion

    #region ShopManager
    public void OpenShopPanel(){
        m_shopPanelAnim.SetTrigger("Open");
        m_menuUIOnLoadAnim.SetTrigger("Loading");
    }
    public void CloseShopPanel(){
        m_shopPanelAnim.SetTrigger("Close");
        m_menuUIOnLoadAnim.SetTrigger("Open");
    }
    #endregion

    #region PublicMethods
    public void OnClickPlayButton(){
        m_sceneLoadingCameraAnim.SetTrigger("Loading");
        m_menuUIOnLoadAnim.SetTrigger("Loading");
        StartCoroutine(FirstSceneLoad());
    }

    public void OnClickOpenSettingsButton(){
        m_settingsPanelAnim.SetTrigger("Open");
        m_menuUIOnLoadAnim.SetTrigger("Loading");
    }
    public void OnClickCloseSettingsPanel(){
        m_settingsPanelAnim.SetTrigger("Close");
        m_menuUIOnLoadAnim.SetTrigger("Open");
    }

    public void OnClickQuitButton(){
        Application.Quit();
    }
    #endregion

    IEnumerator FirstSceneLoad(){
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }
}
