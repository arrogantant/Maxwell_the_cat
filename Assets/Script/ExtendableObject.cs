using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendableObject : Interactable
{
    public float extensionLength = 0.1f;

    public override void Interact()
    {
        base.Interact();

        ExtendObject();
    }

    private void ExtendObject()
    {
        Vector3 newScale = transform.localScale;
        newScale.y += extensionLength;
        transform.localScale = newScale;
    }
}