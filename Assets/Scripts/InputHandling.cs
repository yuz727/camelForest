using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandling : MonoBehaviour
{
  public static InputHandling S_Instance;
  void Awake()
  {
    if (S_Instance == null)
    {
      S_Instance = this;
      SetDualShock();
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }
  // 0 -> Jump, 1 -> UseItem, 2 -> Interact, 3 -> Dash 4 -> Switch Item Left, 5 -> Switch Item Right
  static List<KeyCode> keyCodes;
  public static void SetDualShock()
  {
    keyCodes = new() {KeyCode.Joystick1Button1,
                      KeyCode.Joystick1Button2,
                      KeyCode.Joystick1Button0,
                      KeyCode.Joystick1Button5,
                      KeyCode.Joystick1Button6,
                      KeyCode.Joystick1Button7};
  }

  public static void SetXbox()
  {
    keyCodes = new() {KeyCode.Joystick2Button0,
                      KeyCode.Joystick2Button1,
                      KeyCode.Joystick2Button2,
                      KeyCode.Joystick2Button5,
                      KeyCode.Mouse1,
                      KeyCode.Mouse0};
  }

  public static bool CheckJumpDown()
  {
    return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(keyCodes[0]);
  }
  public static bool CheckJumpUp()
  {
    return Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(keyCodes[0]);
  }

  public static bool CheckUseItem()
  {
    return Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(keyCodes[1]);
  }

  public static bool CheckInteract()
  {
    return Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(keyCodes[2]);
  }

  public static bool CheckDash()
  {
    return Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(keyCodes[3]);
  }


  // Swap Items in slot 0 1 or 2
  public static int CheckSwitchItem(int currentItem)
  {
    var ret = currentItem;
    if (Input.GetKeyDown(KeyCode.Alpha1)) ret = 0;
    if (Input.GetKeyDown(KeyCode.Alpha2)) ret = 1;
    if (Input.GetKeyDown(KeyCode.Alpha3)) ret = 2;
    if (Input.GetKeyDown(keyCodes[4]))
    {
      if (currentItem == 0) ret = 2;
      else ret--;
    }
    if (Input.GetKeyDown(keyCodes[5]))
    {
      if (currentItem == 2) ret = 0;
      else ret--;
    }
    return ret;

  }
}