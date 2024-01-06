using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderTool : DragAndDrop
{
    private Vector3 _originPosition;
    [SerializeField] private List<GameObject> ordersColiders;

    void Awake() {
        _originPosition = transform.position;
        ordersColiders = new List<GameObject>();
    }


    protected override void StopDrag()
    {
        // Reverse to original position
        transform.position = _originPosition;
        // Clear dirt grass if plot available
        if(ordersColiders.Count > 0)
        {
            GameObject dirt = GetClosestObject(ordersColiders);
            Order orderScript = dirt.GetComponent<Order>();
            ToolFunctuality(orderScript);
        }
    }

    protected virtual void ToolFunctuality(Order dirtScript){}

    // Dirt plot detact
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Order" )
            ordersColiders.Add(other.gameObject);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Order")
            ordersColiders.Remove(other.gameObject);
    }
    // Get closest object in list
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
}
