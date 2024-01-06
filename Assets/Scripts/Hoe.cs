using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoe : DirtTool
{
    //Sounds
    [SerializeField] private AudioClip hoeSound;

    protected override void ToolFunctuality(DirtPlot dirtScript)
    {
        dirtScript.ClearGrass();
        AudioManager.instence.PlaySound(hoeSound);
    }
}
