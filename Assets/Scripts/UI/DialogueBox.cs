using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace UI
{
public class DialogueBox : MonoBehaviour
{
    [SerializeField] private Conversation[] conversations;
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private float charAppearDelay = 0.05f;
    [SerializeField] private Sprite playerPortrait;
    [SerializeField] private Sprite bossPortrait;
    [SerializeField] private Sprite nonePortrait;
    [Header("UI Elements")]
    [SerializeField] private Text dialogueText;
    [SerializeField] private Image portrait;
    
    private int _currentConversationIndex;
    private CanvasGroup _canvasGroup;
    private float _timer;

    public void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        HideDialogueBox();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (!(_timer >= 1.1f) || !playOnStart) return; // scene transition time
        
        StopAllCoroutines();
        StartCoroutine(PlayNextConversation());
            
        _timer = 0f;
        playOnStart = false;
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
            Dialogue.Character.Boss   => bossPortrait,
            Dialogue.Character.None   => nonePortrait,
            _ => nonePortrait
        };
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
}