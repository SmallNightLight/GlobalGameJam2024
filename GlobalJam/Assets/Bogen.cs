using ScriptableArchitecture.Data;
using UnityEngine;

public class Bogen : MonoBehaviour
{
    [SerializeField] private IntReference _deathCounter;
    [SerializeField] private int _beforeLives = 3;

    private void Start()
    {
        if (_deathCounter.Value >= _beforeLives)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponentInChildren<SpriteRenderer>().enabled = true;
        }
    }

    private void Update()
    {
        
    }
}