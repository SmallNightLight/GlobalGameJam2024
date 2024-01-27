using UnityEngine;

public class EatPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _bloodyPlayerPrefab;

    public void EatPlayerNow()
    {
        Instantiate(_bloodyPlayerPrefab, transform.position, Quaternion.identity);
    }
}