using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class ShroomEvent : NPCController
{
  public bool StartRepeat = false;
  private bool _canTalk;
  private bool _isTalking;

  private readonly Dialogue _repeatDialogue = new()
  {
    NPCName = "Shroom",
    Sentences = new string[] { "Don't Worry!", "It's Free!" }
  };

  void Update()
  {
    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton0)) && _canTalk)
    {
      FindObjectOfType<CanvasController>().OpenDialogueBox();
      if (!_isTalking)
      {
        if (StartRepeat)
        {
          Dialogues = new Queue<Dialogue>();
          Dialogues.Enqueue(_repeatDialogue);
        }
        else
        {
          Dialogues = JsonConvert.DeserializeObject<DialogueCollection>(NPCDialogue.text).CollectionToQueue();

        }
        _isTalking = true;
        FindObjectOfType<PlayerController>().Talking = true;
      }
      _isTalking = DisplayDialogue();
      if (_isTalking)
      {
        return;
      }
      if (!StartRepeat)
      {
        StartRepeat = true;
      }
    }
  }

  void FixedUpdate()
  {
    _canTalk = CheckPlayerOverlap();
  }
}
