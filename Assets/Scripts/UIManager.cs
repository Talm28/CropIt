using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject clockObj;
    public GameObject coinsObj;
    public GameObject heartsObj;
    public GameObject gameOverObj;
    public GameObject pauseObj;

    private GameManager _gameManager;
    private TextMeshProUGUI _clockText;
    private TextMeshProUGUI _coinsText;
    private GameObject[] _hearts = new GameObject[3];
    private TextMeshProUGUI gameOverProfitsText;
    private TextMeshProUGUI gameOverOrdersConpletedText;
    // Sounds
    [SerializeField] private AudioClip gameOverSound;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.instance;

        _clockText = clockObj.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        _coinsText = coinsObj.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();

        _hearts[0] = heartsObj.transform.GetChild(0).gameObject;
        _hearts[1] = heartsObj.transform.GetChild(1).gameObject;
        _hearts[2] = heartsObj.transform.GetChild(2).gameObject;

        gameOverProfitsText = gameOverObj.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        gameOverOrdersConpletedText = gameOverObj.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        PauseUpdate();
    }

    private void UpdateUI()
    {
        _clockText.text = _gameManager.globalClock.ToString();
        _coinsText.text = _gameManager.money.ToString();

        // Hearts update
        int health = _gameManager.health;
        for(int i=0; i<health ; i++) // Set active hearts
            _hearts[i].SetActive(true);
        for(int i=health ; i<3 ; i++) // Set deactive hearts
            _hearts[i].SetActive(false);
    }

    private void PauseUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_gameManager.isPause)
                DeactivePause();
            else
                ActivePause();
        }
    }

    public void ActiveGameOver()
    {
        gameOverOrdersConpletedText.text = "Orders completed: " + _gameManager.ordersCompleted.ToString();
        gameOverProfitsText.text = "Profits: " + _gameManager.profits.ToString();
        gameOverObj.SetActive(true);
        AudioManager.instence.PlaySound(gameOverSound);
        _gameManager.PauseGame();
    }

    public void ActivePause()
    {
        pauseObj.SetActive(true);
        _gameManager.PauseGame();
    }

    public void DeactivePause()
    {
        pauseObj.SetActive(false);
        _gameManager.ContinueGame();
    }
}
