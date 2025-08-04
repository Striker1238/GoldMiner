using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string text;
    public Sprite portrait;
    public float delayBeforeContinue = 0.5f;
    public IDialogueAction dialogueAction;
}
