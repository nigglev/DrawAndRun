using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using System;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class BorderLeftMeshGenerator : MonoBehaviour
{
    public Mesh mesh
    {
        set
        {
            GetComponent<MeshFilter>().mesh = value;
        }
    }


}
