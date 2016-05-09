﻿using UnityEngine;
using System.Collections;
using JG = Jai.Graphics;

[ExecuteInEditMode]
public class GradientDemo : MonoBehaviour {

    public JG.Gradient gradient;

    [Range(0, 1)]
    public float evaluateTime;
    public Color evaluateColor;

    public void Evaluate()
    {
       evaluateColor = gradient.Evaluate(evaluateTime);
    }

    void Update(){
      Evaluate();
    }
}
