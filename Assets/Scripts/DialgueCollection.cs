using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCollection
{
  public Dialogue[] Dialogues;

  public Queue<Dialogue> CollectionToQueue()
  {
    var newQueue = new Queue<Dialogue>();
    foreach (Dialogue item in Dialogues)
    {
      newQueue.Enqueue(item);
    }
    return newQueue;
  }

}