using UnityEngine;
using UnityEngine.Events;

public abstract class GamePopUp : MonoBehaviour
{
    public delegate void ButtonEvent(int index); 
    public abstract void Init(GameData _gameData);
    public abstract void Display();
    public abstract void Hide();
    public abstract void SetAction(UnityAction _callback);
}