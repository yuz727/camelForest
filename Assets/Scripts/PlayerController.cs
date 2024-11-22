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
  public SpecialItems SpecialItem;
  public List<Items> ItemsOwned;
  public bool CanDoubleJump;
  public bool CanDash;
  public bool Talking;
  public bool Jumping;
  public int ExtraJump;
  public readonly Vector3 RespawnPoint = new Vector3(43f, -4f, -5f);
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
      PlayerBody.velocity = new Vector2(0, 0);
      return;
    }
    _inputHorizontalDirection = Input.GetAxisRaw("Horizontal");
    _inputHorizontalDirection = (_inputHorizontalDirection == 0f) ? 0 : (_inputHorizontalDirection > 0) ? 1f : -1f;
    Move();
    if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton3))
    {
      Jump();
    }
    if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1))
      && (_grounded || ExtraJump > 0))
    {
      Jump();
    }
    else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.JoystickButton1))
    {
      Anim.SetBool("isJump", false);
      Jumping = false;
      PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, 0);
    }
    if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.JoystickButton5))
        && CanDash)
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
      PlayerBody.velocity = new Vector2(0, 0);
      return;
    }
    _grounded = GroundCheck.IsTouchingLayers(GroundLayer);
    if (_grounded)
    {
      Anim.SetBool("isJump", false);
    }
    if (_grounded && CanDoubleJump)
    {
      ExtraJump = 1;
    }
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
    if (_inputHorizontalDirection == 0.0)
    {
      return;
    }
    Anim.SetBool("isDash", true);
    var yVel = PlayerBody.velocity.y;
    var gravity = PlayerBody.gravityScale;
    PlayerBody.velocity = new Vector2(_inputHorizontalDirection * _dashSpeed, 0);
    PlayerBody.gravityScale = 0;
    StartCoroutine(DashTimer(yVel, gravity, 0.2f));
    _dashing = true;
  }

  private IEnumerator DashTimer(float yVel, float gravity, float time)
  {
    yield return new WaitForSeconds(time);
    PlayerBody.gravityScale = gravity;
    PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, yVel);
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
  public void AddItem(Items item)
  {
    if (!ItemsOwned.Contains(item) && ItemsOwned.Count < 3)
    {
      FindObjectOfType<CanvasController>().OpenDialogueBox();
      ItemsOwned.Add(item);
      FindObjectOfType<DialogueTrigger>().Dialogue = new() { NPCName = "", Sentences = new string[] { $"Obtained the {item}!" } };
      FindObjectOfType<DialogueTrigger>().TriggerDialogue();
      StartCoroutine(DialogueUITimer());
    }
    else
    {
      Debug.Log("Item Owned Already");
    }
  }

  private IEnumerator DialogueUITimer()
  {
    yield return new WaitForSeconds(2f);
    FindObjectOfType<CanvasController>().CloseDialogueBox();
  }

  public void UseItem(Items item)
  {
    if (ItemsOwned.Contains(item))
    {
      ItemsOwned.Remove(item);
    }
    else
    {
      Debug.Log("Item Don't Exist");
    }
  }

  void Awake()
  {
    if (S_Instance == null)
    {
      S_Instance = this;
      CanDoubleJump = false;
      CanDash = false;
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

public enum SpecialItems
{
  None,
  Crowbar,
  Dynamite,
  Sword
}
public enum Items
{
  Key,
  Notebook,
  Mushroom,
  Bow,
  Cucumber
}