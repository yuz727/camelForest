using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class AnonEvent : NPCController
{
  private bool _canTalk;
  private bool _isTalking;
  public bool StartRepeat;
  private Dialogue[] _dialogues;
  void Start()
  {

    playerController = FindFirstObjectByType<PlayerController>();
    canvasController = FindFirstObjectByType<CanvasController>();
    dialogueTrigger = FindFirstObjectByType<DialogueTrigger>();
    dialogueManager = FindFirstObjectByType<DialogueManager>();
    itemController = null;
    StartRepeat = false;
    _dialogues = new Dialogue[] {
      new(){
        NPCName = "London",
        Sentences = new string[] { "Do you know Duck has a Bow in his house?", "I wonder what he needs that for...", "Not like there is anything nearby that is hostile...", }
      }, new(){
        NPCName = "London",
        Sentences = new string[] { "Blacksmith's has some dynmaite that you might be able to use...", "Shiitake always looks like she is going to blow up..." }
      }, new(){
        NPCName = "London",
        Sentences = new string[] { "There's also some statue to the west of the village...", "Such a long walk though..." }
      }, new(){
        NPCName = "London",
        Sentences = new string[] { "Duck has some metal looking thing that he said he got for you.", "Apparently there's some catch so I'd check with him..." }
      }, new(){
        NPCName = "London",
        Sentences = new string[] { "There are some hidden caves around the village.", "One of them even have mushrooms in it!" }
      },
    };
  }

  void Update()
  {
    if (InputHandling.CheckInteract() && _canTalk)
    {
      canvasController.OpenDialogueBox();
      if (!_isTalking)
      {
        if (StartRepeat)
        {
          Dialogues = new Queue<Dialogue>();
          Dialogues.Enqueue(_dialogues[Random.Range(0, _dialogues.Length)]);
        }
        else
        {
          Dialogues = JsonConvert.DeserializeObject<DialogueCollection>(NPCDialogue.text).CollectionToQueue();
        }
        _isTalking = true;
        playerController.Talking = true;
      }
      _isTalking = DisplayDialogue();
      if (_isTalking) return;
      if (!StartRepeat) StartRepeat = true;
    }
  }

  void FixedUpdate()
  {
    _canTalk = CheckPlayerOverlap();
  }
}
