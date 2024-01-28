using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private FloatReference _horizontalInput;
    [SerializeField] private Vector2Reference _velocity;
    [SerializeField] private BoolReference _onGround;

    [SerializeField] BoolReference isVisible;

    [Header("Sound")]
    [SerializeField] private SoundEffectReference _soundEffectRaiser;
    [SerializeField] private List<SoundEffectReference> _footsteps;
    [SerializeField] private Vector2 _footstepTime = new Vector2(0.3f, 0.5f);

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(Footsteps());
    }

    private void Update()
    {
        if(!isVisible.Value)
        {
            gameObject.SetActive(false);
        }
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

    private IEnumerator Footsteps()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_footstepTime.x, _footstepTime.y));

            if (_footsteps != null && Mathf.RoundToInt(_horizontalInput.Value) != 0 && _onGround.Value)
                _soundEffectRaiser.Raise(_footsteps[Random.Range(0, _footsteps.Count - 1)].Value);

            yield return new WaitForEndOfFrame();
        }
    }
}