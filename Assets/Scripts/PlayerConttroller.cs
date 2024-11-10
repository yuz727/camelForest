using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public static PlayerController _instance;

  public Rigidbody2D playerBody;
  public BoxCollider2D groundCheck;
  public LayerMask groundLayer;

  public List<Items> itemsOwned;

  readonly float speed = 5f;
  readonly float jumpSpeed = 30f;
  readonly float dashSpeed = 30f;
  public bool canDoubleJump;
  public bool canDash;
  bool facingRight = true;
  bool grounded;
  bool dashing;
  int extraJump;
  float inputHorizontalDirection;
  float gravity;
  float yVel;

  void Awake()
  {
    if (_instance == null)
    {
      _instance = this;
      canDoubleJump = true;
      canDash = true;
      groundLayer = LayerMask.GetMask("Ground");
      playerBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
      gravity = playerBody.gravityScale;
      groundCheck = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<BoxCollider2D>();
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  void Update()
  {
    inputHorizontalDirection = Input.GetAxisRaw("Horizontal");

    if (!dashing)
    {
      playerBody.gravityScale = gravity;
      Move();
      if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1)) && (grounded || extraJump > 0))
      {
        Jump();
      }
      if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.JoystickButton5))
          && canDash)
      {
        Dash();
      }
    }
  }
  void FixedUpdate()
  {
    grounded = groundCheck.IsTouchingLayers(groundLayer);
    if (grounded && canDoubleJump)
    {
      extraJump = 1;
    }
  }


  void Move()
  {
    if (inputHorizontalDirection == 0.0f)
    {
      playerBody.velocity = new Vector2(0, playerBody.velocity.y);
      return;
    }
    facingRight = inputHorizontalDirection > 0f;
    playerBody.velocity = new Vector2(inputHorizontalDirection * speed, playerBody.velocity.y);

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
    playerBody.velocity = new Vector2(inputHorizontalDirection * dashSpeed, 0);
    yVel = playerBody.velocity.y;
    playerBody.gravityScale = 0;
    StartCoroutine(DashTimer());
    dashing = true;
  }

  private IEnumerator DashTimer()
  {
    yield return new WaitForSeconds(.2f);
    playerBody.gravityScale = gravity;
    playerBody.velocity = new Vector2(playerBody.velocity.x, yVel);
    canDash = false;
    dashing = false;
    StartCoroutine(DashCooldown());
  }

  private IEnumerator DashCooldown()
  {
    yield return new WaitForSeconds(.5f);
    canDash = true;
  }

  void useItem(Items item)
  {
    if (itemsOwned.Contains(item))
    {
      itemsOwned.Remove(item);

    }
  }

}

public enum Items
{
  Item1,
  Item2
}