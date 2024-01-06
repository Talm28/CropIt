using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class StoreSlot : MonoBehaviour, IPointerClickHandler
{
    private GameManager _gameManager;
    //Sounds
    [SerializeField] private AudioClip unlockSound;
    [SerializeField] private AudioClip lockOpen;
    // UI 
    private Image _itemImage;
    private Image _lockImage;
    private TextMeshProUGUI _priceText;
    [SerializeField] private Sprite _closeLockImage;
    [SerializeField] private Sprite _openLockImage;
    // Events
    public UnityEvent slotUnlock;
    // Variables
    private bool _isBought;
    private bool _isUnlock;
    private int _price;
    [SerializeField] private int _slotNumber;

    void Awake()
    {
        _price = GameUtils.storePrices[_slotNumber];
        _lockImage = transform.GetChild(2).gameObject.GetComponent<Image>();
        // Color set
        _itemImage = transform.GetChild(1).gameObject.GetComponent<Image>();
        Color newColor = _itemImage.color;
        newColor.a = 0.5f;
        _itemImage.color = newColor;
        // Text set
        _priceText = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        _priceText.text = _price.ToString();
        _isBought = false;
        _isUnlock = false;

        slotUnlock.AddListener(UnlockSlot);
    }
    void Start()
    {
        _gameManager = GameManager.instance;

        if(_slotNumber == 0)
        {
            UnlockSlot();
            _isBought = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckForOpen();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(_gameManager.money >= _price && !_isBought && _gameManager.gameStage == _slotNumber)
        {
            slotUnlock.Invoke();
            _isBought = true;
        }   
    }

    private void UnlockSlot()
    {
        _priceText.text = "";
        Color newColor = _itemImage.color;
        newColor.a = 1f;
        _itemImage.color = newColor;
        _lockImage.gameObject.SetActive(false);
        if(_slotNumber != 0)
        {
            _gameManager.TakeMoney(_price);
            _gameManager.IncreaseGameStage();
            AudioManager.instence.PlaySound(unlockSound);
        }
    }

    private void CheckForOpen()
    {
        if(!_isUnlock && _gameManager.gameStage == _slotNumber && _gameManager.money >= _price) // Unlock check
        {
            _isUnlock = true;
            _lockImage.sprite = _openLockImage;
            AudioManager.instence.PlaySound(lockOpen);
        }

        if(_isUnlock && _gameManager.gameStage == _slotNumber && _gameManager.money < _price) // Check if was unlock and cant unlock now
        {
            _isUnlock = false;
            _lockImage.sprite = _closeLockImage;
        }
    }
}
