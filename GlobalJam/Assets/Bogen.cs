using ScriptableArchitecture.Data;
using UnityEngine;

public class Bogen : MonoBehaviour
{
    [SerializeField] private IntReference _deathCounter;
    [SerializeField] private int _beforeLives = 3;
    [SerializeField] private GameObject _child;

    private void Start()
    {
        if (_deathCounter.Value >= _beforeLives)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<PolygonCollider2D>().enabled = false;
            _child.SetActive(true);
        }
    }

    private void Update()
    {
        
    }
}