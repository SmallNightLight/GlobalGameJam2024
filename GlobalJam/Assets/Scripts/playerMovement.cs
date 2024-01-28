using ScriptableArchitecture.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class playerMovement : MonoBehaviour 
{
    [Header("Components")]
    private Rigidbody2D rigidBody;

    [Header("Movement Stats")]
    [SerializeField, Range(0f, 20f)][Tooltip("Maximum movement speed")] 
    public float maxSpeed = 10f;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to reach max speed")] 
    public float maxAcceleration = 52f;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop after letting go")] 
    public float maxDecceleration = 52f;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop when changing direction")] 
    public float maxTurnSpeed = 80f;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to reach max speed when in mid-air")] 
    public float maxAirAcceleration;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop in mid-air when no direction is used")] 
    public float maxAirDeceleration;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop when changing direction when in mid-air")] 
    public float maxAirTurnSpeed = 80f;

    [Header("Options")]
    [Tooltip("When false, the charcter will skip acceleration and deceleration and instantly move and stop")] public bool useAcceleration;

    [Header("Calculations")]
    [SerializeField] private FloatReference _directionX;
    private Vector2 desiredVelocity;
    [SerializeField] private Vector2Reference velocity;
    private float maxSpeedChange;
    private float acceleration;
    private float deceleration;
    private float turnSpeed;
    private bool pressingKey;

    [Header("Slow Down variables")]
    [SerializeField] private FloatReference _slowDownFactor;
    [SerializeField] private FloatReference _slowDownJumpFactor;

    [Header("Current State")]
    [SerializeField] private BoolReference canMove;
    [SerializeField] private BoolReference onGround;

    [SerializeField] private Vector2Reference PlayerPosition;

    private void Awake()
    {
        PlayerPosition.Value = transform.position;
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        //playerEffects = GetComponent<playerEffects>();

        ResetLevel();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        //Debug.Log("Moving");
        if (canMove.Value)
        {
            _directionX.Value = context.ReadValue<float>();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (canMove.Value)
        {
            if (context.performed)
            {
                desiredJump = true;
                pressingJump = true;
            }

            if (context.canceled)
            {
                pressingJump = false;
            }
        }
    }

    private void Update()
    {
        PlayerPosition.Value = transform.position;

        if (!canMove.Value || _directionX.Value == 0)
        {
            _directionX.Value = 0;
            pressingKey = false;
        }
        else
        {
            transform.localScale = new Vector3(_directionX.Value > 0 ? 1 : -1, 1, 1);
            pressingKey = true;
        }

        desiredVelocity = new Vector2(_directionX.Value, 0f) * Mathf.Max(maxSpeed * _slowDownFactor.Value, 0f);

        JumpUpdate();
    }

    private void FixedUpdate()
    {
        velocity.Value = rigidBody.velocity;

        //Calculate movement, depending on the acceleration type
        if (useAcceleration || !onGround.Value)
            runWithAcceleration();
        else
            runWithoutAcceleration();

        JumpFixedUpdate();
    }

    private void runWithAcceleration() 
    {
        //Set our acceleration, deceleration, and turn speed stats, based on whether we're on the ground on in the air

        acceleration = onGround.Value ? maxAcceleration : maxAirAcceleration;
        deceleration = onGround.Value ? maxDecceleration : maxAirDeceleration;
        turnSpeed = onGround.Value ? maxTurnSpeed : maxAirTurnSpeed;

        if (pressingKey) 
        {
            //If the sign (i.e. positive or negative) of our input direction doesn't match our movement, it means we're turning around and so should use the turn speed stat.
            if (Mathf.Sign(_directionX.Value) != Mathf.Sign(velocity.Value.x))
                maxSpeedChange = turnSpeed * Time.deltaTime;
            else 
            {
                //If they match, it means we're simply running along and so should use the acceleration stat
                maxSpeedChange = acceleration * Time.deltaTime;
            }
        }
        else 
        {
            //And if we're not pressing a direction at all, use the deceleration stat
            maxSpeedChange = deceleration * Time.deltaTime;
        }

        //Move our velocity towards the desired velocity, at the rate of the number calculated above
        velocity.Value = new Vector2(Mathf.MoveTowards(velocity.Value.x, desiredVelocity.x, maxSpeedChange), velocity.Value.y);

        //Update the Rigidbody with this new velocity
        rigidBody.velocity = velocity.Value;
    }

    private void runWithoutAcceleration() 
    {
        //If we're not using acceleration and deceleration, just send our desired velocity (direction * max speed) to the Rigidbody
        velocity.Value = new Vector2(desiredVelocity.x, velocity.Value.y);

        rigidBody.velocity = velocity.Value;
    }

    //Jumping

    [Header("Jump Stats")]
    [SerializeField, Range(2f, 10f)][Tooltip("Maximum jump height")] 
    public float jumpHeight = 7.3f;
    [SerializeField, Range(0.2f, 1.25f)][Tooltip("How long it takes to reach that height before coming back down")] 
    public float timeToJumpApex;
    [SerializeField, Range(0f, 5f)][Tooltip("Gravity multiplier to apply when going up")] 
    public float upwardMovementMultiplier = 1f;
    [SerializeField, Range(1f, 10f)][Tooltip("Gravity multiplier to apply when coming down")] 
    public float downwardMovementMultiplier = 6.17f;
    [SerializeField, Range(0, 1)][Tooltip("How many times can you jump in the air?")] 
    public int maxAirJumps = 0;

    [Header("Jump Options")]
    [Tooltip("Should the character drop when you let go of jump?")] 
    public bool variablejumpHeight;
    [SerializeField, Range(1f, 10f)][Tooltip("Gravity multiplier when you let go of jump")] 
    public float jumpCutOff;
    [SerializeField][Tooltip("The fastest speed the character can fall")] 
    public float speedLimit;
    [SerializeField, Range(0f, 0.3f)][Tooltip("How long should coyote time last?")] 
    public float coyoteTime = 0.15f;
    [SerializeField, Range(0f, 0.3f)][Tooltip("How far from ground should we cache your jump?")] 
    public float jumpBuffer = 0.15f;

    [Header("Calculations")]
    private float jumpSpeed;
    private float defaultGravityScale = 1;
    private float gravMultiplier;

    [Header("Current State")]
    private bool canJumpAgain = false;
    private bool desiredJump;
    private float jumpBufferCounter;
    private float coyoteTimeCounter = 0;
    private bool pressingJump;
    private bool currentlyJumping;


    void JumpUpdate()
    {
        setPhysics();

        //Jump buffer allows us to queue up a jump, which will play when we next hit the ground
        if (jumpBuffer > 0)
        {
            //Instead of immediately turning off "desireJump", start counting up...
            //All the while, the DoAJump function will repeatedly be fired off
            if (desiredJump)
            {
                jumpBufferCounter += Time.deltaTime;

                if (jumpBufferCounter > jumpBuffer)
                {
                    //If time exceeds the jump buffer, turn off "desireJump"
                    desiredJump = false;
                    jumpBufferCounter = 0;
                }
            }
        }

        //If we're not on the ground and we're not currently jumping, that means we've stepped off the edge of a platform.
        //So, start the coyote time counter...
        if (!currentlyJumping && !onGround.Value)
        {
            coyoteTimeCounter += Time.deltaTime;
        }
        else
        {
            //Reset it when we touch the ground, or jump
            coyoteTimeCounter = 0;
        }
    }

    private void setPhysics()
    { 
        //Determine the character's gravity scale, using the stats provided. Multiply it by a gravMultiplier, used later
        //Debug.Log(gravMultiplier + " " + jumpHeight + " " + timeToJumpApex);
        Vector2 newGravity = new Vector2(0, (-2 * jumpHeight) / (timeToJumpApex * timeToJumpApex));
        rigidBody.gravityScale = (newGravity.y / Physics2D.gravity.y) * gravMultiplier;
    }

    private void JumpFixedUpdate()
    {
        //Get velocity from Kit's Rigidbody 
        velocity.Value = rigidBody.velocity;

        //Keep trying to do a jump, for as long as desiredJump is true
        if (desiredJump)
        {
            DoAJump();
            rigidBody.velocity = velocity.Value;

            //Skip gravity calculations this frame, so currentlyJumping doesn't turn off
            //This makes sure you can't do the coyote time double jump bug
            return;
        }

        calculateGravity();
    }

    private void calculateGravity()
    {
        if (rigidBody.velocity.y > 0.01f)
        {
            if (onGround.Value)
            {
                //Don't change it if Kit is stood on something (such as a moving platform)
                gravMultiplier = defaultGravityScale;
            }
            else
            {
                //If we're using variable jump height...)
                if (variablejumpHeight)
                {
                    //Apply upward multiplier if player is rising and holding jump
                    if (pressingJump && currentlyJumping)
                    {
                        gravMultiplier = upwardMovementMultiplier;
                    }
                    //But apply a special downward multiplier if the player lets go of jump
                    else
                    {
                        gravMultiplier = jumpCutOff;
                    }
                }
                else
                {
                    gravMultiplier = upwardMovementMultiplier;
                }
            }
        }

        //Else if going down...
        else if (rigidBody.velocity.y < -0.01f)
        {

            if (onGround.Value)
            //Don't change it if Kit is stood on something (such as a moving platform)
            {
                gravMultiplier = defaultGravityScale;
            }
            else
            {
                //Otherwise, apply the downward gravity multiplier as Kit comes back to Earth
                gravMultiplier = downwardMovementMultiplier;

                if (!currentlyJumping && pressingJump)
                    gravMultiplier = defaultGravityScale;
            }

        }
        //Else not moving vertically at all
        else
        {
            if (onGround.Value)
            {
                currentlyJumping = false;
            }

            gravMultiplier = defaultGravityScale;
        }

        //Set the character's Rigidbody's velocity
        //But clamp the Y variable within the bounds of the speed limit, for the terminal velocity assist option
        rigidBody.velocity = new Vector3(velocity.Value.x, Mathf.Clamp(velocity.Value.y, -speedLimit, 100));
    }

    private void DoAJump()
    {
        //Create the jump, provided we are on the ground, in coyote time, or have a double jump available
        if (onGround.Value || (coyoteTimeCounter > 0.03f && coyoteTimeCounter < coyoteTime) || canJumpAgain)
        {
            desiredJump = false;
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;

            //If we have double jump on, allow us to jump again (but only once)
            canJumpAgain = (maxAirJumps == 1 && canJumpAgain == false);

            //Determine the power of the jump, based on our gravity and stats
            calculateGravity();
            setPhysics();

            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * rigidBody.gravityScale * jumpHeight * _slowDownJumpFactor.Value);

            //If Kit is moving up or down when she jumps (such as when doing a double jump), change the jumpSpeed;
            //This will ensure the jump is the exact same strength, no matter your velocity.
            if (velocity.Value.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.Value.y, 0f);
            }
            else if (velocity.Value.y < 0f)
            {
                jumpSpeed += Mathf.Abs(rigidBody.velocity.y);
            }

            //Apply the new jumpSpeed to the velocity. It will be sent to the Rigidbody in FixedUpdate;
            velocity.Value += new Vector2(0, jumpSpeed);
            currentlyJumping = true;

            //if (playerEffects != null)
            //{
            //    //Apply the jumping effects on the juice script
            //    playerEffects.jumpEffects();
            //}

            //SoundEffectRaiser.Raise(_soundJump);
        }

        if (jumpBuffer == 0)
        {
            //If we don't have a jump buffer, then turn off desiredJump immediately after hitting jumping
            desiredJump = false;
        }
    }

    public void bounceUp(float bounceAmount)
    {
        //Used by the springy pad
        rigidBody.AddForce(Vector2.up * bounceAmount, ForceMode2D.Impulse);
    }

    public void ResetLevel()
    {
        rigidBody.velocity = Vector2.zero;
        velocity.Value = Vector2.zero;
        _directionX.Value = 0f;
    }
}