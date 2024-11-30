using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RikiEvent : NPCController
{
  private bool _canTalk;
  private bool _isTalking;
  void Start()
  {
    playerController = FindFirstObjectByType<PlayerController>();
    canvasController = FindFirstObjectByType<CanvasController>();
    dialogueTrigger = FindFirstObjectByType<DialogueTrigger>();
    dialogueManager = FindFirstObjectByType<DialogueManager>();
    itemController = FindFirstObjectByType<ItemController>();
  }
  void Update()
  {
    if (InputHandling.CheckInteract() && _canTalk)
    {
      itemController.SetSpecialItem(SpecialItems.Dynamite);
    }
  }

  void FixedUpdate()
  {
    _canTalk = CheckPlayerOverlap();
  }

}
