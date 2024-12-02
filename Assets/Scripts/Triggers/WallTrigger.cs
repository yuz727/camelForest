using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrigger : MonoBehaviour
{
  public BoxCollider2D trigger;
  public CanvasController canvasController;
  public LayerMask player;
  public DialogueTrigger dialogueTrigger;
  public bool Collided;
  // Start is called before the first frame update
  void Start()
  {
    trigger = GetComponent<BoxCollider2D>();
    canvasController = FindFirstObjectByType<CanvasController>();
    dialogueTrigger = FindFirstObjectByType<DialogueTrigger>();
  }

  // Update is called once per frame
  void Update()
  {
    if (!Collided && trigger.IsTouchingLayers(player))
    {
      Collided = true;
      canvasController.OpenDialogueBox();
      dialogueTrigger.Dialogue = new() { NPCName = "", Sentences = new string[] { "No." } };
      dialogueTrigger.TriggerDialogue();
      StartCoroutine(Hint());
    }
    else if (!trigger.IsTouchingLayers(player))
    {

      Collided = false;
    }
  }
  private IEnumerator Hint()
  {
    yield return new WaitForSeconds(3f);
    canvasController.CloseDialogueBox();
  }
}


