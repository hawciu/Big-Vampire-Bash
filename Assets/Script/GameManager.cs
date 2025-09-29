using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public GameObject GetPlayer()
    {
        return player;
    }
}