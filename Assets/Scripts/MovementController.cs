using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Rendering;
using UnityEngine;

public class MovementController : MonoBehaviour
{

  [SerializeField] Rigidbody2D playerBody;
  [SerializeField] BoxCollider2D groundCheck;
  [SerializeField] LayerMask groundLayer;

  readonly float speed = 5f;
  readonly float jumpSpeed = 30f;
  readonly float dashSpeed = 50f;
  bool facingRight = true;
  public bool grounded;
  public int extraJump = 0;
  float inputDirection;

  public float dashTimer = 0.5f;
  // Update is called once per frame
  void Update()
  {
    dashTimer -= Time.deltaTime;

    if (dashTimer <= 0.0f)
    {
      inputDirection = Input.GetAxis("Horizontal");
      Move();
      if (Input.GetKeyDown(KeyCode.Space) && (grounded || extraJump > 0))
      {
        Jump();
      }
      if (Input.GetKeyDown(KeyCode.LeftShift))
      {
        Dash();
      }
    }
  }
  void FixedUpdate()
  {
    grounded = groundCheck.IsTouchingLayers(groundLayer);
    if (grounded)
    {
      extraJump = 0;
    }
  }

  void Move()
  {
    if (inputDirection == 0.0f)
    {
      playerBody.velocity = new Vector2(0, playerBody.velocity.y);
      return;
    }
    facingRight = inputDirection > 0f;
    playerBody.velocity = new Vector2(inputDirection * speed, playerBody.velocity.y);

  }

  void Jump()
  {
    playerBody.velocity = new Vector2(playerBody.velocity.x, jumpSpeed);
    if (extraJump > 0)
    {
      extraJump--;
    }
  }

  void Dash()
  {
    playerBody.velocity = new Vector2(dashSpeed, playerBody.velocity.y);
    dashTimer = 0.2f;
  }
}
