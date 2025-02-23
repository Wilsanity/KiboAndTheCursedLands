using UnityEngine;

public class PlayerData : ScriptableObject
{
    public float health;

    public PlayerData(float h)
    {
        health = h;
    }
}
