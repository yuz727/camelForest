using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class GabeEvent : NPCController
{
  private bool _inactive;
  private bool _canTalk;
  private bool _isTalking;
  void Start()
  {
    _inactive = false;
    itemController = FindFirstObjectByType<ItemController>();
    playerController = FindFirstObjectByType<PlayerController>();
    canvasController = FindFirstObjectByType<CanvasController>();
    dialogueTrigger = FindFirstObjectByType<DialogueTrigger>();
    dialogueManager = FindFirstObjectByType<DialogueManager>();
  }

  void Update()
  {
    if (InputHandling.CheckInteract() && _canTalk && !_inactive)
    {
      canvasController.OpenDialogueBox();
      if (!_isTalking)
      {
        Dialogues = JsonConvert.DeserializeObject<DialogueCollection>(NPCDialogue.text).CollectionToQueue();
        _isTalking = true;
        playerController.Talking = true;
      }
      _isTalking = DisplayDialogue();
      if (!_isTalking)
      {
        _inactive = true;
        itemController.SetSpecialItem(SpecialItems.Crowbar);
      }
    }
  }

  void FixedUpdate()
  {
    _canTalk = CheckPlayerOverlap();
    // if(!_canTalk) playerController.Talking = false;
  }
}
