using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DeathSpawner : MonoBehaviour
{
    [SerializeField] GameObject deathObject;
    [SerializeField] Transform deathSpawn;
    private void Awake()
    {
            Instantiate(deathObject, deathSpawn.position, Quaternion.identity);
    }
}
