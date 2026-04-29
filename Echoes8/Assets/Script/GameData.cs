using UnityEngine;

[System.Serializable]
public class GameData
{
    public int loopCount;
    public float completionTime;

    public GameData()
    {
        loopCount = 0;
        completionTime = 0f;
    }
}
