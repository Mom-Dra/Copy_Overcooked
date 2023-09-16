using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Player player;

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public void OnMove(InputValue value) // Arrow
    {
        Vector2 input = value.Get<Vector2>();
        player.SetMoveDirection(new Vector3(input.x, 0f, input.y));
    }

    public void OnGrabAndPut() // Space 
    {
        player.GrabAndPut();
    }

    public void OnInteractAndThrow() // Ctrl
    {
        player.OnInteractAndThrow();
    }

    public void OnDash() // Alt
    {
        player.Dash();
    }
}
