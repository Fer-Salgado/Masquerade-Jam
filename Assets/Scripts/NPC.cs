using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestStage
{
    public string questName;
    public int requiredItemID;
    public NPC_dialogue introDialogue;
    public NPC_dialogue waitingDialogue;
    public NPC_dialogue completeDialogue;
}

public class NPC : MonoBehaviour, IInteractable
{
    [Header("NPC Normal (Sin Misiones)")]
    public NPC_dialogue defaultDialogue;

    [Header("Progreso del NPC (Con Misiones)")]
    public int friendshipLevel = 0;
    public List<QuestStage> quests;

    [Header("UI y Componentes")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    [Header("UI de Amistad")]
    public Image friendshipUI;
    public Sprite[] friendshipSprites;

    private int dialogueIndex = 0;
    private bool isTyping, isDialogueActive;
    private bool hasIntroducedSelf = false;
    private bool hasStartedCurrentQuest = false;
    private NPC_dialogue currentDialoguePlaying;
    private InventoryController inventory;

    void Start()
    {
        inventory = FindObjectOfType<InventoryController>();
    }

    public bool CanInteract() => !isDialogueActive;

    public void Interact()
    {
        if (isDialogueActive) { NextLine(); return; }
        if (PauseController.isGamePaused) return;

        CheckDialogueState();

        if (currentDialoguePlaying != null)
        {
            StartDialogue();
        }
    }

    private void CheckDialogueState()
    {
        // 1. Si no hay misiones configuradas
        if (quests == null || quests.Count == 0)
        {
            currentDialoguePlaying = defaultDialogue;
            return;
        }

        // 2. Primera interacción: Se presenta antes de pedir nada
        if (!hasIntroducedSelf && defaultDialogue != null)
        {
            currentDialoguePlaying = defaultDialogue;
            hasIntroducedSelf = true;
            return;
        }

        // 3. Si ya completó todas las misiones
        if (friendshipLevel >= quests.Count)
        {
            currentDialoguePlaying = quests[quests.Count - 1].completeDialogue;
            return;
        }

        // 4. Ciclo de misiones
        QuestStage currentQuest = quests[friendshipLevel];

        if (!hasStartedCurrentQuest)
        {
            currentDialoguePlaying = currentQuest.introDialogue;
            hasStartedCurrentQuest = true;
        }
        else
        {
            if (inventory != null && inventory.HasItem(currentQuest.requiredItemID))
            {
                currentDialoguePlaying = currentQuest.completeDialogue;

                inventory.RemoveItem(currentQuest.requiredItemID);
                friendshipLevel++;
                hasStartedCurrentQuest = false;
            }
            else
            {
                currentDialoguePlaying = currentQuest.waitingDialogue;
            }
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;
        dialoguePanel.SetActive(true);
        PauseController.SetPause(true);

        UpdateFriendshipDisplay();

        StartCoroutine(Typeline());
    }

    void NextLine()
    {
        if (isTyping)
        {
            // Salto rápido de texto
            StopAllCoroutines();
            dialogueText.SetText(currentDialoguePlaying.dialogueLines[dialogueIndex].text);
            isTyping = false;
            return;
        }

        if (++dialogueIndex < currentDialoguePlaying.dialogueLines.Length)
        {
            StartCoroutine(Typeline());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator Typeline()
    {
        isTyping = true;
        dialogueText.SetText("");
        DialogueLine line = currentDialoguePlaying.dialogueLines[dialogueIndex];

        nameText.text = (line.speaker == Speaker.NPC) ? currentDialoguePlaying.npcName : currentDialoguePlaying.playerName;
        portraitImage.sprite = (line.speaker == Speaker.NPC) ? currentDialoguePlaying.npcPortrait : currentDialoguePlaying.playerPortrait;

        foreach (char letter in line.text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(currentDialoguePlaying.typingSpeed);
        }
        isTyping = false;
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        PauseController.SetPause(false);
    }

    private void UpdateFriendshipDisplay()
    {
        if (friendshipUI != null && friendshipSprites != null && friendshipSprites.Length > 0)
        {
            int spriteIndex = Mathf.Clamp(friendshipLevel, 0, friendshipSprites.Length - 1);
            friendshipUI.sprite = friendshipSprites[spriteIndex];
        }
    }
}