using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SatteliteManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform _homePlanet;
    private List<Transform> _sattelites = new();
    public List<Transform> Sattelites => _sattelites;

    public void AddSatelite(Transform sattelite)
    {
        if (!_sattelites.Contains(sattelite))
        {
            _sattelites.Add(sattelite);
        }
    }

    public Transform GetClosestSatelite(Vector3 position)
    {
        Transform closest = null;
        float minDistance = float.MaxValue;
        foreach (var sattelite in _sattelites)
        {
            float distance = Vector3.Distance(position, sattelite.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = sattelite;
            }
        }
        if(closest == null) return _homePlanet;

        float distanceToHome = Vector3.Distance(position, _homePlanet.position);
        if (distanceToHome < minDistance)
        {
            closest = _homePlanet;
        }

        return closest;
    }
}
