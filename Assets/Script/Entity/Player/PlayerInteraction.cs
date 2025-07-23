using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour {
    public UnityEvent interactionEvent;

    internal void OnInteraction(InputAction.CallbackContext context) {
        interactionEvent.Invoke();
    }
}
