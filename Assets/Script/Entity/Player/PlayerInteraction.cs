using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour {
    [HideInInspector]
    public Queue<UnityEvent> _interactionQueue;

    internal void OnInteraction(InputAction.CallbackContext context) {

    }

    public void DeleteInteraction() {

    }

    public void InsertInteraction() {

    }
}
