using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDragAndDrop : DragAndDrop
{
    [SerializeField] private AudioClip plantingSound;

    private bool _isTaken;
    // Dirt plots
    [SerializeField] private List<GameObject> dirtsColiders;
    GameObject currentDirtPlot;
    // Orders
    [SerializeField] private List<GameObject> orders;

    public InventorySlot itemSpawner;

    private Plant _itemPlantScript;

    void Awake()
    {
        _isTaken = false;
        dirtsColiders = new List<GameObject>();
        currentDirtPlot = null;
        _itemPlantScript = GetComponent<Plant>();
    }
    
    // Added functionality when start and stop the drag
    override protected void StopDrag()
    {
        if(orders.Count > 0) // Put the item in order
        {
            GameObject order = GetClosestObject(orders);
            Order orderScript = order.GetComponent<Order>();
            if(_itemPlantScript._isReady && orderScript.TryPutItem(_itemPlantScript.plantName))
            {
                currentDirtPlot.GetComponent<DirtPlot>().RandomGrass(); // Set grass
                Destroy(this.gameObject);
                return;
            }
            else // If order didnt accept the item
            {
                if(currentDirtPlot != null) // try to return to dirt
                {
                    SetItemPosition(currentDirtPlot.transform.position);
                    _itemPlantScript.ActivePlant();
                } 
                else
                    Destroy(this.gameObject);   
            }
        }
        else if(dirtsColiders.Count > 0) // Put the item in dirt and update amount
        {
            // Get closest dirt plot
            GameObject newDirtPlot = GetClosestObject(dirtsColiders);
            if(newDirtPlot.GetComponent<DirtPlot>().IsEmpty())
            {
                if(currentDirtPlot != null) // Has old dirt plot
                {
                    currentDirtPlot.GetComponent<DirtPlot>().SetItem(null);
                    currentDirtPlot.GetComponent<DirtPlot>().RandomGrass(); // Set grass
                }
                // Update current dirt plot
                currentDirtPlot = newDirtPlot;
                SetItemPosition(currentDirtPlot.transform.position);
                currentDirtPlot.GetComponent<DirtPlot>().SetItem(this.gameObject);
            }
            else
            {
                if(currentDirtPlot == null) // If the first dirt is not empty
                {
                    Destroy(this.gameObject);
                    return;
                } 
                SetItemPosition(currentDirtPlot.transform.position); // Return to the old dirt
            }
            // Update spawner count
            if(itemSpawner != null && !_isTaken) 
            {
                _isTaken = true;
            }
            // Update dirt slot item
            currentDirtPlot.GetComponent<DirtPlot>().SetItem(this.gameObject);
            // Active plant
            _itemPlantScript.ActivePlant();

            AudioManager.instence.PlaySound(plantingSound);
        }
        else // Destroy the item if not set on dirt or order
        {
            if(currentDirtPlot != null) // Has old dirt plot
                currentDirtPlot.GetComponent<DirtPlot>().RandomGrass(); // Set grass
            Destroy(this.gameObject);
        }
    }
    override protected void StartDarg()
    {
        // Deactive plant
        _itemPlantScript.DeactivePlant();
    }
    

    // Dirt plot detact
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Dirt plot" && other.gameObject.GetComponent<DirtPlot>().isAvailable)
            dirtsColiders.Add(other.gameObject);
        else if(other.tag == "Order")
            orders.Add(other.gameObject);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Dirt plot")
            dirtsColiders.Remove(other.gameObject);
        else if(other.tag == "Order")
            orders.Remove(other.gameObject);
    }

    private GameObject GetClosestObject(List<GameObject> objects)
    {
        GameObject res = null;
        float minDist = Mathf.Infinity;
        foreach(GameObject obj in objects)
        {
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if(dist <= minDist)
            {
                minDist = dist;
                res = obj;
            }
        }
        return res;
    }

    private void SetItemPosition(Vector3 position)
    {
        Vector3 newPos = position;
        newPos.z = 0;
        transform.position = newPos;
    }
}
