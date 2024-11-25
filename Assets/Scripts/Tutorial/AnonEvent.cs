using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class AnonEvent : NPCController
{
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

  }
}
