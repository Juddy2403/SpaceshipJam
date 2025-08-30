using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SatteliteManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform _homePlanet;
    [SerializeField] private int _resourceNum;
    private int _currentResourceNum;
    private List<Transform> _sattelites = new();
    public List<Transform> Sattelites => _sattelites;
    public Action OnGameFinish;

    public void AddSatelite(Transform sattelite)
    {
        _sattelites.Add(sattelite);
        AddCurrResourceNum();
    }

    public void AddCurrResourceNum()
    { 
        ++_currentResourceNum;
        if (_currentResourceNum >= _resourceNum)
        {
            OnGameFinish.Invoke();
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
        if (closest == null) return _homePlanet;

        float distanceToHome = Vector3.Distance(position, _homePlanet.position);
        if (distanceToHome < minDistance)
        {
            closest = _homePlanet;
        }

        return closest;
    }
}
