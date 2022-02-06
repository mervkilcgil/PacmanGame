using System;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject gridPrefab;
    int columLength;
    int rowLength;
    float xSpace = 4f, ySpace= 4f;

    public void Start()
    {
        var rect = GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        columLength = (int)(Mathf.Abs(corners.Max(x => x.x) - corners.Min(x => x.x)) / xSpace);
        rowLength = (int)(Mathf.Abs(corners.Max(x => x.y) - corners.Min(x => x.y)) / ySpace);
        for (int i = 0; i < columLength*rowLength; i++)
        {
            Instantiate(gridPrefab, new Vector2(xSpace*(i%columLength), ySpace*(i/columLength)), Quaternion.identity);
        }
        
    }
}