using System.Collections.Generic;
using System.Linq;
using Cabbage.Helpers;
using UnityEngine;

public class PowerCalculation : MonoBehaviour
{
    public Dictionary<PowerBlock, Vector2[]> Endpoints { get; set; } = new Dictionary<PowerBlock, Vector2[]>();
    
    private PowerBlock[] _powerBlocks;
    private Gate[] _gates;


    private bool _setAll = false;
    
    public void Start()
    {
        _gates = FindObjectsOfType<Gate>();
        _powerBlocks = FindObjectsOfType<PowerBlock>();
        foreach (var block in _powerBlocks)
        {
            CalculateEndpoints(block);
            
            //initial power calculation
            foreach (var powerBlock in _powerBlocks)
            {
                powerBlock.Powered = false;
            }
            foreach (var endpoint in Endpoints.Where(endpoint => endpoint.Key.IsSource))
            {
                CalculateAdjacentPower(endpoint);
                CalculateGates();
            }
            _setAll = true;
        }
    }

    public void CalculateEndpoints(PowerBlock block)
    {
        var endpoints = block.Endpoints;
        var rotation = block.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;

        var finalEndpoints = endpoints.Select(e =>
            new Vector2(e.x * Mathf.Cos(rotation) - e.y * Mathf.Sin(rotation),
                e.x * Mathf.Sin(rotation) + e.y * Mathf.Cos(rotation))).ToArray();

        finalEndpoints = finalEndpoints.Select(e => (Vector2)block.transform.position + (e / 2)).ToArray();

        Endpoints[block] = finalEndpoints;

        if (_setAll)
        {
            foreach (var powerBlock in _powerBlocks)
            {
                powerBlock.Powered = false;
            }
            foreach (var endpoint in Endpoints.Where(endpoint => endpoint.Key.IsSource))
            {
                CalculateAdjacentPower(endpoint);
            }
            CalculateGates();
        }
    }
    
    private void CalculateAdjacentPower(KeyValuePair<PowerBlock, Vector2[]> block)
    {
        block.Key.Powered = true;

        foreach (var allBlocks in Endpoints)
        {
            if (allBlocks.Key.Powered) continue;
            
            if (allBlocks.Value.Any(endpoint =>
                block.Value.Any(poweredBlockEndpoint => endpoint == poweredBlockEndpoint)))
            {
                CalculateAdjacentPower(allBlocks);
            }
        }
    }

    private void CalculateGates()
    {
        foreach (var gate in _gates)
        {
            Vector2[] positions = null;
            if (gate.transform.rotation.eulerAngles.z == 0)
            {
                positions = new Vector2[]
                {
                    gate.transform.position + new Vector3(0.5f, 0),
                    gate.transform.position + new Vector3(-0.5f, 0)
                };
            }
            else if (gate.transform.rotation.eulerAngles.z == 90)
            {
                positions = new Vector2[]
                {
                    gate.transform.position + new Vector3(0, 0.5f),
                    gate.transform.position + new Vector3(0, -0.5f)
                };
            }

            gate.Open = false;
            foreach (var position in positions)
            {
                if (Endpoints.Any(end =>
                    end.Key.Powered && end.Value.Any(v2 => v2 == position)))
                {
                    gate.Open = true;
                    break;
                }
            }
        }
    }
}