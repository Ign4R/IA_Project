using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float min = 50;
    public float max = 100;
    Dictionary<string, float> _dic = new Dictionary<string, float>();
    private void Start()
    {
        _dic["Crash"] = 100;
        _dic["Drake"] = 45;
        _dic["Sonic"] = 20;
        _dic["AntiCristPlomeroFeoSeñorM"] = 1;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //var random = MyRandoms.Range(min, max);
            //print(random.ToString());
            for (int i = 0; i < 100; i++)
            {
                var item = MyRandoms.Roulette(_dic);
                print(item);
            }
        }
    }
}
