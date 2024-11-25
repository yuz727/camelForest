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
  public static PlayerController S_Instance;
  public Rigidbody2D PlayerBody;
  public BoxCollider2D GroundCheck;
  public LayerMask GroundLayer;
  public Animator Anim;
  public SpriteRenderer Sprite;
  public bool CanDoubleJump;
  public bool CanDash;
  public bool Talking;
  public bool Jumping;
  public int ExtraJump;
  public bool Invincibility;
  public bool Mushroomed;
  public Vector3 RespawnPoint = new(43f, -4f, -5f);
  private readonly float _acceleration = 50f;
  private readonly float _maxHorizontalSpeed = 5f;
  private readonly float _jumpSpeed = 20f;
  private readonly float _dashSpeed = 30f;
  private bool _facingRight;
  private bool _grounded;
  private bool _dashing;
  private float _inputHorizontalDirection;


  void Update()
  {
    if (_dashing || Talking)
    {
      Anim.SetBool("isJump", false);
      Anim.SetBool("isDash", false);
      Anim.SetBool("isRun", false);
      if (Talking)
      {
        PlayerBody.velocity = new Vector2(0f, 0f);
      }
      return;
    }
    _inputHorizontalDirection = Input.GetAxisRaw("Horizontal");
    _inputHorizontalDirection = (_inputHorizontalDirection == 0f) ? 0 : (_inputHorizontalDirection > 0) ? 1f : -1f;
    Move();

    if (InputHandling.CheckJumpDown() && (_grounded || ExtraJump > 0))
    {
      Debug.Log("Jumping");
      Jump();
    }
    else if (InputHandling.CheckJumpUp() && Jumping)
    {
      Anim.SetBool("isJump", false);
      Jumping = false;
      PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, 0);
    }
    if (InputHandling.CheckDash() && CanDash)
    {
      Dash();
    }
  }

  void FixedUpdate()
  {
    if (_dashing || Talking)
    {
      Anim.SetBool("isJump", false);
      Anim.SetBool("isDash", false);
      Anim.SetBool("isRun", false);
      if (Talking)
      {
        PlayerBody.velocity = new Vector2(0f, 0f);
      }
      return;
    }
    _grounded = GroundCheck.IsTouchingLayers(GroundLayer);
    if (_grounded) Anim.SetBool("isJump", false);
    if (_grounded && CanDoubleJump) ExtraJump = 1;

  }

  void Move()
  {
    if (_inputHorizontalDirection == 0.0f)
    {
      Anim.SetBool("isRun", false);
      PlayerBody.velocity = new Vector2(0, PlayerBody.velocity.y);
      return;
    }
    if ((_facingRight && _inputHorizontalDirection < 0.0f) || (!_facingRight && _inputHorizontalDirection > 0.0f))
    {
      _facingRight = !_facingRight;
      Sprite.flipX = !Sprite.flipX;
    }
    if (!Jumping)
    {
      Anim.SetBool("isRun", true);
    }
    PlayerBody.velocity = new Vector2(PlayerBody.velocity.x + _inputHorizontalDirection * _acceleration * Time.deltaTime,
                                        PlayerBody.velocity.y);
    // Speed Cap
    if (PlayerBody.velocity.x > _maxHorizontalSpeed)
    {
      PlayerBody.velocity = new Vector2(_maxHorizontalSpeed, PlayerBody.velocity.y);
    }
    else if (PlayerBody.velocity.x < -_maxHorizontalSpeed)
    {
      PlayerBody.velocity = new Vector2(-_maxHorizontalSpeed, PlayerBody.velocity.y);
    }
  }
  public void DuckJump(float speed)
  {
    Anim.SetBool("isJump", true);
    Jumping = true;
    PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, speed);
  }

  void Jump()
  {
    Anim.SetBool("isJump", true);
    Jumping = true;
    PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, _jumpSpeed);
    if (ExtraJump > 0)
    {
      ExtraJump--;
    }
  }

  void Dash()
  {
    if (_inputHorizontalDirection == 0.0) return;
    Anim.SetBool("isDash", true);
    var gravity = PlayerBody.gravityScale;
    var dashTime = 0.15f;
    if (Mushroomed)
    {
      _inputHorizontalDirection = -_inputHorizontalDirection;
      dashTime = 0.4f;
    }
    PlayerBody.velocity = new Vector2(_inputHorizontalDirection * _dashSpeed, 0);
    PlayerBody.gravityScale = 0;
    StartCoroutine(DashTimer(gravity, dashTime));
    _dashing = true;
  }

  private IEnumerator DashTimer(float gravity, float time)
  {
    yield return new WaitForSeconds(time);
    PlayerBody.gravityScale = gravity;
    PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, 0);
    CanDash = false;
    _dashing = false;
    Anim.SetBool("isDash", false);
    StartCoroutine(DashCooldown());
  }

  private IEnumerator DashCooldown()
  {
    yield return new WaitForSeconds(.5f);
    CanDash = true;
  }

  public void KillPlayer()
  {
    Respawn();
  }

  private void Respawn()
  {
    PlayerBody.position = RespawnPoint;
  }


  void Awake()
  {
    if (S_Instance == null)
    {
      S_Instance = this;
      CanDoubleJump = false;
      CanDash = true;
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
      _facingRight = false;
      Anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
      Sprite = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
      GroundLayer = LayerMask.GetMask("Ground");
      PlayerBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
      GroundCheck = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<BoxCollider2D>();
    }
  }
}

