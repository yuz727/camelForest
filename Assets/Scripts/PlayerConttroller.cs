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
  public Animator anim;
  public SpriteRenderer sprite;
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
      sprite.flipX = true;
      anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
      sprite = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
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
        anim.SetBool("isJump", true);
        Jump();
      }
      else if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.JoystickButton1)))
      {
        anim.SetBool("isJump", false);
      }
      if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.JoystickButton5))
          && canDash)
      {
        anim.SetBool("isDash", true);
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
      anim.SetBool("isRun", false);
      playerBody.velocity = new Vector2(0, playerBody.velocity.y);
      return;
    }
    if ((facingRight && inputHorizontalDirection < 0.0f) || (!facingRight && inputHorizontalDirection > 0.0f))
    {
      facingRight = !facingRight;
      sprite.flipX = !sprite.flipX;
    }
    if (facingRight)
      anim.SetBool("isRun", true);
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
    anim.SetBool("isDash", false);
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