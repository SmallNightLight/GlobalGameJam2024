using ScriptableArchitecture.Data;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Vector2Reference _playerPosition;
    [SerializeField] private float _speed;
    [SerializeField] private float _downSpeed;
    [SerializeField] private Vector3 _endPosition;
    [SerializeField] private Vector3 _downToPlayerPosition;

    private Vector3 _startPosition;
    private bool _movingToEnd = true;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _startPosition = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (IsPlayerUnder())
            MovePlatform(_downToPlayerPosition, _downSpeed);
        else
            MovePlatform(_movingToEnd ? _endPosition : _startPosition, _speed);
    }

    private bool IsPlayerUnder()
    {
        bool inX = _playerPosition.Value.x > _spriteRenderer.bounds.min.x && _playerPosition.Value.x < _spriteRenderer.bounds.max.x;
        bool inY = _playerPosition.Value.y < _spriteRenderer.bounds.min.y;

        return inX && inY;

    }

    private void MovePlatform(Vector3 targetPosition, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position == targetPosition)
            _movingToEnd = !_movingToEnd;
    }
}