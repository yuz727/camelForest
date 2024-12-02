using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Numerics;

public class L4RanaTrigger : NPCController
{
  public GameObject Floor;
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
      if (_isTalking) return;
      playerController.CanDash = true;
      Floor.SetActive(false);
      gameObject.SetActive(false);
      return;
    }
  }

  void FixedUpdate()
  {
    _canTalk = CheckPlayerOverlap();
  }
}
