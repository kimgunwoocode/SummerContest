using UnityEngine;
using System.Collections.Generic;


public class WaterWave : MonoBehaviour {
    [SerializeField] int columnCount = 10;
    [SerializeField] float width = 2f;
    [SerializeField] float height = 1f;
    [SerializeField] float k = 0.025f;
    [SerializeField] float m = 1f;
    [SerializeField] float drag = 0.025f;
    [SerializeField] float spread = 0.025f;
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] Collider2D anyCollider;

    private List<WaterColumn> columns = new List<WaterColumn>();

    private void Awake() {
        InitCols();
    }

    private void InitCols() {
        columns.Clear();
        float space = width / columnCount;
        for (int i = 0; i < columnCount; i++) {
            columns.Add(new WaterColumn(i * space - width * 0.5f, height, k, m, drag));
        }
    }

    internal int? WorldToColumn(Vector2 position) {
        float space = width / columnCount;
        float localPosX = transform.InverseTransformPoint(position).x;
        int result = Mathf.RoundToInt((localPosX + width * 0.5f) / space);
        Debug.Log(result);
        if (result >= columns.Count || result < 0)
            return null;
        return result;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log(collision.transform.position);
        int? column = WorldToColumn(collision.transform.position);
        if (column.HasValue) {
            columns[column.Value].velocity = 0.6f;
        }
    }

    private void Update() {
        /*int? column = WorldToColumn(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Debug.Log("m :" + Input.GetMouseButtonDown(0));
        Debug.Log("Hv:" + column.HasValue);
        if (Input.GetMouseButtonDown(0) && column.HasValue)
            columns[column.Value].velocity = -1f;*/
    }

    private void FixedUpdate() {
        for (int i = 0; i < columns.Count; i++) {
            columns[i].UpdateColumn();
        }

        float[] leftDelta = new float[columns.Count];
        float[] rightDelta = new float[columns.Count];
        for (int i = 0; i < columns.Count; i++) {
            if (i > 0) {
                leftDelta[i] = (columns[i].height - columns[i - 1].height) * spread;
                columns[i - 1].velocity += leftDelta[i];
            }

            if (i < columns.Count - 1) {
                rightDelta[i] = (columns[i].height - columns[i + 1].height) * spread;
                columns[i + 1].velocity += rightDelta[i];
            }
        }

        for (int i = 0; i < columns.Count - 1; i++) {
            if (i > 0)
                columns[i - 1].height += leftDelta[i];
            if (i > 0)
                columns[i + 1].height += rightDelta[i];
        }

        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[columns.Count * 2];
        int v = 0;
        for (int i = 0; i < columns.Count; i++) {
            vertices[v] = new Vector2(columns[i].xPos, columns[i].height);
            vertices[v + 1] = new Vector2(columns[i].xPos, 0);

            v += 2;
        }

        int[] triangles = new int[(columns.Count - 1) * 6];
        int t = 0;
        v = 0;
        for (int i = 0; i < columns.Count - 1; i++) {
            triangles[t] = v;
            triangles[t + 1] = v + 2;
            triangles[t + 2] = v + 1;
            triangles[t + 3] = v + 1;
            triangles[t + 4] = v + 2;
            triangles[t + 5] = v + 3;

            v += 2;
            t += 6;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();

        meshFilter.mesh = mesh;
    }


    public class WaterColumn {
        public float xPos, height, targetHeight, k, m, velocity, drag;
        public WaterColumn(float xPos, float targetHeight, float k, float m, float drag) {
            this.xPos = xPos;
            this.height = targetHeight;
            this.targetHeight = targetHeight;
            this.k = k;
            this.m = m;
            this.drag = drag;
        }

        public void UpdateColumn() {
            float a = -k / m * (height - targetHeight);
            velocity += a;
            velocity -= drag * velocity;
            height += velocity;
        }
    }
}

