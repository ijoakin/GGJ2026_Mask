using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public Rigidbody Rigidbody { get; set; }

    public PlayerState()
    {
    }

    [SerializeField]
    public AnimationClip PlayerAnimation;

    public virtual void OnExitState()
    {

    }

    public virtual void OnEnterState()
    {
    }

    public virtual void OnUpdateState()
    {
    }
}
