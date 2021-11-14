using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBlock : MonoBehaviour
{
    public Vector2[] Endpoints;

    public Sprite UnPoweredSprite;
    public Sprite PoweredSprite;

    private SpriteRenderer _spriteRenderer;
    
    public bool Powered { get; set; }

    public bool IsSource;

    public void Start()
    {
        this._spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        if (Powered && _spriteRenderer.sprite == UnPoweredSprite)
            _spriteRenderer.sprite = PoweredSprite;

        if (!Powered && _spriteRenderer.sprite == PoweredSprite)
            _spriteRenderer.sprite = UnPoweredSprite;
    }

    public void Rotate()
    {
        this.transform.Rotate(0, 0, 90);
    }
}
