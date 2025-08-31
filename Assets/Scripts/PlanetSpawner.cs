using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _planetPrefab;
    [SerializeField] private int _planetCount = 10;
    [SerializeField] private Rect _planetSpawnBounds;
    [SerializeField] private float _minSpacing = 1.5f; 

    private List<Vector2> _spawnedPositions = new();

    void Awake()
    {
        SpawnPlanets();
    }
    private void SpawnPlanets()
    {
        // Get planet radius from prefab
        float planetRadius = 0.5f;
        var collider = _planetPrefab.GetComponent<CircleCollider2D>();
        if (collider != null)
            planetRadius = collider.radius * _planetPrefab.transform.localScale.x;

        int maxAttempts = 1000;

        for (int i = 0; i < _planetCount; i++)
        {
            bool positionFound = false;
            Vector2 spawnPos = Vector2.zero;
            int attempts = 0;

            while (!positionFound && attempts < maxAttempts)
            {
                attempts++;
                float x = Random.Range(_planetSpawnBounds.xMin + planetRadius, _planetSpawnBounds.xMax - planetRadius);
                float y = Random.Range(_planetSpawnBounds.yMin + planetRadius, _planetSpawnBounds.yMax - planetRadius);
                spawnPos = new Vector2(x, y);

                positionFound = true;
                foreach (var pos in _spawnedPositions)
                {
                    float minDist = planetRadius * 2 + _minSpacing;
                    if (Vector2.Distance(spawnPos, pos) < minDist)
                    {
                        positionFound = false;
                        break;
                    }
                }
            }

            if (positionFound)
            {
                var planet = Instantiate(_planetPrefab, spawnPos, Quaternion.identity, transform);
                _spawnedPositions.Add(spawnPos);
            }
            else
            {
                Debug.LogWarning($"Could not find non-overlapping position for planet {i + 1}");
            }
        }
    }
}
