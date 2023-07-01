using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    public int pointsPerSheepGroup = 180;
    public int sheepGroupSize = 6;
    private int _sheepCount;
    
    //para evitar el collider multiples veces d ela misma Oveja.
    private HashSet<SheepModel> _sheepInWinZone = new HashSet<SheepModel>();

    private void OnTriggerEnter(Collider other)
    {
        SheepModel sheep = other.GetComponent<SheepModel>();


        if (sheep != null && !_sheepInWinZone.Contains(sheep))
        {

            sheep.IsStop = true;
            // AUmenta el contador de Ovejas.
            _sheepCount++;
            _sheepInWinZone.Add(sheep);

            // Chequea si se alcanzó el objetivo de Ovejas.
            if (_sheepCount >= sheepGroupSize)
            {
                // Añade puntos el playerScore.
                GameManager.Instance.AddGemScore(pointsPerSheepGroup);

                _sheepCount = 0;
            }

            // Frena el movimiento de las ovejas.
           
            

        }
    }
}