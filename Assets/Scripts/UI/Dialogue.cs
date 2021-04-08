using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;

    public DialogueTalkSprites talkSprites;

    [TextArea(3,10)]
    public string sentence;

    public bool allowButtons;
    [Range(1,3)]
    public int amountOfButtons;
}
