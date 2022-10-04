using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMagnet : SwitchesInteractableObject
{
    [SerializeField] private AreaEffector2D positive;
    [SerializeField] private AreaEffector2D negative;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private interactionMode currentType;
    [SerializeField] private bool pole;
    [SerializeField] private bool isActive;
    private enum interactionMode{ onOff, invert }

    private void Start()
    {
        UpdateSprite();
    }
    public override void Activate()
    {
        switch ((int)currentType)
        {
            case 0:
                isActive = !isActive;
                positive.enabled = isActive;
                negative.enabled = isActive;
                break;
            case 1:
                pole = !pole;
                LayerMask temp = positive.colliderMask;
                positive.colliderMask = negative.colliderMask;
                negative.colliderMask = temp;
                break;
        }
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (pole)
        {
            if (isActive)
            {
                spriteRenderer.sprite = sprites[0];
            }
            else
            {
                spriteRenderer.sprite = sprites[1];
            }
        }
        else
        {
            if (isActive)
            {
                spriteRenderer.sprite = sprites[2];
            }
            else
            {
                spriteRenderer.sprite = sprites[3];
            }
        }
       
    }
}
