using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;

public class Plant : MonoBehaviour
{
    // Sprites
    [SerializeField] private List<Sprite> _sprites;
    private int _currentSpriteIndex;
    private SpriteRenderer _spriteRenderer;
    // Game manager
    private GameManager _gameManager;
    // Clock
    private int _clock;
    [SerializeField] private TextMeshProUGUI clockText;
    // Gorw
    private int _growTime;
    public bool _isReady;
    public bool isFertilized;
    private int _rottTime;
    [SerializeField] private int rottInitialValue;
    //Sounds
    [SerializeField] private AudioClip gorwSound;

    private bool _isPlanned;
    [SerializeField] public string plantName;

    private ParticleSystem leafParticles;

    // Start is called before the first frame update
    void Awake()
    {
        _currentSpriteIndex = 0;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        clockText.gameObject.SetActive(false);
        _growTime = GameUtils.cropGrowTime[plantName];
        _clock = _growTime;
        _isPlanned = false;
        _isReady = false;
        isFertilized = false;
        _growTime = GameUtils.cropGrowTime[plantName];
        _rottTime = rottInitialValue;

        _gameManager = GameManager.instance;

        leafParticles = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if(!_gameManager.isPause && !_gameManager.isGamOver)
        {
            GrowUpdate();
        }
    }

    public void ActivePlant()
    {
        _gameManager.clockTick.AddListener(PlantTick);
        clockText.gameObject.SetActive(true);
        if(_clock > 0)
            clockText.text = _clock.ToString();
        else if(_isReady && _rottTime <= 5)
            clockText.text = _rottTime.ToString();
        else
            clockText.text = "";
            
        if(!_isPlanned)
        {
            _isPlanned = true;
            _spriteRenderer.sprite = _sprites[_currentSpriteIndex];
        }
    }
    public void DeactivePlant()
    {
        _gameManager.clockTick.RemoveListener(PlantTick);
        DeFertilizePlant();
        clockText.gameObject.SetActive(false);
    }

    public void UpdateSprite(int index)
    {
        if(index == _currentSpriteIndex) return;
        if(index <= _sprites.Count)
        {
            _currentSpriteIndex = index;
            _spriteRenderer.sprite = _sprites[_currentSpriteIndex];
        }
    }

    void PlantTick()
    {
        if(_clock > 0) // Regular clock
        {
            _clock -= 1;
            clockText.text = _clock.ToString();
            if(_clock == 0)
                clockText.text = "";
        }

        if(_isReady) // Rott clock
        {
            if(isFertilized)
            {
                _rottTime = rottInitialValue;
                return;
            }
            _rottTime -= 1;
            if(_rottTime <= 5)
            {
                clockText.text = _rottTime.ToString();
                clockText.color = Color.red;
                if(_rottTime == 0)
                    clockText.text = "";
            }
            
        }
    }

    private void GrowUpdate()
    {
        if(_rottTime == 0)
        {
            UpdateSprite(4);
            _isReady = false;
        }
        else if(_clock == 0)
        {
            UpdateSprite(3);
            if(!_isReady)
            {
                leafParticles.Play();
                AudioManager.instence.PlaySound(gorwSound);
            }
            _isReady = true;
            
        }
        else if (_clock <= 0.33 * _growTime)
            UpdateSprite(2);
        else if (_clock <= 0.66 * _growTime)
            UpdateSprite(1);
    }

    public void FertilizePlant()
    {
        if(!isFertilized)
        {
            isFertilized = true;
            _gameManager.halfClockTick.AddListener(PlantTick);
        }
    }

    public void DeFertilizePlant()
    {
        if(isFertilized)
        {
            isFertilized = false;
            _gameManager.halfClockTick.RemoveListener(PlantTick);
        }
    }
}
