using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DodecagonCreate : MonoBehaviour
{
    //mesh properties
    Mesh inner;
    Mesh outer;
    public GameObject outerDodecagon;
    public Vector3[] polygonPoints;
    List<Vector3> innerPoints;
    List<Vector3> outerPoints;
    public int[] polygonTriangles;

    public void SetUp()
    {
        inner = this.GetComponent<MeshFilter>().mesh;
        outer = outerDodecagon.GetComponent<MeshFilter>().mesh;
        innerPoints = new List<Vector3>(inner.vertices).GetRange(0, 12);
        outerPoints = new List<Vector3>(outer.vertices).GetRange(0, 12);
        List<Vector3> outerPoints2 = CaculateDistance(innerPoints, outerPoints);
        Connectpolygon(outerPoints2, innerPoints);
    }

    void Update()
    {
        if (outerPoints.Count > 0)
        {
            List<Vector3> outerPoints2 = CaculateDistance(innerPoints, outerPoints);
            Connectpolygon(outerPoints2, innerPoints);
        }
    }
    List<Vector3> CaculateDistance(List<Vector3> inner,List<Vector3> outer)
    {
        List<Vector3> listDistance = new List<Vector3>();
        for (int i = 0;i<12;i++)
        {
            listDistance.Add(innerPoints[i]+(outerDodecagon.transform.TransformPoint(outer[i]) - this.transform.TransformPoint(inner[i])));
        }
        return listDistance;
    }

    void Connectpolygon(List<Vector3> outerPoints, List<Vector3> innerPoints)
    {
        List<Vector3> pointsList = new List<Vector3>();
        pointsList.AddRange(outerPoints);
        pointsList.AddRange(innerPoints);
        polygonPoints = pointsList.ToArray();

        polygonTriangles = DrawHollowTriangles(polygonPoints);
        inner.Clear();
        inner.vertices = polygonPoints;
        inner.triangles = polygonTriangles;
    }
    int[] DrawHollowTriangles(Vector3[] points)
    {
        int sides = points.Length / 2;

        List<int> newTriangles = new List<int>();
        for (int i = 0; i < sides; i++)
        {
            int outerIndex = i;
            int innerIndex = i + sides;
           
            //first triangle starting at outer edge i
            newTriangles.Add(outerIndex);
            newTriangles.Add(innerIndex);
            newTriangles.Add((i + 1) % sides);

            //second triangle starting at outer edge i
            newTriangles.Add(outerIndex);
            newTriangles.Add(sides + ((sides + i - 1) % sides));
            newTriangles.Add(outerIndex + sides);
        }
        return newTriangles.ToArray();
    }

    List<Vector3> GetCircumferencePoints(int sides, float radius)
    {
        List<Vector3> points = new List<Vector3>();
        float circumferenceProgressPerStep = (float)1 / sides;
        float TAU = 2 * Mathf.PI;
        float radianProgressPerStep = circumferenceProgressPerStep * TAU;

        for (int i = 0; i < sides; i++)
        {
            float currentRadian = radianProgressPerStep * i;
            points.Add(new Vector3(Mathf.Cos(currentRadian) * radius, Mathf.Sin(currentRadian) * radius, 0));
        }
        return points;
    }

}
