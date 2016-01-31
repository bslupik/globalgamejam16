using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Conway : MonoBehaviour {
    [SerializeField]
    protected float width;

    [SerializeField]
    protected float height;

    [SerializeField]
    protected int xCount;

    [SerializeField]
    protected int yCount;

    [SerializeField]
    protected float stepTime;

    ConwayNode[,] nodes;

    MeshRenderer meshRend;
    MeshFilter meshFil;

	// Use this for initialization
	void Awake () {
        meshRend = GetComponent<MeshRenderer>();
        meshFil = GetComponent<MeshFilter>();
	}

    void Start()
    {
        CreateMesh();
        StartCoroutine(DoStep());
    }

    IEnumerator DoStep()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(stepTime);
            Step();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void CreateMesh()
    {
        nodes = new ConwayNode[xCount, yCount];
        List<Vector3> verts = new List<Vector3>();
        List<Color> colors = new List<Color>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();

        for (int x = 0; x < xCount; x++)
        {
            for (int y = 0; y < yCount; y++)
            {
                float x1 = Mathf.Lerp(-width, width, ((float)x) / xCount);
                float x2 = Mathf.Lerp(-width, width, ((float)x+1) / xCount);
                float y1 = Mathf.Lerp(-height, height, ((float)y) / yCount);
                float y2 = Mathf.Lerp(-height, height, ((float)y+1) / yCount);
                Vector3 v1 = new Vector3(x1, y1);
                Vector3 v2 = new Vector3(x1, y2);
                Vector3 v3 = new Vector3(x2, y2);
                Vector3 v4 = new Vector3(x2, y1);
                /*
                Vector3 v1 = nodes[x - 1, y - 1].transform.position;//new Vector3(x1, y1, 0);
                Vector3 v2 = nodes[x - 1, y].transform.position;//new Vector3(x1, y2, 0);
                Vector3 v3 = nodes[x, y].transform.position;//new Vector3(x2, y2, 0);
                Vector3 v4 = nodes[x, y - 1].transform.position;//new Vector3(x2, y1, 0);
                */
                int v = verts.Count; //future index of v1

                nodes[x, y] = new ConwayNode();
                nodes[x, y].uvIndex = v;
                nodes[x, y].alive = Random.value > 0.6f;

                

                verts.Add(v1);
                colors.Add(Color.clear);
                uvs.Add(new Vector2(0.0f, 0.0f));

                verts.Add(v2);
                colors.Add(Color.clear);
                uvs.Add(new Vector2(0.0f, 1.0f));

                verts.Add(v3);
                colors.Add(Color.clear);
                uvs.Add(new Vector2(1.0f, 1.0f));

                verts.Add(v4);
                colors.Add(Color.clear);
                uvs.Add(new Vector2(0.0f, 1.0f));

                //upper left triangle
                tris.Add(v);
                tris.Add(v + 1);
                tris.Add(v + 2);

                //bottom right triangle
                tris.Add(v + 2);
                tris.Add(v + 3);
                tris.Add(v);
            }
        }

        Mesh m = new Mesh();
        m.vertices = verts.ToArray();
        m.uv = uvs.ToArray();
        m.colors = colors.ToArray();
        m.triangles = tris.ToArray();
        m.RecalculateBounds();
        meshFil.mesh = m;
    }

    void Step()
    {
        bool[, ] newNodeValues = new bool[xCount, yCount];
        Color[] cols = new Color[meshFil.mesh.vertexCount];

        for (int x = 0; x < xCount; x++)
        {
            for (int y = 0; y < yCount; y++)
            {
                newNodeValues[x, y] = CalcuateNewState(x, y);
            }
        }

        for (int x = 0; x < xCount; x++)
        {
            for (int y = 0; y < yCount; y++)
            {
                for (int i = 0; i < 4; i++)
                {
                    cols[i + nodes[x, y].uvIndex] = newNodeValues[x, y] ? HSVColor.HSVToRGB(Random.value / 5, 1, 1) : Color.clear;
                }
                if (Random.value > 0.995f)
                    newNodeValues[x, y] = !newNodeValues[x, y];
                nodes[x, y].alive = newNodeValues[x, y];
            }
        }
        meshFil.mesh.colors = cols;
    }

    bool CalcuateNewState(int x, int y)
    {
        int count = 0;
        count += ValueOf(x, y + 1);
        count += ValueOf(x+ 1, y + 1);
        count += ValueOf(x+1, y);
        count += ValueOf(x+1, y - 1);
        count += ValueOf(x, y - 1);
        count += ValueOf(x-1, y - 1);
        count += ValueOf(x-1, y);
        count += ValueOf(x-1, y + 1);
        if (nodes[x, y].alive)
            return count >= 2 && count <= 3;
        else
            return count == 3;
    }

    int ValueOf(int x, int y)
    {
        return nodes[(x + xCount) % xCount, (y + yCount) % yCount].alive ? 1 : 0;
    }
}

public class ConwayNode
{
    public bool alive;
    public int uvIndex;
}