using ScriptableArchitecture.Data;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private FloatReference _horizontalInput;
    [SerializeField] private Vector2Reference _velocity;
    [SerializeField] private BoolReference _onGround;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();

    }

    private void Update()
    {
        _animator.SetBool("IsMoving", Mathf.RoundToInt(_horizontalInput.Value) != 0);

        if (!_onGround.Value)
        {
            bool isFalling = _velocity.Value.y < 0;
            _animator.SetBool("IsJumping", !isFalling);
            _animator.SetBool("IsFalling", isFalling);
        }
        else
        {
            _animator.SetBool("IsJumping", false);
            _animator.SetBool("IsFalling", false);
        }
    }
}