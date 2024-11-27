using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  public Transform playerLocation;
  public PlayerController playerController;
  public bool LookingDown;
  public float translationFactor = 20;
  private readonly float _maxOffset = 3f;
  public float LookTimer;
  private float _inputDirection;
  public CameraState _cameraState;
  void Start()
  {
    playerController = FindFirstObjectByType<PlayerController>();
    LookingDown = false;
  }

  void Update()
  {
    _inputDirection = Input.GetAxisRaw("Vertical");
    var horizontal = Input.GetAxisRaw("Horizontal");
    switch (_inputDirection)
    {
      case var _inputDirection when _inputDirection < 0f && Math.Abs(horizontal) < 0.5f:
        if (_cameraState == CameraState.Down)
        {
          if (LookTimer > 0.75f)
          {
            playerController.Viewing = true;
            transform.position = new Vector3(transform.position.x, GetDownwardView(), -10f);
          }
          else
          {
            LookTimer += Time.deltaTime;
          }
        }
        else
        {
          playerController.Viewing = false;
          LookTimer = 0f;
          _cameraState = CameraState.Down;
        }
        break;
      case var _inputDirection when _inputDirection > 0f && Math.Abs(horizontal) < 0.5f:
        if (_cameraState == CameraState.Up)
        {
          if (LookTimer > 0.75f)
          {
            playerController.Viewing = true;
            transform.position = new Vector3(transform.position.x, GetUpwardView(), -10f);
          }
          else
          {
            LookTimer += Time.deltaTime;
          }
        }
        else
        {
          playerController.Viewing = false;
          LookTimer = 0f;
          _cameraState = CameraState.Up;
        }
        break;
      default:
        playerController.Viewing = false;
        LookTimer = 0f;
        _cameraState = CameraState.Neutral;
        break;
    }
  }

  private float GetDownwardView()
  {
    if (Math.Abs(transform.position.y - 3 - playerLocation.position.y) < _maxOffset)
    {
      return (float)(transform.position.y - 0.1);
    }
    return transform.position.y;
  }
  private float GetUpwardView()
  {
    if (Math.Abs(transform.position.y - playerLocation.position.y) < 3 + _maxOffset)
    {
      return (float)(transform.position.y + 0.1);
    }
    return transform.position.y;
  }

  void LateUpdate()
  {
    if (transform.position != playerLocation.position && !playerController.Viewing)
    {
      transform.position = new Vector3(transform.position.x + (playerLocation.position.x - transform.position.x) / translationFactor,
                             3 + playerLocation.position.y + (playerLocation.position.y - transform.position.y) / translationFactor,
                             -10);
    }
  }
}

public enum CameraState
{
  Neutral,
  Up,
  Down
}