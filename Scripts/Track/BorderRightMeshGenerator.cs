using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using System;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class BorderRightMeshGenerator : MonoBehaviour
{   
    public Mesh mesh
    {
        set
        {
            GetComponent<MeshFilter>().mesh = value;
        }
    }

}
