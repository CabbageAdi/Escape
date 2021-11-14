using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cabbage.Helpers;

public class CameraMove : MonoBehaviour
{
    public GameObject[] Regions;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var region = CalculatePlayerPosition();
        if (region != null)
            this.transform.position = region.transform.position + new Vector3(0, 0, -10);
        else
            this.transform.position = Player.transform.position + new Vector3(0, 0, -10);
    }

    private GameObject CalculatePlayerPosition()
    {
        var position = (Vector2)Player.transform.position;

        foreach (var region in Regions)
        {
            var bounding = new Vector2[2];
            var difference = new Vector2(region.transform.localScale.x / 2, region.transform.localScale.y / 2);
            bounding[0] = (Vector2)region.transform.position - difference;
            bounding[1] = (Vector2)region.transform.position + difference;

            if (position.IsMoreThan(bounding[0]) && position.IsLessThan(bounding[1]))
                return region;
        }

        return null;
    }
}
