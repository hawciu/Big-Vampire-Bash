using UnityEngine;

[CreateAssetMenu]
public class SaveObjectScriptableObject : ScriptableObject
{
    [Header("Config")]
    public bool loadedOnce = false;

    [Header("Player Choice")]
    public PlayerType PlayerChoice;

    [Header("Level Choice")]
    public string levelChoicePlaceholder;

    [Header("loaded values")]
    public string coinChoicePlaceholder;
}
