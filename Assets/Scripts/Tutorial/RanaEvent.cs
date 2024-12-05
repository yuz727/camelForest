using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class RanaEvent : NPCController
{
  public Transform RanaLocation;
  public BoxCollider2D UpTrigger;
  public BoxCollider2D DownTrigger;
  private bool _canTalk;
  private bool _isTalking;

  void Start()
  {
    playerController = FindFirstObjectByType<PlayerController>();
    canvasController = FindFirstObjectByType<CanvasController>();
    dialogueTrigger = FindFirstObjectByType<DialogueTrigger>();
    dialogueManager = FindFirstObjectByType<DialogueManager>();
    itemController = null;
  }

  void Update()
  {
    if (InputHandling.CheckInteract() && _canTalk)
    {
      canvasController.OpenDialogueBox();
      if (!_isTalking)
      {
        Dialogues = JsonConvert.DeserializeObject<DialogueCollection>(NPCDialogue.text).CollectionToQueue();
        _isTalking = true;
        playerController.Talking = true;
      }
      _isTalking = DisplayDialogue();
    }
  }

  void FixedUpdate()
  {
    _canTalk = CheckPlayerOverlap();
    if (UpTrigger != null && DownTrigger != null)
    {
      if (UpTrigger.IsTouchingLayers(Player))
      {
        RanaLocation.position = new Vector3(84.56f, 26.48f, -2);
      }
      if (DownTrigger.IsTouchingLayers(Player))
      {
        RanaLocation.position = new Vector3(36.96f, -3.15f, -2);
      }
    }
  }
}
