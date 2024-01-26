using ScriptableArchitecture.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using System.Net;

public class PlayerCollision : MonoBehaviour
{
    [Header("Ground Settings")]
    [SerializeField][Tooltip("The box for the ground checking")] private Rect groundCheckBox;
    [SerializeField][Tooltip("Which layers are read as the ground")] private LayerMask groundLayer;

    [SerializeField] private BoolReference _onGround;

    private void FixedUpdate()
    {
        //RayCast to check if player is on ground
        RaycastHit2D[] rayCasts = Physics2D.BoxCastAll(transform.position + new Vector3(groundCheckBox.position.x, groundCheckBox.position.y, 0), groundCheckBox.size, 0f, Vector2.down, 0f, groundLayer);
        _onGround.Value = rayCasts.Length != 0;
    }

    private void OnDrawGizmos()
    {
        //Draw the ground colliders on screen for debug purposes
        if (_onGround.Value) Gizmos.color = Color.green; else Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(groundCheckBox.position.x, groundCheckBox.position.y, 0), groundCheckBox.size);
    }
}
