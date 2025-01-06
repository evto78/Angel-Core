using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    LineRenderer lr;
    public Transform firePoint;

    List<Vector3> linePoints = new List<Vector3>();
    float lineTimer;

    public float atkSpd;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {


        if (lineTimer > 0f) { lineTimer -= Time.deltaTime * atkSpd; if (lineTimer < 0f) { lineTimer = 0f; } }
        lr.startWidth = lineTimer;
        lr.endWidth = lineTimer;

        if (Input.GetMouseButtonUp(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        lr.positionCount = 1;
        lr.SetPosition(0, firePoint.position);
        linePoints.Clear();
        linePoints.Add(firePoint.position);

        Vector3 direction = firePoint.forward;

        if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, float.MaxValue))
        {

            linePoints.Add(hit.point);

            RenderLine();

        }
        else
        {
            linePoints.Add(firePoint.position + direction * 9999);
            RenderLine();
        }
    }

    void RenderLine()
    {
        lineTimer = 0.3f;

        lr.positionCount = linePoints.Count;

        for (int i = 0; i < linePoints.Count; i++)
        {
            Debug.DrawLine(linePoints[i], linePoints[i] + Vector3.up * 3f, Color.yellow, 10f);
            lr.SetPosition(i, linePoints[i]);
        }
    }
}
