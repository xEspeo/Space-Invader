using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Interactable : MonoBehaviour  //Abstrakt damit man es für verschiedene Rätsel benutzen kann
{
    //Methode wird vom Spieler aufgerufen
    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {
        //Ist leer, da es überschrieben wird bei Unterklassen
    }
}

