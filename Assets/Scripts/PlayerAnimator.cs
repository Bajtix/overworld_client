using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Transform handTarget;
    void Update()
    {
        handTarget.position = GetComponent<PlayerInventory>().instance.GetComponent<ItemReactor>().handTransform.position;
        handTarget.rotation = GetComponent<PlayerInventory>().instance.GetComponent<ItemReactor>().handTransform.rotation;
    }
}
