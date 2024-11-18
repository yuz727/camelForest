using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
  public static PlayerController _instance;
  public Rigidbody2D playerBody;
  public BoxCollider2D groundCheck;
  public LayerMask groundLayer;
  public Animator anim;
  public SpriteRenderer sprite;
  readonly float acceleration = 50f;
  readonly float maxHorizontalSpeed = 5f;
  readonly float maxJumpSpeed = 15f;
  readonly float dashSpeed = 30f;

  public SpecialItems specialItem;
  public List<Items> itemsOwned;
  public bool canDoubleJump;
  public bool canDash;
  public bool talking;
  public int extraJump;
  bool facingRight;
  bool grounded;
  bool dashing;

  bool canJump = true;
  bool jumping;
  float jumpSpeed = 0f;

  float inputHorizontalDirection;



  void Update()
  {
    if (dashing || talking)
    {
      anim.SetBool("isJump", false);
      anim.SetBool("isDash", false);
      anim.SetBool("isRun", false);
      playerBody.velocity = new Vector2(0, 0);
      return;
    }
    if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1))
      && (grounded || extraJump > 0) && canJump && !jumping)
    {
      anim.SetBool("isJump", true);
      Jump();
    }
    else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.JoystickButton1))
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

  void FixedUpdate()
  {
    if (dashing || talking)
    {
      anim.SetBool("isJump", false);
      anim.SetBool("isDash", false);
      anim.SetBool("isRun", false);
      playerBody.velocity = new Vector2(0, 0);
      return;
    }

    inputHorizontalDirection = Input.GetAxisRaw("Horizontal");
    inputHorizontalDirection = (inputHorizontalDirection > 0) ?
      (float)Math.Ceiling(inputHorizontalDirection) : (float)Math.Floor(inputHorizontalDirection);
    Move();
    CheckCanJump();
    if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.JoystickButton1)) && jumping && canJump)
    {
      Debug.Log("accelerating");
      jumpSpeed = (jumpSpeed >= maxJumpSpeed) ? maxJumpSpeed :
        jumpSpeed + playerBody.velocity.y + acceleration * Time.deltaTime;
      playerBody.velocity = new Vector2(playerBody.velocity.x, jumpSpeed);
    }
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
    anim.SetBool("isRun", true);
    playerBody.velocity = new Vector2(playerBody.velocity.x + inputHorizontalDirection * acceleration * Time.deltaTime,
                                        playerBody.velocity.y);
    if (playerBody.velocity.x > maxHorizontalSpeed)
    {
      playerBody.velocity = new Vector2(maxHorizontalSpeed, playerBody.velocity.y);
    }
    else if (playerBody.velocity.x < -maxHorizontalSpeed)
    {
      playerBody.velocity = new Vector2(-maxHorizontalSpeed, playerBody.velocity.y);
    }
  }

  void Jump()
  {

    jumping = true;
    playerBody.velocity = new Vector2(playerBody.velocity.x, playerBody.velocity.y + acceleration * Time.deltaTime);
    if (extraJump > 0)
    {
      extraJump--;
    }
  }

  void Dash()
  {
    if (inputHorizontalDirection == 0.0)
    {
      return;
    }
    var yVel = playerBody.velocity.y;
    var gravity = playerBody.gravityScale;
    playerBody.velocity = new Vector2(inputHorizontalDirection * dashSpeed, 0);
    playerBody.gravityScale = 0;
    StartCoroutine(DashTimer(yVel, gravity, 0.2f));
    dashing = true;
  }

  private IEnumerator DashTimer(float yVel, float gravity, float time)
  {
    yield return new WaitForSeconds(time);
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

  private void CheckCanJump()
  {
    if (jumpSpeed >= maxJumpSpeed)
    {
      canJump = false;
    }
    if (!canJump && playerBody.velocity.y == 0f)
    {
      canJump = true;
      jumping = false;
    }
  }

  public void UseItem(Items item)
  {
    if (itemsOwned.Contains(item))
    {
      itemsOwned.Remove(item);
    }
  }

  void Awake()
  {
    if (_instance == null)
    {
      _instance = this;
      canDoubleJump = true;
      canDash = true;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  void OnEnable()
  {
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  void OnDisable()
  {
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }

  void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    Debug.Log("Entering " + scene.name);
    if (!scene.name.Equals("Menu"))
    {
      facingRight = false;
      anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
      sprite = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
      groundLayer = LayerMask.GetMask("Ground");
      playerBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
      groundCheck = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<BoxCollider2D>();
    }
  }
}

public enum SpecialItems
{
  Crowbar,
  Dynamite,
  Sword
}
public enum Items
{
  SilverKey,
  Notebook,
  Mushroom,
  Bow,
  Cucumber
}