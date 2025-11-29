using UnityEngine;

[CreateAssetMenu]
public class SaveObjectScriptableObject : ScriptableObject
{
    [Header("Config")]
    public bool loadedOnce = true;

    [Header("Player Choice")]
    public PlayerType PlayerChoice;

    [Header("Level Choice")]
    public LevelType levelChoice;

    [Header("loaded values")]
    public int coinAmount;
}
