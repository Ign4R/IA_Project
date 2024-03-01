using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    public int sheepGroupSize = 6;
    private int _countSave;
    public Light _light;
    
    //para evitar el collider multiples veces d ela misma Oveja.
    private HashSet<AllyModel> _sheepSaved = new HashSet<AllyModel>();

    private void OnTriggerEnter(Collider other)
    {
        AllyModel sheepM = other.GetComponent<AllyModel>();
        AllyView sheepV = other.GetComponent<AllyView>();

        if (sheepM != null && sheepV!=null)
        {
            if (sheepM._leaders.Count == 1 && !_sheepSaved.Contains(sheepM))
            {
                sheepV.ChangeColor(Color.white);
                sheepM.IsStop = true;
                sheepM.HasLeader = false;
                // AUmenta el contador de Ovejas.
                _countSave++;
                _sheepSaved.Add(sheepM);

                // Chequea si se alcanzó el objetivo de Ovejas.
                if (_countSave >= sheepGroupSize)
                {
                    _light.color = Color.green;
                    _light.intensity = 3;
                    // Añade puntos el playerScore.
                    //GameManager.Instance.WinGame();
                    //_countSave = 0;
                }

                // Frena el movimiento de las ovejas.
            }
        }
  
    }
}