using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class FlowerGrowthSystem : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRenderer;

    //List of sprites representing different growth stages
    [SerializeField] Sprite sprite1;
    [SerializeField] Sprite sprite2;
    [SerializeField] Sprite sprite3;

    //Value when sprites should change
    [SerializeField] float stage1Threshold;
    [SerializeField] float stage2Threshold;

    //Speed at which the plant should grow
    [SerializeField] private float growthRate;
    [SerializeField] private float growthStage;

    private bool isGrown;
    
    private void Awake()
    {
        growthStage = 0;
        isGrown = false;
    }
    private void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

   private void Update()
    {
        GrowthUpdate();
    }

    //Change sprite depending on growth stage
    private void GrowthUpdate()
    {
        growthStage += growthRate * Time.deltaTime;

        if (growthStage < stage1Threshold)
        {
            spriteRenderer.sprite = sprite1;
        }
        else if (growthStage < stage2Threshold)
        {
            spriteRenderer.sprite = sprite2;
        }
        else
        {
            spriteRenderer.sprite = sprite3;
            isGrown = true;

        }
    }

    public bool GetGrowth()
    {
        if (isGrown)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
