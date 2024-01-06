using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // Singelton
    public static GameManager instance;
    // Game clock
    public int globalClock;
    public float clock;
    private bool isHalfed;
    [SerializeField] private int _tick;
    // Public game variables
    [SerializeField] public int money;
    [SerializeField] public int health;
    public int ordersCompleted;
    public int profits;
    public int gameStage;
    public bool isPause;
    public bool isGamOver;
    public GameObject[] inventorySlots;
    // Events
    public UnityEvent clockTick;
    public UnityEvent halfClockTick;
    public UnityEvent gameOver;
    // Objects
    [SerializeField] Radio radio;
    [SerializeField] OrdersManager ordersManager;
    // Sounds
    [SerializeField] private AudioClip loseHealthSound;

    void Awake()
    {
        // Singleton stuff...
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);

        isPause = false;
        isGamOver = false;
        globalClock = 0;
        clock = 0;
        money = 0;
        gameStage = 1;
        health = 3;
        isHalfed = false;
        ordersCompleted = 0;
        profits = 0;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        inventorySlots[0].GetComponent<InventorySlot>().ActiveSlot();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateClock();
    }

    private void UpdateClock()
    {
        clock += Time.deltaTime;
        if(!isHalfed && clock >= _tick * 0.5)
        {
            isHalfed = true;
            halfClockTick.Invoke();
        }
        if(clock >= _tick)
        {
            globalClock += 1;
            clock = 0;
            isHalfed = false;
            clockTick.Invoke();
        }
    }

    public void TakeMoney(int amount)
    {
        if(money >= amount)
            money -= amount;
    }

    public void GiveMoney(int amount)
    {
        money += amount;
    }

    public void IncreaseGameStage()
    {
        inventorySlots[gameStage].GetComponent<InventorySlot>().ActiveSlot();
        gameStage += 1;
        if(gameStage == 3 || gameStage == 5 || gameStage == 7)
            radio.AddQuantity();
        ordersManager.ResetOrderTimerBound(); // Reset min and max tick for new orders
    }

    public void TakeLife(int i)
    {
        if(health > 0)
        {
            health --;
        }

        if(health <= 0)
            gameOver.Invoke();
        else
            AudioManager.instence.PlaySound(loseHealthSound);
    }

    public void GiveLife()
    {
        if(health < 3)
            health ++;
    }

    public void GameOver()
    {
        isGamOver = true;
        Time.timeScale = 0f;
    }

    public void PauseGame()
    {
        isPause = true;
        Time.timeScale = 0f;
    }

    public void ContinueGame()
    {
        isPause = false;
        Time.timeScale = 1f;
    }
}
