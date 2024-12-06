using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RikiEvent : NPCController
{
  private bool _canTalk;
  public Animator Anim;
  private bool _inActive;
  void Start()
  {
    playerController = FindFirstObjectByType<PlayerController>();
    canvasController = FindFirstObjectByType<CanvasController>();
    dialogueTrigger = FindFirstObjectByType<DialogueTrigger>();
    dialogueManager = FindFirstObjectByType<DialogueManager>();
    itemController = FindFirstObjectByType<ItemController>();
    Anim = GetComponent<Animator>();
  }
  void Update()
  {
    if (InputHandling.CheckInteract() && _canTalk && !_inActive)
    {
      StartCoroutine(Red1());
      Anim.SetBool("isRed1", true);
      _inActive = true;
    }
  }

  void FixedUpdate()
  {
    _canTalk = CheckPlayerOverlap();
  }
  IEnumerator Red1()
  {
    yield return new WaitForSeconds(0.4f);
    Anim.SetBool("isRed1", false);
    Anim.SetBool("isRed2", true);
    StartCoroutine(Red2());
  }
  IEnumerator Red2()
  {
    yield return new WaitForSeconds(0.4f);
    Anim.SetBool("isRed2", false);
    itemController.SetSpecialItem(SpecialItems.Dynamite);
  }
}


