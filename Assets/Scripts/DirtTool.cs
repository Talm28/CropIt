using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtTool : DragAndDrop
{
    private Vector3 _originPosition;
    [SerializeField] private List<GameObject> dirtsColiders;

    void Awake() {
        _originPosition = transform.position;
        dirtsColiders = new List<GameObject>();
    }


    protected override void StopDrag()
    {
        // Reverse to original position
        transform.position = _originPosition;
        // Clear dirt grass if plot available
        if(dirtsColiders.Count > 0)
        {
            GameObject dirt = GetClosestObject(dirtsColiders);
            DirtPlot dirtScript = dirt.GetComponent<DirtPlot>();
            if(dirtScript.isAvailable)
            {
                ToolFunctuality(dirtScript);
            }
        }
    }

    protected virtual void ToolFunctuality(DirtPlot dirtScript){}

    // Dirt plot detact
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Dirt plot" )
            dirtsColiders.Add(other.gameObject);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Dirt plot")
            dirtsColiders.Remove(other.gameObject);
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
