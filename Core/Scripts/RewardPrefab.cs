using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardPrefab : MonoBehaviour
{
    [SerializeField]
    private Image _background;
    [SerializeField]
    private Color _defaultColor;
    [SerializeField]
    private Color _currentColor;

    [Space(5)]
    [SerializeField]
    private TMP_Text _rewardValueText;
    [SerializeField]
    private TMP_Text _dayText;

    [Space(5)]
    [SerializeField]
    private Image _rewardIcon;
    [SerializeField]
    private Sprite _coinsSprite;
    [SerializeField]
    private Sprite _skinsSprite;

    public void SetRewardData(int day, int currentState, Reward reward){
        _dayText.text = $"Day {day+1}";
        _rewardIcon.sprite = reward.m_type == Reward.RewardType.COINS ? _coinsSprite : _skinsSprite;
        _rewardValueText.text = reward.m_value.ToString();
        _background.color = day == currentState ? _currentColor : _defaultColor;
    }
}
