using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialCropsController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] cropsTimesTexts;
    [SerializeField] private TextMeshProUGUI[] cropsValuesTexts;

    // Start is called before the first frame update
    void Start()
    {
        Dictionary<string,int> cropGrowTime = GameUtils.cropGrowTime;
        Dictionary<string,int> cropValues = GameUtils.cropValues;
        // Update time texts
        int i=0;
        foreach( KeyValuePair<string,int> crop in cropGrowTime)
        {
            cropsTimesTexts[i].text = crop.Value.ToString();
            i++;
        }
        // Update values texts
        i=0;
        foreach( KeyValuePair<string,int> crop in cropValues)
        {
            cropsValuesTexts[i].text = crop.Value.ToString();
            i++;
        }
    }
}
