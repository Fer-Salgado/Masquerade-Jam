using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogues")]
public class NPC_dialogue : ScriptableObject
{
    public DialogueLine[] dialogueLines;

    public string playerName;
    public Sprite playerPortrait;

    public string npcName;
    public Sprite npcPortrait;
    //public string[] dialogueLines;
    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f;
    public float typingSpeed = 0.05f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;

    
   
}
