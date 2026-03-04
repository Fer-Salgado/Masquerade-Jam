using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public NPC_dialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    [Header("Sistema de Amistad")]
    public int friendshipLevel = 0;
    public Sprite[] friendshipSprites;
    public Image friendshipIndicatorUI;

    private int dialogueIndex = 0;
    private bool isTyping, isDialogueActive;

    public int lineasDichas = 0;
    public int pauseAt;
    public bool isInMinigame = false;

    public NPC_dialogue wellStageTwo;
    public NPC_dialogue piperStageTwo;
    public NPC_dialogue piperStageThree;
    public NPC_dialogue gayStageTwo;
    public NPC_dialogue gayStageThree;

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null || (PauseController.isGamePaused && !isDialogueActive))
            return;

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            UpdateFriendshipUI();
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        PauseController.SetPause(true);
        StartCoroutine(Typeline());
    }

    void UpdateFriendshipUI()
    {
        if (friendshipIndicatorUI == null || friendshipSprites.Length < 4) return;

        friendshipIndicatorUI.gameObject.SetActive(true);
        friendshipIndicatorUI.sprite = friendshipSprites[friendshipLevel];
    }

    void NextLine()
    {
        if (isInMinigame) return;

        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex].text);
            isTyping = false;
            return;
        }

        if (lineasDichas == pauseAt)
        {
            StartCoroutine(MiniGame());
            return;
        }

        if (++dialogueIndex < dialogueData.dialogueLines.Length)
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

        DialogueLine line = dialogueData.dialogueLines[dialogueIndex];
        if (line.speaker == Speaker.NPC)
        {
            nameText.text = dialogueData.npcName;
            portraitImage.sprite = dialogueData.npcPortrait;
        }
        else
        {
            nameText.text = dialogueData.playerName;
            portraitImage.sprite = dialogueData.playerPortrait;
        }

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex].text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
        ++lineasDichas;
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        PauseController.SetPause(false);

        if (friendshipIndicatorUI != null)
            friendshipIndicatorUI.gameObject.SetActive(false);

        if (friendshipLevel < 3)
        {
            friendshipLevel++;
        }
    }

    IEnumerator MiniGame()
    {
        if (isInMinigame) yield break;
        isInMinigame = true;
        isDialogueActive = true;
        dialoguePanel.SetActive(false);
        PauseController.SetPause(false);
        yield return new WaitForSeconds(5f);
        isInMinigame = false;
        PauseController.SetPause(true);
        dialoguePanel.SetActive(true);
        StartCoroutine(Typeline());
    }
}