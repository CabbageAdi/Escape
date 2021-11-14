using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI CollectableText;

    private Tilemap _tilemap;
    private int _totalOrbCount;
    private int _orbCount;

    public void Start()
    {
        _orbCount = 0;
        _tilemap = FindObjectOfType<Tilemap>();
        _totalOrbCount = _tilemap.GetTilesBlock(_tilemap.cellBounds).Count(t => t.name == "orb");
        CollectableText.text = $"0 / {_totalOrbCount}";
    }
    
    public void AddCollectable()
    {
        _orbCount++;
        CollectableText.text = $"{_orbCount} / {_totalOrbCount}";
    }
}