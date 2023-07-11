using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerModel : BaseModel
 {
    public List<GameObject> _allies = new List<GameObject>();
    public int lifes;
    public float _mouseSensibilty = 100;
    public Action<int> OnTakeDamage;

    public void AddAlly(Collider[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider curr = colliders[i];
            print(curr);
            if (curr != null)
            {
                if (!_allies.Contains(curr.gameObject))
                {
                    _allies.Add(curr.gameObject);
                }
            }

        }
    }
    public void TakeLife()
    {

        if (OnTakeDamage != null && lifes >= 1)
        {
            //_rb.AddForce(Vector3.up * 200 + (-GetForward * 200));
            lifes--;
            OnTakeDamage(lifes);
            if (lifes < 1)
            {
                OnDie();
            }

        }


    }


    public override void LookDir(Vector3 dir)
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensibilty * Time.deltaTime; //TODO: SACAR HARCODIADO
        gameObject.transform.Rotate(dir, mouseX);
    }



    //[CustomEditor(typeof(PlayerModel))]
    //public class PlayerModelTool : Editor
    //{
    //    public override void OnInspectorGUI()
    //    {
    //        base.OnInspectorGUI();
    //        PlayerModel pModel = (PlayerModel)target;
    //        if (GUILayout.Button("Take DMG"))
    //        {
    //            pModel.TakeLife();
    //            if (pModel.lifes < 1)
    //            {
    //                //pModel.lifes = 3;
    //            }

    //        }



    //    }
    //}

}