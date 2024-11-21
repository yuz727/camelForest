using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
  public GameObject DialogueBox;

  public void OpenDialogueBox()
  {
    DialogueBox.SetActive(true);
  }
  public void CloseDialogueBox()
  {
    DialogueBox.SetActive(false);
  }
}
