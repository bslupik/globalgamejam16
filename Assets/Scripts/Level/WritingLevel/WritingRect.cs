using UnityEngine;
using System.Collections;

public class WritingRect : MonoBehaviour {
    [SerializeField]
    protected Vector2 start;
    [SerializeField]
    protected Vector2 end;
    [SerializeField]
    protected float width;
    [SerializeField]
    protected int numSegments;
    [SerializeField]
    protected GameObject sectionPrefab;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < numSegments; i++)
        {
            GameObject section = Instantiate(sectionPrefab);
            section.GetComponent<WritingSection>().Initialize(Vector3.Lerp(start, end, (float)i / numSegments), Vector3.Lerp(start, end, ((float)i + 1) / numSegments), width);
        }
	}
}
