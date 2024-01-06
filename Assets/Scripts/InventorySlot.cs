using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : ObjectSpawner
{
    private Image _itemChildImage;
    private bool _isAvailable;


    void Awake()
    {
        _isAvailable = false;

        _itemChildImage = transform.GetChild(0).gameObject.GetComponent<Image>();

        DeActiveSlot();

    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(_isAvailable)
        {
            GameObject obj = SpawnItem();
            ItemDragAndDrop objScript = obj.GetComponent<ItemDragAndDrop>();
            if(objScript != null) objScript.itemSpawner = this;
        } 
    }

    public void ActiveSlot()
    {
        _isAvailable = true;
        Color tempColor = _itemChildImage.color;
        tempColor.a = 1f;
        _itemChildImage.color = tempColor;
    }

    public void DeActiveSlot()
    {
        _isAvailable = false;
        Color tempColor = _itemChildImage.color;
        tempColor.a = 0.5f;
        _itemChildImage.color = tempColor;
    }

}
