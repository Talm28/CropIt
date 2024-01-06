using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtPlot : MonoBehaviour
{
    private GameManager _gameManager;
    private GameObject _currentItem;

    public bool isAvailable;
    public bool hasGrass;
    public bool isFertilize;
    [SerializeField] private int grassRandom;
    [SerializeField] private int buyPrice;
    [SerializeField] private Sprite[] sprites;
    
    SpriteRenderer spriteRenderer;
    GameObject fertilizeEffect;

    private int fertilizeClock;
    public int fertilizeDuration;

    System.Random random;

    void Awake() {
        _currentItem = null;
        random = new System.Random();
        fertilizeEffect = transform.GetChild(0).gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(!isAvailable)
        {
            spriteRenderer.sprite = sprites[1];
        }
        hasGrass = false;
    }

    void Start()
    {
        _gameManager = GameManager.instance;
        _gameManager.clockTick.AddListener(FertilizeClockUpdate);
    }

    void Update()
    {

    }

    public GameObject GetItem()
    {
        return _currentItem;
    }

    public bool IsEmpty()
    {
        if(_currentItem == null && !hasGrass) return true;
        return false;
    }

    public void SetItem(GameObject item)
    {
        _currentItem = item;
        if(item != null && isFertilize)
            item.GetComponent<Plant>().FertilizePlant();
    }

    public void RandomGrass()
    {
        if(isFertilize) return; // Fertilized grass doesnt have grass

        int i = random.Next(grassRandom);
        if(i == 0)
        {
            spriteRenderer.sprite = sprites[2];
            hasGrass = true;
        }
    }

    public void ClearGrass()
    {
        spriteRenderer.sprite = sprites[0];
        hasGrass = false;
    }

    public void PurchaseDirt()
    {
        spriteRenderer.sprite = sprites[0];
        isAvailable = true;
    }

    void OnMouseDown()
    {
        if(!isAvailable && _gameManager.money >= buyPrice)
        {
            _gameManager.TakeMoney(buyPrice);
            PurchaseDirt();
        }
    }

    public void FertilizeDirt()
    {
        isFertilize = true;
        spriteRenderer.sprite = sprites[3];
        fertilizeEffect.SetActive(true);
        fertilizeClock = fertilizeDuration;

        if(_currentItem != null)
            _currentItem.GetComponent<Plant>().FertilizePlant();
    }

    public void DeFertilizeDirt()
    {
        isFertilize = false;
        spriteRenderer.sprite = sprites[0];
        fertilizeEffect.SetActive(false);

        if(_currentItem != null)
            _currentItem.GetComponent<Plant>().DeFertilizePlant();
    }

    public void FertilizeClockUpdate()
    {
        if(isFertilize)
        {
            fertilizeClock -= 1;
            if(fertilizeClock <= 0 )
            {
                DeFertilizeDirt();
            }
        } 
    }
}
