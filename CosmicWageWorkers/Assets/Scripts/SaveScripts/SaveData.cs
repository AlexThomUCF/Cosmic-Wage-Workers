using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public List<string> completedInteractionIDs = new();
    public int miniGameCount;
}