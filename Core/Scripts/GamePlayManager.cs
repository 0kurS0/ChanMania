using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager m_instance;

    [SerializeField]
    private Rigidbody _rb;
    [SerializeField]
    private FixedJoystick _joystick;
    [SerializeField]
    private float _movementSpeed = 2f;
    [SerializeField]
    private ParticleSystem _playerDeathParticle;
    [SerializeField]
    private GameObject _playerUIDeathPanel;
    [SerializeField]
    private Animator _nextLevelAnim;
    [SerializeField]
    private Camera _mainCamera;
    [SerializeField]
    private Transform _cameraTarget;
    [SerializeField]
    private AudioSource _winLevelSrc;
    
    [HideInInspector]
    public int m_moneyValue = 0;
    [HideInInspector]
    public int m_allCoins = 0;
    public TMP_Text m_moneyValueText;
    public float m_maxMoneyToWin = 0;
    public GameObject m_player;
    public Material[] m_playerSkinsMaterials;

    float _cameraMoveSpeed = 5f;
    float _targetPosX;

    bool _playerIsLive = true;

    #region PrivateMethods
    private void Awake() {
        if(m_instance==null)
            m_instance = this;
    }

    private void Start() {
        m_allCoins = PlayerPrefs.GetInt("PlayerMoney");
    }

    private void Update() {
        if(m_player!=null)
            m_player.GetComponent<Renderer>().material = m_playerSkinsMaterials[PlayerPrefs.GetInt("skinNum")];
        
        m_moneyValueText.text = m_moneyValue.ToString();
        if(m_maxMoneyToWin == 3){
            WinGame();
            PlayerPrefs.SetInt("PlayerMoney", m_allCoins);
            PlayerPrefs.Save();
        }
         Vector3 _cameraPos = Vector3.Lerp(_mainCamera.transform.position, _cameraTarget.position, _cameraMoveSpeed * Time.deltaTime);
        _mainCamera.transform.position = _cameraPos;
        _targetPosX = _cameraTarget.position.x - 9.75f;
    }

    private void FixedUpdate(){
        if(_playerIsLive)
            _rb.velocity = new Vector3(-_joystick.Horizontal * _movementSpeed, _rb.velocity.y, -_joystick.Vertical * _movementSpeed);
    }
    #endregion

    public void PlayerDeath(){
        _playerIsLive = false;
        _playerDeathParticle.Play();
        Destroy(_rb.gameObject);
        _playerUIDeathPanel.transform.GetChild(0).gameObject.SetActive(true);
        _playerUIDeathPanel.GetComponent<Animator>().SetTrigger("Open");
    }

    public void WinGame(){
        m_allCoins += 3;
        m_maxMoneyToWin = 0;
        _nextLevelAnim.SetTrigger("Win");
        ChunkGenerator.m_instance.SpawnChunk();
        
        _winLevelSrc.Play();
        _cameraTarget.position = new Vector3(_targetPosX, _cameraTarget.position.y, _cameraTarget.position.z);
    }

    #region ApplicationManagers
    public void RestartLevel(){
        Application.LoadLevel(Application.loadedLevel);
    }

    public void LoadMenu(){
        SceneManager.LoadScene(0);
    }

    public void QuitGame(){
        Application.Quit();
    }
    #endregion
}
