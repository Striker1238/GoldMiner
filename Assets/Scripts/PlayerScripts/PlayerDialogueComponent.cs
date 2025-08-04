using System.Collections.Generic;
using TMPro;
using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDialogueComponent : MonoBehaviour
{
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private TextMeshProUGUI dialogueLineTMP;
    [SerializeField] private TextMeshProUGUI npcNameTMP;
    [SerializeField] private Image npcPortraitImage;

    private string currentNpcName;
    private Queue<DialogueLine> dialogueQueue;
    public string CurrentNpcName { get => currentNpcName; set { dialogueLineTMP.text = value;  currentNpcName = value; } }
    
    public void InitializeNewDialogue(string npcName, List<DialogueLine> dialogue)
    {
        if (dialogueUI == null || dialogue == null)
        {
            Debug.LogError("Dialogue UI or dialogueLine is not assigned.");
            return;
        }
        // Открываем UI с диалогом
        dialogueQueue = new Queue<DialogueLine>(dialogue);
        dialogueUI.SetActive(true);

        //Показываем первый месседж диалога
        ShowDialogueLine();
    }
    public void ShowDialogueLine()
    {
        // Вывод месседжа диалога
        // Переход к следующей строке диалога
        if(dialogueQueue.Count == 0)
        {
            dialogueUI.SetActive(false);
            return;
        }
        var line = dialogueQueue.Dequeue();
        dialogueLineTMP.text = line.text;

        line.dialogueAction?.Execute();

        if (line.portrait != null)
            npcPortraitImage.sprite = line.portrait;

    }



    public void ShowShop(List<ItemBase> items)
    {
        Debug.Log($"Shop items: {items}");
    }
}