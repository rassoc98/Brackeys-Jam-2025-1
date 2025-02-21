using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace UI
{
public class DialogueBox : MonoBehaviour
{
    [SerializeField] private Conversation[] conversations;
    [SerializeField] private float charAppearDelay = 0.5f;
    [SerializeField] private Sprite playerPortrait;
    [SerializeField] private Sprite bossPortrait;
    [Header("UI Elements")]
    [SerializeField] private Text dialogueText;
    [SerializeField] private Image portrait;
    
    private int _currentConversationIndex;
    private CanvasGroup _canvasGroup;

    public void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        HideDialogueBox();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StopAllCoroutines();
            StartCoroutine(PlayNextConversation());
        }
    }

    public IEnumerator PlayNextConversation()
    {
        foreach (var dialogue in conversations[_currentConversationIndex].dialogues)
        {
            yield return PlayDialogue(dialogue);
            yield return WaitForInput(KeyCode.Return);
        }
        
        _currentConversationIndex++;
        HideDialogueBox();
    }

    private IEnumerator PlayDialogue(Dialogue dialogue)
    {
        dialogueText.text = string.Empty;
        SetPortrait(dialogue.character);
        ShowDialogueBox();
        
        var text = dialogue.text;
        
        while (text != string.Empty)
        {
            dialogueText.text += text[0];
            text = text[1..];
            yield return new WaitForSeconds(charAppearDelay);
        }
    }

    private IEnumerator WaitForInput(KeyCode keyCode)
    {
        while (!Input.GetKeyDown(keyCode))
        {
            yield return null;
        }
    }

    private void SetPortrait(Dialogue.Character character)
    {
        portrait.sprite = character switch
        {
            Dialogue.Character.Player => playerPortrait,
            Dialogue.Character.Boss => bossPortrait,
            Dialogue.Character.None => null,
            _ => null
        };

        portrait.color = portrait.sprite == null ? Color.clear : Color.white;
    }

    private void ShowDialogueBox()
    {
        _canvasGroup.alpha = 1;
    }

    private void HideDialogueBox()
    {
        _canvasGroup.alpha = 0;
    }
}
}

[Serializable]
internal class Conversation
{
    public Dialogue[] dialogues;
}

[Serializable]
internal class Dialogue
{
    public Character character;
    public string text;

    public enum Character
    {
        None,
        Player,
        Boss
    }
}