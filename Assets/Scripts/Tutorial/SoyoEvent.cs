using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;

public class SoyoEvent : NPCController
{
  public bool StartRepeat = false;
  private bool _canTalk;
  private bool _isTalking;


  private readonly Dialogue _repeatDialogue = new()
  {
    NPCName = "Veggie",
    Sentences = new string[] { "Make sure to bring Camel Back." }
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
        FindObjectOfType<PlayerController>().AddItem(Items.Cucumber);
      }
    }
  }
  void FixedUpdate()
  {
    _canTalk = CheckPlayerOverlap();

  }
}
