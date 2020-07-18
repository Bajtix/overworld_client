using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimator : ItemReactor
{
    public Animator item;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && Cursor.lockState == CursorLockMode.Locked)
        {
            Use();
        }
    }

    public void Use()
    {
        item.SetTrigger("use");
    }
}
