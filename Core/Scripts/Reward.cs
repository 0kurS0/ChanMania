[System.Serializable]
public class Reward
{
    public enum RewardType{
        COINS,
        SKINS
    }

    public RewardType m_type;
    public int m_value;
    public string m_name;
}
