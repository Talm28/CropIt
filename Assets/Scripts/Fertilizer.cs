using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fertilizer : DirtTool
{
    [SerializeField] private GameObject[] dirtPlots;
    [SerializeField] private AudioClip achiveSound;
    [SerializeField] private AudioClip useItemSound;

    SpriteRenderer spriteRenderer;

    private ParticleSystem _effect;

    public bool isAvailable;

    private Color _originColor;
    private Color _fadedColor;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _originColor = spriteRenderer.color;
        _fadedColor = _originColor;
        _fadedColor.a = 0.25f;
        _effect = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        Deactive();
    }

    protected override void ToolFunctuality(DirtPlot dirtScript)
    {
        if(isAvailable)
        {
            AudioManager.instence.PlaySound(useItemSound);
            Deactive();
            foreach(GameObject dirt in dirtPlots)
            {
                dirtScript = dirt.GetComponent<DirtPlot>();
                if(dirtScript.isAvailable)
                {
                    dirtScript.FertilizeDirt();
                }   
            }
        }
    }

    public void Active()
    {
        isAvailable = true;
        spriteRenderer.color = _originColor;
        _effect.Play();
        AudioManager.instence.PlaySound(achiveSound);
    }

    public void Deactive()
    {
        isAvailable = false;
        spriteRenderer.color = _fadedColor;
    }
}
