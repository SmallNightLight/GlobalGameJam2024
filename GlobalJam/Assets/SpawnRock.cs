using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRock : MonoBehaviour
{
    [SerializeField] GameObject rockPrefab;

    private bool _spawned;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_spawned) return;

        _spawned = true;
        Instantiate(rockPrefab, gameObject.transform.position, Quaternion.identity);
    }
}
