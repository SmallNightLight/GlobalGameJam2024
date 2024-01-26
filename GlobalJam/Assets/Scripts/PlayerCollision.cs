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

    [System.Serializable]
    public class CollisionEvent : UnityEvent<Collision2D> { }

    [System.Serializable]
    private struct TileEvent
    {
        public TileBase tile;
        public CollisionEvent collisionEvent;
    }

    [SerializeField] private BoolReference _onGround;


    private void FixedUpdate()
    {
        //RayCast to check if player is on ground
        RaycastHit2D[] rayCasts = Physics2D.BoxCastAll(transform.position + new Vector3(groundCheckBox.position.x, groundCheckBox.position.y, 0), groundCheckBox.size, 0f, Vector2.down, 0f, groundLayer);
        
        if (rayCasts.Length != 0)
        {
            _onGround.Value = true;
        }
        else
        {
            _onGround.Value = false;
        }
    }

    private void OnDrawGizmos()
    {
        //Draw the ground colliders on screen for debug purposes
        if (_onGround.Value) Gizmos.color = Color.green; else Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(groundCheckBox.position.x, groundCheckBox.position.y, 0), groundCheckBox.size);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckCollision(collision);
    }

    private void CheckCollision(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Tilemap>() != null)
        {
            CheckTile(collision);
        }
    }

    private void CheckTile(Collision2D collision)
    {      
        //var map = collision.gameObject.GetComponent<Tilemap>();
        //var grid = map.layoutGrid;

        //List<CollisionEvent> collisionEvents = new List<CollisionEvent>();

        //foreach(ContactPoint2D contact in collision.contacts)
        //{
        //    Vector3 contactPoint = contact.point - 0.05f * contact.normal;
        //    Vector3 gridPosition = grid.transform.InverseTransformPoint(contactPoint);
        //    Vector3Int cell = grid.LocalToCell(gridPosition);

        //    var tile = map.GetTile(cell);

        //    if (tile == null)
        //        continue;

        //    if (_effectMap.TryGetValue(tile, out CollisionEvent collisionEvent) && collisionEvent != null)
        //       collisionEvents.Add(collisionEvent);
        //}

        //foreach(CollisionEvent collisionEvent in collisionEvents)
        //    collisionEvent.Invoke(collision);
    }
}
