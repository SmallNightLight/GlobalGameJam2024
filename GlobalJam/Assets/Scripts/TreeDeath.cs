using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TreeDeath : MonoBehaviour
{
    [SerializeField] GameObject deathTree;
    [SerializeField] Transform deathSpawn;
    public bool diedFromThisOnce = false;
    private void Awake()
    {
            Instantiate(deathTree, deathSpawn.position, Quaternion.identity);
    }
}
