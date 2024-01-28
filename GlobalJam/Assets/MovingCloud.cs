using ScriptableArchitecture.Data;
using System.Collections;
using UnityEngine;

public class MovingCloud : MonoBehaviour
{
    [SerializeField] private Vector2 _upPosition;
    [SerializeField] private Vector2 _downPosition;
    [SerializeField] private float _speed;

    [SerializeField] private float _upTime;
    [SerializeField] private float _downTime;

    [SerializeField] private BoolReference _treeActive;

    private void Start()
    {
        transform.position = _upPosition;

        StartCoroutine(MoveCloud());
    }

    IEnumerator MoveCloud()
    {
        while (true)
        {
            //Go down
            _treeActive.Value = true;

            float timer = 0f;

            while (timer < _downTime)
            {
                MovePlatform(_downPosition);

                _treeActive.Value = new Vector2(transform.position.x, transform.position.y) == _downPosition;

                timer += Time.deltaTime;
                yield return null;
            }

            timer = 0f;

            //Go up
            _treeActive.Value = false;
            while (timer < _upTime)
            {
                MovePlatform(_upPosition);

                timer += Time.deltaTime;
                yield return null;
            }
        }
    }

    private void MovePlatform(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
    }
}