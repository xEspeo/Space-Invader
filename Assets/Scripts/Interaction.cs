using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interaction : Interactable
{
    [SerializeField] private UnityEvent onHit;
    protected override void Interact()
    {
        onHit.Invoke();
    }
}
