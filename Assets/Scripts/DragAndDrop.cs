using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 _dragOffset;
    private bool _isDragged;
    private bool _isJustSpawn; // for the drag action when the object is just spawn

    void Awake()
    {
        _isDragged = false;
        _isJustSpawn = false;
    }

    void Update()
    {
        DragUpdate();
    }

    protected void DragUpdate()
    {
        if(_isDragged)
        {
            transform.position = GetMousePos() + _dragOffset;

            if(_isJustSpawn) // Stopping the drag when the object was spawn
            {
                if(Input.GetMouseButtonUp(0))
                    DeactiveDrag();  
            }
        }
    }

    // Get offset position on click
    void OnMouseDown()
    {
        _dragOffset = transform.position - GetMousePos();
        _isDragged = true;
        StartDarg();
    }
    // End drag
    void OnMouseUp()
    {
        _isDragged = false;
        StopDrag();
    }
    // Get real mouse position
    private Vector3 GetMousePos()
    {
        Vector3 mousePos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
    
    // Manually start and end drag action
    public void ActiveDrag()
    {
        OnMouseDown();
        _isJustSpawn = true;
    }
    public void DeactiveDrag()
    {
        OnMouseUp();
        _isJustSpawn = false;
    }

    // Function for add functionality to inheret object
    virtual protected void StopDrag(){}
    virtual protected void StartDarg(){}

}
