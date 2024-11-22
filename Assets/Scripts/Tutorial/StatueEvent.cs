using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class StatueEvent : NPCController
{
  private bool _inactive;
  private bool _canTalk;
  private bool _isTalking;
  private PlayerController playerController;
  void Start()
  {
    _inactive = false;
    playerController = FindFirstObjectByType<PlayerController>();
  }

  void Update()
  {
    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton0)) && _canTalk && !_inactive)
    {
      FindObjectOfType<CanvasController>().OpenDialogueBox();
      if (!_isTalking)
      {
        Dialogues = JsonConvert.DeserializeObject<DialogueCollection>(NPCDialogue.text).CollectionToQueue();
        _isTalking = true;
        FindObjectOfType<PlayerController>().Talking = true;
      }
      _isTalking = DisplayDialogue();
      if (!_isTalking)
      {
        _inactive = true;
        playerController.SetSpecialItem(SpecialItems.Sword);
      }
    }
  }

  void FixedUpdate()
  {
    _canTalk = CheckPlayerOverlap();
  }
}
