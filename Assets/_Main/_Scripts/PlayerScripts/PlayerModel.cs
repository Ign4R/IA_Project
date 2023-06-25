using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerModel : BaseModel
 {
     public float lifes;
     public float _mouseSensibilty = 100;
     public Action OnTakeDamage;

     
     // ReSharper disable Unity.PerformanceAnalysis
     public void TakeLife()
     {
       
         if (OnTakeDamage != null && lifes>=1)
         {         
             print("daño");
             //_rb.AddForce(Vector3.up * 200 + (-GetForward * 200));
             OnTakeDamage();
             lifes--;
             if (lifes < 1) 
             {
                 IsDie = true;
                 OnDie();
             }
 
         }
 
   
     }
     private void Update()
     {
        
     }
     public override void LookDir(Vector3 dir)
     {
         float mouseX = Input.GetAxis("Mouse X") * _mouseSensibilty * Time.deltaTime; //TODO: SACAR HARCODIADO
         gameObject.transform.Rotate(dir, mouseX);
     }
 
     [CustomEditor(typeof(PlayerModel))]
     public class PlayerModelTool : Editor
     {
         public override void OnInspectorGUI()
         {
             base.OnInspectorGUI();
             PlayerModel pModel = (PlayerModel)target;
             if (GUILayout.Button("Take DMG"))
             {
                 pModel.TakeLife();
                 if (pModel.lifes < 1)
                 {
                     pModel.lifes = 3;
                 }
 
             }
 
 
 
         }
     }
 }