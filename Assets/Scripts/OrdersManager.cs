using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

// Order repsentation class
public class OrderObject
{
    public GameObject obj;
    public bool isActive;
    public OrderObject(GameObject order)
    {
        this.obj = order;
        this.isActive = false;
    }
    public void Active()
    {
        obj.SetActive(true);
        isActive = true;
    }
    public void DeActive()
    {
        obj.SetActive(false);
        isActive = false;
    }
}

public class OrdersManager : MonoBehaviour
{
    private GameManager _gameManager;
    private System.Random random;
    // Public onjects references
    public GameObject fertilizer;
    public GameObject coinsEffect;
    // Audio clips
    [SerializeField] private AudioClip cashSound;
    [SerializeField] private AudioClip orderAriveSound;
    // Crops variables
    private Dictionary<string,Sprite> itemsSprites;
    private List<string> itemsName;
    [SerializeField] private List<Sprite> sprites;
    // Orders variables
    public List<OrderObject> orders;
    public int activeOrdersNum;
    private bool _isAutoOrder;
    // Order Clock
    [SerializeField] private int minClockTick;
    [SerializeField] private int maxClockTick;
    [SerializeField] private int amountForReduceOrderTick;
    private int currentMinCloclTick;
    private int currentMaxClockTick;
    private int tickToClock;
    private int clock;
    private int reduceOrderTickClock;
    [SerializeField] private int specialOrderChance;
    [SerializeField] private int superSpecialOrderChance;
    // Radio item
    [SerializeField] private int radioTime;
    private bool _isRadio;
    private int radioTimer;
    

    void Awake()
    {
        orders = new List<OrderObject>();
        orders.Add(new OrderObject(transform.GetChild(0).gameObject));
        orders.Add(new OrderObject(transform.GetChild(1).gameObject));
        orders.Add(new OrderObject(transform.GetChild(2).gameObject));
        orders.Add(new OrderObject(transform.GetChild(3).gameObject));
        orders.Add(new OrderObject(transform.GetChild(4).gameObject));

        itemsSprites = new Dictionary<string, Sprite>();
        itemsSprites.Add("Cabbage", sprites[0]);
        itemsSprites.Add("Carrot", sprites[1]);
        itemsSprites.Add("Corn", sprites[2]);
        itemsSprites.Add("Cucumber", sprites[3]);
        itemsSprites.Add("Onion", sprites[4]);
        itemsSprites.Add("Potato", sprites[5]);
        itemsSprites.Add("Radish", sprites[6]);
        itemsSprites.Add("Tomato", sprites[7]);
        
        itemsName = new List<string>{"Cabbage", "Carrot", "Corn", "Cucumber", "Onion", "Potato", "Radish", "Tomato"};
        
        random = new System.Random();

        tickToClock = 5;

        activeOrdersNum = 0;
        _isAutoOrder = true;

        _isRadio = false;

        ResetOrderTimerBound();
            
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.instance;

        _gameManager.clockTick.AddListener(UpdateClock);

        // Set orders events
        for(int i=0 ; i<orders.Count ; i++)
        {
            // Regular order events
            orders[i].obj.GetComponent<Order>().regularOrderFinish.AddListener(CompleteRegularOrder);
            orders[i].obj.GetComponent<Order>().regularOrderTimeOver.AddListener(OrderTimeOver);
            orders[i].obj.GetComponent<Order>().regularOrderTimeOver.AddListener(_gameManager.TakeLife);
            // Special order events
            orders[i].obj.GetComponent<Order>().specialOrderFinish.AddListener(CompleteSpacialOrder);
            orders[i].obj.GetComponent<Order>().specialOrderTimeOver.AddListener(OrderTimeOver);
            // Super special events
            orders[i].obj.GetComponent<Order>().superSpecialOrderFinish.AddListener(CompleteSuperSpacialOrder);
            orders[i].obj.GetComponent<Order>().superSpecialOrderTimeOver.AddListener(OrderTimeOver);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!_gameManager.isPause && !_gameManager.isGamOver)
        {
            ClockUpdate();

            if(activeOrdersNum == 0 && !_isAutoOrder)
            {
                _isAutoOrder = true;
                clock = tickToClock - 2;
            }
        }
    }

    public void UpdateClock()
    {
        clock += 1;
        reduceOrderTickClock += 1;
    }

    private void ClockUpdate()
    {  
        if(clock >= tickToClock) // Order Clock tick
        {
            _isAutoOrder = false;
            clock = 0;
            tickToClock = random.Next(currentMinCloclTick, currentMaxClockTick + 1);
            AddOrder();
        }

        if(reduceOrderTickClock >= amountForReduceOrderTick)
        {
            reduceOrderTickClock = 0;
            if(currentMaxClockTick > currentMinCloclTick)
                currentMaxClockTick -= 1;
        }

        if(_isRadio) // check for music finish
        {
            if(radioTimer <= 0)
                DeactiveMusic();
        }
    }

    public void ResetOrderTimerBound()
    {
        currentMaxClockTick = maxClockTick;
        currentMinCloclTick = minClockTick;
    }

    public void AddOrder()
    {
        int index = GetOrderIndex();
        if(index == -1) return;

        bool isSpecial = false;
        bool isSuperSpecial = false;
        Dictionary<string,int> order;

        if(random.Next(0, specialOrderChance) == 0 && !fertilizer.GetComponent<Fertilizer>().isAvailable) // Special order
        {
            isSpecial = true;
            order = GameUtils.GetSpecialOrder();
        }
        else if(random.Next(0, superSpecialOrderChance) == 0 && _gameManager.health < 3) // Super special order
        {
            isSuperSpecial = true;
            order = GameUtils.GetSuperSpecialOrder();
        }
        else // Regular order
        {
            order = GameUtils.GetOrder();
        }

        orders[index].Active();
        orders[index].obj.GetComponent<Order>().SetOrder(order, itemsSprites, isSpecial, isSuperSpecial);

        AudioManager.instence.PlaySound(orderAriveSound);
        
        if(_isRadio)
            orders[index].obj.GetComponent<Order>().ActiveMusic();
        else
            orders[index].obj.GetComponent<Order>().DeactiveMusic();

        activeOrdersNum ++;
    }
    private int GetOrderIndex()
    {
        for(int i=0; i<orders.Count ; i++)
        {
            if(orders[i].isActive == false)
                return i;
        }
        return -1;
    }

    // Regular order
    public void CompleteRegularOrder(int orderNum, int orderValue)
    {
        orders[orderNum].DeActive();
        _gameManager.GiveMoney(orderValue);
        _gameManager.profits += orderValue;
        _gameManager.ordersCompleted += 1;
        activeOrdersNum --;
        AudioManager.instence.PlaySound(cashSound);
        StartCoroutine(StartCoinsEffect(orders[orderNum].obj.transform.position));
    }
    public void OrderTimeOver(int orderNum)
    {
        orders[orderNum].DeActive();
        activeOrdersNum --;
    }

    // Special order
    public void CompleteSpacialOrder(int orderNum, int orderValue)
    {
        orders[orderNum].DeActive();
        activeOrdersNum --;
        fertilizer.GetComponent<Fertilizer>().Active();
    }

    // Super special order
    public void CompleteSuperSpacialOrder(int orderNum, int orderValue)
    {
        orders[orderNum].DeActive();
        activeOrdersNum --;
        _gameManager.GiveLife();
    }

    IEnumerator StartCoinsEffect(Vector3 pos)
    {
        Vector2 mousePosition = GameUtils.GetyMousePos();
        GameObject effect = Instantiate(coinsEffect, mousePosition, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Destroy(effect);
    }

    // Radio functions
    public void ActiveRadio()
    {
        _isRadio = true;
        radioTimer = radioTime;
        _gameManager.clockTick.AddListener(RadioTimerUpdate);

        foreach(OrderObject order in orders)
        {
            if(order.isActive)
                order.obj.GetComponent<Order>().ActiveMusic();
        }
    }
    public void DeactiveMusic()
    {
        _isRadio = false;
        _gameManager.clockTick.RemoveListener(RadioTimerUpdate);

        foreach(OrderObject order in orders)
        {
            if(order.isActive)
                order.obj.GetComponent<Order>().DeactiveMusic();
        }
            
    }
    private void RadioTimerUpdate()
    {
        radioTimer -= 1;
    }
}
