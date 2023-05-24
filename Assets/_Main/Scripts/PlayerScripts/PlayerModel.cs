using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : BaseModel
{
    public float lifes;
    public float _mouseSensibilty = 100;
    public Action OnTakeDamage;
    
    public void TakeLife()
    {
        print("daño");
        if (OnTakeDamage != null) OnTakeDamage();
        lifes--;
    }

    public override void LookDir(Vector3 dir)
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensibilty * Time.deltaTime; //TODO: SACAR HARCODIADO
        gameObject.transform.Rotate(dir, mouseX);
    }
}
