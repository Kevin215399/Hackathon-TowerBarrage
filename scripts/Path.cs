using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Path : MonoBehaviour
{
    public static Path Instance { get; private set; }
    [SerializeField] public Vector3[] path;
    [SerializeField] private LineRenderer lineRenderer;
    private MeshCollider lineCollider;
    private void Update()
    {
        Instance = this;
        lineRenderer.useWorldSpace = false;

        lineRenderer.positionCount = (path.Length);
        lineRenderer.SetPositions(path);

    

    }



}
