using ScriptableArchitecture.Data;
using UnityEngine;
using UnityEngine.Video;

public class EndVideo : MonoBehaviour
{
    [SerializeField] private VideoPlayer _player;
    [SerializeField] private GameObject _playerX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _player.Play();
            _playerX.SetActive(false);
        }
           
    }
}