using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Sprite OpenSprite;
    public Sprite ClosedSprite;

    public bool Open;

    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if (Open && _spriteRenderer.sprite == ClosedSprite)
            _spriteRenderer.sprite = OpenSprite;

        if (!Open && _spriteRenderer.sprite == OpenSprite)
            _spriteRenderer.sprite = ClosedSprite;
    }
}
