using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] Transform pointPrefab;
    [SerializeField, Range(10, 100)] int resolution = 10;
    [SerializeField, Range(1, 90)] int rotationScale = 45; 

    private Transform[] points;

    void Awake () {
		var position = Vector3.zero;
        float step = 2f / resolution;
		var scale = Vector3.one * step;
        points = new Transform[resolution];

        Transform point;
		for (int i = 0; i < points.Length; i++) {
			point = Instantiate(pointPrefab);
            points[i] = point;
			position.x = (i + 0.5f) * step - 1f;
			point.localPosition = position;
			point.localScale = scale;
            point.SetParent(transform, false);
		}
	}

    // Update is called once per frame
    void Update()
    {
        Transform point;
        Vector3 position;
        float time = Time.time;
        for (int i = 0; i < points.Length; i++) {
			point = points[i];
            position = point.localPosition;
            position.y = Mathf.Sin(Mathf.PI * (position.x + time));

            // y position is between -1 and 1
            // take y position, add 1 (now between 0 and 2) and div by 2 (now between 0 and 1)
            // rotation should be between 0 and 45 degrees with rotationScale == 45
            point.localEulerAngles = (((position.y + 1) / 2) * rotationScale) * Vector3.forward;
            point.localPosition = position;
		}
    }
}
