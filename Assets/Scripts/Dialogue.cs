using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

[System.Serializable]
public record Dialogue
{
  public string NPCName { get; set; }
  public string[] Sentences { get; set; }
  public Dialogue(string npcName, string[] sentences)
  {
    NPCName = npcName;
    Sentences = sentences;
  }
};
