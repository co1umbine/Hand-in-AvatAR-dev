using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionPredictor
{
    private Transform target;
    private Vector3 privPosition;
    public Vector3 Predict
    {
        get;
        private set;
    }

    public PositionPredictor(Transform target)
    {
        this.target = target;
        privPosition = target.position;
    }
    public Vector3 Update()
    {
        Predict = Vector3.LerpUnclamped(privPosition, target.position, 2);
        this.privPosition = target.position;
        return Predict;
    }

    
}
