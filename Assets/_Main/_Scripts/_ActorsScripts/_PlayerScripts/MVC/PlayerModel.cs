using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerModel : BaseModel
 {
    public int maxAlliesAdd = 3;
    [ReadOnly] [SerializeField] private int allies;
    public int lifes;
    public float _mouseSensibilty = 100;
    public Action<int> OnTakeDamage;
    private Color leadColor = Color.cyan;


    public void TakeLife()
    {

        if (OnTakeDamage != null && lifes >= 1)
        {
            //_rb.AddForce(Vector3.up * 200 + (-GetForward * 200));
            //lifes--;
            OnTakeDamage(lifes);
            if (lifes < 1)
            {
                //OnDie();
            }

        }


    }


    public override void LookDir(Vector3 dir)
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensibilty * Time.deltaTime; //TODO: SACAR HARCODIADO
        gameObject.transform.Rotate(dir, mouseX);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 13)
        {
            AllyModel sheep = other.GetComponent<AllyModel>();
            List<Transform> allyLeaders = sheep._leaders;

            if (!allyLeaders.Contains(transform) && GameManager.Instance.CountSheep < maxAlliesAdd) 
            {
                allyLeaders.Add(transform);
                sheep.HasLeader = true;
                sheep.ColorFollow = leadColor;
            }


        }
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