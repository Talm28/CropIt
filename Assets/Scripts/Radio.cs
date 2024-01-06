using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Radio : OrderTool
{
    public OrdersManager orderManager;
    [SerializeField] private AudioClip achiveSound;
    [SerializeField] private AudioClip useItemSound;

    private ParticleSystem _effect;
    
    private bool _isAvailable;
    private int quantity;
    // sprite variables
    private SpriteRenderer _spriteRenderer;
    private Color _originColor;
    private Color _fadedColor;
    // UI
    private TextMeshProUGUI quantityText;

    void Start()
    {
        _isAvailable = false;
        quantity = 0;

        quantityText = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        quantityText.text = "";
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originColor = _spriteRenderer.color;
        _fadedColor = _originColor;
        _fadedColor.a = 0.25f;
        _spriteRenderer.color = _fadedColor;

        _effect = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
    }

    protected override void ToolFunctuality(Order dirtScript)
    {
        if(_isAvailable)
        {
            orderManager.ActiveRadio();
            AudioManager.instence.PlaySound(useItemSound);
            RemoveQuantity();
        }
        
    }

    public void AddQuantity()
    {
        if(quantity == 0)
        {
            _isAvailable = true;
            _spriteRenderer.color = _originColor;
        }
        
        quantity += 1;
        _effect.Play();
        AudioManager.instence.PlaySound(achiveSound);

        // Text Update
        if(quantity >=2)
            quantityText.text = quantity.ToSafeString();
    }

    public void RemoveQuantity()
    {
        if(quantity == 0) return;
        if(quantity == 1)
        {
            _isAvailable = false;
            _spriteRenderer.color = _fadedColor;
        }
        quantity -= 1;

        // Text Update
        if(quantity <= 1)
            quantityText.text = "";
        else
            quantityText.text = quantity.ToSafeString();
    }
}
