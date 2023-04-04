using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    internal int health;
    internal Rigidbody2D rb;
    internal float moveSpeed = 20f, jumpForce = 40f, wallHorizontalJumpForce = 35f, dashForce = 60f;
    internal float wallSlideSpeed = 2f;
    internal float fallMultiplier = 7.5f, lowJumpMultiplier = 7.5f, playerGravityScale = 2;
    internal int  jumpCapacity = 2, maxJumpCapacity = 2;
    internal bool dashable = true, grounded = true;
    internal bool stopMovement = false;
    internal float horizontal, vertical;

    PlayerState playerState;
    FaceDirection faceDirection;
    WallDirection wallDirection;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        faceDirection = FaceDirection.Right;
        wallDirection = WallDirection.None;
    }

    private void FixedUpdate()
    {
        if (playerState == PlayerState.Active) {
            //Horizontal input to player's face direction
            if (horizontal < 0) {
                faceDirection = FaceDirection.Left;
            } else if (horizontal > 0) {
                faceDirection = FaceDirection.Right;
            }

            //Horizontal input to movement, Stop if not moving
            if (horizontal != 0) {
                rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
            } else if (!grounded && horizontal == 0 || grounded) {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            //Player falls faster if they remove 
            if (!grounded && wallDirection == WallDirection.None) {
                if (rb.velocity.y < 0) {
                    rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
                } else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) {
                    rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
                }
            }
            if (wallDirection != WallDirection.None) {
                transform.Translate(0, -wallSlideSpeed * Time.deltaTime, 0);
            }
        }
    }

    //Change direction of player's facing
    void Flip(FaceDirection direction)
    {
        Vector3 selfScale = transform.localScale;
        selfScale.x *= -1;
        transform.localScale = selfScale;
    }

    internal void Dash() {
        StartCoroutine(DashCoroutine(faceDirection));
        SoundManager.Instance.PlaySound("Dash");
    }

    //Dash timing
    IEnumerator DashCoroutine(FaceDirection direction) {
        playerState = PlayerState.Dashing;
        dashable = false;
        switch(direction) {
            case FaceDirection.Right:
                rb.velocity = new Vector2(dashForce, 0);
                break;
            case FaceDirection.Left:
                rb.velocity = new Vector2(-dashForce, 0);
                break;
        }
        rb.gravityScale = 0;
        yield return new WaitForSeconds(0.25f);
        rb.velocity = new Vector2(rb.velocity.x / 4, rb.velocity.y);
        playerState = PlayerState.Active;
        rb.gravityScale = playerGravityScale;
        StartCoroutine(DashCooldown());
        yield break;
    }

    //Dash cooldown before being able to dash again
    IEnumerator DashCooldown() {
        yield return new WaitForSeconds(0.5f);
        dashable = true;
        yield break;
    }

    public void CancelDash() {
        playerState = PlayerState.Active;
        rb.gravityScale = playerGravityScale;
    }

    internal void JumpInput() {
        bool jumping = false;
        if (!grounded) {
            switch (wallDirection) {
                case WallDirection.Left: 
                    rb.velocity = new Vector2(wallHorizontalJumpForce, jumpForce);
                    jumpCapacity = 1;
                    StartCoroutine(WallJumpCoroutine());
                    jumping = true;
                    break;
                case WallDirection.Right:
                    rb.velocity = new Vector2(-wallHorizontalJumpForce, jumpForce);
                    jumpCapacity = 1;
                    StartCoroutine(WallJumpCoroutine());
                    jumping = true;
                    break;
            }
        }
        if (jumpCapacity > 0 && !jumping) {
            rb.velocity = new Vector2(horizontal, jumpForce);
            jumpCapacity--;
            grounded = false;
            jumping = true;
        }
        if (jumping) {
            SoundManager.Instance.PlaySound("Jump");
        }
    
    }

    IEnumerator WallJumpCoroutine() {
        playerState = PlayerState.Staggered;
        yield return new WaitForSeconds(0.15f);
        playerState = PlayerState.Active;
        yield break;
    }

    public void Knockback(Vector2 angle, float magnitude) {
        rb.AddForce(new Vector2(angle.x * magnitude, angle.y * magnitude), ForceMode2D.Impulse);
    }

    internal void Ground() {
        grounded = true;
        jumpCapacity = maxJumpCapacity;
    }

    internal void Unground() {
        grounded = false;
        jumpCapacity = 1;
    }

    internal void Wall(WallDirection direction) {
        playerState = PlayerState.Active;
        wallDirection = direction;
    }

    internal void Unwall() {
        wallDirection = WallDirection.None;
    }
}