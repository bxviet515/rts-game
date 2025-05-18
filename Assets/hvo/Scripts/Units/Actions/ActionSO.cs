using UnityEngine;
public abstract class ActionSO : ScriptableObject
{
    public Sprite Icon;
    public string ActioneName;
    public string Guid = System.Guid.NewGuid().ToString();
    public abstract void Execute(GameManager gameManager);
}