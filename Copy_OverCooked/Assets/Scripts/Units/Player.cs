using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Unit
{
    [SerializeField]
    protected Vector3 _handPos;
    [SerializeField]
    public IObject hand;

    [SerializeField]
    protected float distance;



    public void Hand(IObject ob)
    {
        if (ob == null)
        {
            if (hand != null)
                hand.gameObject.layer = LayerMask.NameToLayer("Interactable");
            hand = null;
        }
        else
        {
            hand = ob;
            hand.gameObject.layer = LayerMask.NameToLayer("Hand");
        }

    }
}
