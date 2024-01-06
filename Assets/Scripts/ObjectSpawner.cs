using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectSpawner : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] public GameObject objectToSpawn;

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        SpawnItem();
    }

    protected GameObject SpawnItem()
    {
        // Get mouse position
        Vector3 mousePos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // Spawn an object and activate his drag
        GameObject obj = Instantiate(objectToSpawn, mousePos, Quaternion.identity);
        DragAndDrop objScript = obj.GetComponent<DragAndDrop>();
        if(objScript != null)
            objScript.ActiveDrag();
        return obj;
    }
}
