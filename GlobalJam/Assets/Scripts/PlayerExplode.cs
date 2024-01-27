using ScriptableArchitecture.Data;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExplode : MonoBehaviour
{
    [SerializeField] private Vector2Reference _spawnPosition;
    [SerializeField] private Transform _middlePoint;
    [SerializeField] private List<SpriteRenderer> _sprites = new List<SpriteRenderer>();
    [SerializeField] private float _force = 10f;
    [SerializeField] private GameObject _bloodPrefab;

    private void Start()
    {
        //Explode on start

        transform.position = _spawnPosition.Value;
        Vector3 forcePoint = _middlePoint.position;

        foreach (SpriteRenderer sprite in _sprites)
        {
            Rigidbody2D rigidbody = sprite.GetComponent<Rigidbody2D>();

            if (rigidbody != null)
            {
                Vector2 forceVector = (sprite.transform.position - forcePoint).normalized * _force;
                rigidbody.AddForce(forceVector, ForceMode2D.Impulse);
                GameObject blood = Instantiate(_bloodPrefab, rigidbody.transform.position, Quaternion.identity);
                Destroy(blood, 2f);
            }
        }
    }
}