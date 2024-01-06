using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;

public class OrderItem
{
    public string name;
    public int currentAmount;
    public int maxAmount;

    public OrderItem(string newName, int amount)
    {
        this.name = newName;
        this.currentAmount = 0;
        this.maxAmount = amount;
    }
}

public class Order : MonoBehaviour
{
    private GameManager _gameManager;

    // UI
    private List<Image> _itemsImages;
    private List<TextMeshProUGUI> _itemsTexts;
    private GameObject _valueTextOject;
    private TextMeshProUGUI _valueText;
    private GameObject _valueImageObject;
    private Image _valueImage;
    private GameObject _specialValueObject;
    private Image _specialValueImage;
    [SerializeField] private Sprite[] _valueSprites;
    private Slider _slider;
    [SerializeField] private Image _sliderFiller;
    private Color32 _sliderOriginColor;
    private ParticleSystem _musicEffect;
    // Quantities
    private List<OrderItem> _orderContent;
    // Order variables
    public int orderNumber;
    public int orderTime;
    public int orderValue;
    private bool _isSpecial;
    private bool _isSuperSpecial;
    private bool _isMusical;
    private Image image;
    [SerializeField] private int minimumOrderTime;
    // Colors
    private Color _originColor;
    private Color32 _specialColor;
    private Color32 _superSpecialOrder;
    private Color32 _textOriginColor;
    private Color32 _textCompletedColor;
    // Sounds
    [SerializeField] private AudioClip putItemInOrderSound;
    // Regular order events
    public UnityEvent<int,int> regularOrderFinish;
    public UnityEvent<int> regularOrderTimeOver;
    // Special order events
    public UnityEvent<int,int> specialOrderFinish;
    public UnityEvent<int> specialOrderTimeOver;
    // Super special order events
    public UnityEvent<int,int> superSpecialOrderFinish;
    public UnityEvent<int> superSpecialOrderTimeOver;

    void Awake()
    {
        _itemsImages = new List<Image>();
        _itemsTexts = new List<TextMeshProUGUI>();
        // Get child UI objects
        GameObject itemsObj = transform.GetChild(0).gameObject;
        _itemsImages.Add(itemsObj.transform.GetChild(0).gameObject.GetComponent<Image>());
        _itemsImages.Add(itemsObj.transform.GetChild(1).gameObject.GetComponent<Image>());
        _itemsImages.Add(itemsObj.transform.GetChild(2).gameObject.GetComponent<Image>());
        GameObject quantitiesObj = transform.GetChild(1).gameObject;
        _itemsTexts.Add(quantitiesObj.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>());
        _itemsTexts.Add(quantitiesObj.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>());
        _itemsTexts.Add(quantitiesObj.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>());
        _slider = transform.GetChild(2).GetComponent<Slider>();
        _sliderOriginColor = new Color32(30,255,0,255);
        _valueImageObject = transform.GetChild(4).gameObject;
        _valueImage = _valueImageObject.GetComponent<Image>();
        _specialValueObject = transform.GetChild(5).gameObject;
        _specialValueObject.SetActive(false);
        _specialValueImage = _specialValueObject.GetComponent<Image>();
        _valueTextOject = transform.GetChild(3).gameObject;
        _valueText = _valueTextOject.GetComponent<TextMeshProUGUI>();
        _musicEffect = transform.GetChild(6).gameObject.GetComponent<ParticleSystem>();

        ClearUI();

        _orderContent = new List<OrderItem>();
        _isSpecial = false;
        _isSuperSpecial = false;
        image = GetComponent<Image>();
        _originColor = image.color;
        _specialColor = new Color32(150,190,245,255);
        _superSpecialOrder = new Color32(250,200,200,255);
        _textOriginColor = new Color32(255,255,255,255);
        _textCompletedColor = new Color32(125,240,100,255);


    }

    void Start()
    {
        _gameManager = GameManager.instance;
    }

    void Update()
    {
        if(!_gameManager.isPause && !_gameManager.isGamOver)
        {
            if(IsComplete()) // Order complete
            {
                if(_isSpecial)
                    specialOrderFinish.Invoke(orderNumber, orderValue); 
                else if(_isSuperSpecial)
                    superSpecialOrderFinish.Invoke(orderNumber, orderValue); 
                else
                    regularOrderFinish.Invoke(orderNumber, orderValue); 
            }
            
            if(!_isMusical)
            {
                _slider.value -= Time.deltaTime;
                if(_slider.value <= 0) // Order run out of time
                {
                    if(_isSpecial)
                        specialOrderTimeOver.Invoke(orderNumber);  
                    else if(_isSuperSpecial)
                        superSpecialOrderTimeOver.Invoke(orderNumber); 
                    else
                        regularOrderTimeOver.Invoke(orderNumber);   
                }

                SliderFillerUpdate();
            }
        }
    }

    // Initialize order
    public void SetOrder(Dictionary<string,int> orderDict, Dictionary<string,Sprite> spriteDict, bool isSpecial, bool isSuperSpecial)
    {   
        _orderContent.Clear();
        ClearUI();
        int i=0;
        foreach(var item in orderDict)
        {
            // Set order content
            _orderContent.Add(new OrderItem(item.Key,item.Value));
            // Set sprite
            _itemsImages[i].enabled = true;
            _itemsImages[i].sprite = spriteDict[item.Key];
            // Set quantities text
            _itemsTexts[i].text = "0 / " + item.Value.ToString();
            i++;
        }

        _isSpecial = isSpecial;
        _isSuperSpecial = isSuperSpecial;

        SetOrderColor();

        CalculateOrderProperties();

        // Set slider
        _sliderFiller.color = _sliderOriginColor;
        _slider.maxValue = orderTime;
        _slider.value = _slider.maxValue;

        // Set value 
        if(_isSpecial || _isSuperSpecial)
        {
            _valueImageObject.SetActive(false);
            _valueTextOject.SetActive(false);
            _specialValueObject.SetActive(true);
            if(_isSpecial)
                _specialValueImage.sprite = _valueSprites[1];
            else
                _specialValueImage.sprite = _valueSprites[2];
        }
        else
        {
            _valueImageObject.SetActive(true);
            _valueTextOject.SetActive(true);
            _specialValueObject.SetActive(false);
            _valueText.text = orderValue.ToString();
            _valueImage.sprite = _valueSprites[0];
        }
    }

    // Try to add item to the order
    public bool TryPutItem(string itemName)
    {
        OrderItem item = GetItemByName(itemName);
        if(item == null) return false;
        if(item.currentAmount >= item.maxAmount) return false;
        item.currentAmount += 1;
        UpdateUI();
        AudioManager.instence.PlaySound(putItemInOrderSound);
        return true;
    }
    // Get item object by name
    private OrderItem GetItemByName(string name)
    {
        for(int i=0 ; i<_orderContent.Count ; i++)
        {
            if(_orderContent[i].name == name)
                return _orderContent[i];
        }
        return null;
    }

    private void UpdateUI()
    {
        for(int i=0 ; i<_orderContent.Count ; i++)
        {
            int current = _orderContent[i].currentAmount;
            int max = _orderContent[i].maxAmount;
            _itemsTexts[i].text = current.ToString() + " / " + max.ToString();
            if(current == max)
                _itemsTexts[i].color = _textCompletedColor;
        }
    }

    private void ClearUI()
    {
        for(int i=0; i<3 ; i++)
        {
            _itemsImages[i].enabled = false;
            _itemsTexts[i].text = "";
            _itemsTexts[i].color = _textOriginColor;
        }

        _valueText.text = "";
    }

    public bool IsComplete()
    {
        for(int i=0 ; i<_orderContent.Count ; i++)
        {
            if(_orderContent[i].currentAmount != _orderContent[i].maxAmount)
                return false;
        }
        return true;
    }

    private void CalculateOrderProperties()
    {
        int value = 0;
        int time = 0;
        Dictionary<string,int> cropValues = GameUtils.cropValues;
        Dictionary<string,int> cropTimes = GameUtils.cropGrowTime;
        foreach(OrderItem item in _orderContent)
        {
            value += cropValues[item.name] * item.maxAmount;
            time += cropTimes[item.name] * item.maxAmount;
        }
        orderValue = value;
        orderTime = (int)MathF.Max(minimumOrderTime,time);

        if(_isSpecial || _isSuperSpecial)
            orderValue = 0;
    }

    // Set order color according to order state
    private void SetOrderColor()
    {
        if(_isSpecial)
            image.color = _specialColor;
        else if(_isSuperSpecial)
            image.color = _superSpecialOrder;
        else
            image.color = _originColor;
    }

    private void SliderFillerUpdate()
    {
        if(_slider.value <= 0.35f * orderTime)
        {
            _sliderFiller.color = Color.red;
        }
    }

    public void ActiveMusic()
    {
        _isMusical = true;
        _musicEffect.Play();

        // Slider update
        _sliderFiller.color = _sliderOriginColor;
        _slider.value = _slider.maxValue;
    }
    public void DeactiveMusic()
    {
        _isMusical = false;
        _musicEffect.Stop();
    }
}
