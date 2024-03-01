using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    public int sheepGroupSize = 6;
    private int _countSave;
    
    //para evitar el collider multiples veces d ela misma Oveja.
    private HashSet<AllyModel> _sheepSaved = new HashSet<AllyModel>();

    private void OnTriggerEnter(Collider other)
    {
        AllyModel sheep = other.GetComponent<AllyModel>();


        if (sheep != null && !_sheepSaved.Contains(sheep))
        {

            sheep.IsStop = true;
            // AUmenta el contador de Ovejas.
            _countSave++;
            _sheepSaved.Add(sheep);

            // Chequea si se alcanzó el objetivo de Ovejas.
            if (_countSave >= sheepGroupSize)
            {
                // Añade puntos el playerScore.
                GameManager.Instance.WinGame();
                _countSave = 0;
            }

            // Frena el movimiento de las ovejas.
           
            

        }
    }
}