using System;
using System.Collections.Generic;
using UnityEngine;

public class RandomWheelSelection<T> :EntityStateBase<T>
{
    public bool _selectDone;
    public bool _startRWS;
    public AllyStates Result { get; private set; }
    AllyModel _sheepM;
    Dictionary<AllyStates, float> _randomWS = new Dictionary<AllyStates, float>();


    public override void InitializedState(BaseModel model, BaseView view, FSM<T> fsm)
    {
        base.InitializedState(model, view, fsm);
        _sheepM = (AllyModel)model;
        _randomWS.Add(AllyStates.Scared, 1);
        _randomWS.Add(AllyStates.Die, 0);
        _randomWS.Add(AllyStates.Aimless, 1);



    }
    public override void Awake()
    {       
        base.Awake();
        CalculatedDynamicWeight();
        Result = MyRandoms.Roulette(_randomWS);
        _selectDone = true;
    }
    public override void Sleep()
    {
        base.Sleep();
    }

    public void CalculatedDynamicWeight()
    {    
        _randomWS[AllyStates.Scared] = _sheepM._affinityW;
        _randomWS[AllyStates.Aimless] = _sheepM._leaveW;
        _randomWS[AllyStates.Die] = _sheepM._dieW;

    }
}