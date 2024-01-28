using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRock : MonoBehaviour
{
    [SerializeField] GameObject rockPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(rockPrefab, gameObject.transform.position, Quaternion.identity);
    }
}
