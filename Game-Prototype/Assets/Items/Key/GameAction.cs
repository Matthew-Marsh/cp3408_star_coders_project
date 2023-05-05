using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

//Base class for actions
public abstract class GameAction : MonoBehaviour
{
    public abstract void Activated();
}

//Base Class used to activate GameAction when a specific action is done
public abstract class GameTrigger : MonoBehaviour
{
    public GameAction[] actions;

    public void Trigger()
    {
        foreach (GameAction g in actions)
            g.Activated();
    }
}