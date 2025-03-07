using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public static Action<Vector3> PlayerClicksOnField = null;

    [SerializeField] private float timeBetweenClicks = 0.2f;

    private Vector2 pointer = Vector2.zero;
    private float clicksTimer = 0;

    public void RecordPosition(InputAction.CallbackContext context) => pointer = context.ReadValue<Vector2>();

    public void Click(InputAction.CallbackContext context)
    {
        if (Time.time < clicksTimer) return;
        clicksTimer = Time.time + timeBetweenClicks;

        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(pointer);

        if (Physics.Raycast(ray, out hit) && hit.collider.tag == "Player Field")
            PlayerClicksOnField.Invoke(hit.point);
    }
}
