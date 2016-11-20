using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowAnim : MonoBehaviour {
    [SerializeField]
    LineRenderer lineRend;
    [SerializeField]
    AnimationCurve curve;
    [SerializeField]
    int maxPoints; // Line will actually have maxPoints + 1 points

    [SerializeField]
    [Range(0.0f, 1.0f)]
    float leftTarget;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float rightTarget;
    [SerializeField]
    [Range(0.1f, 10.0f)]
    float radius;
    [SerializeField]
    [Range(0.1f, 2.0f)]
    float width;
    [SerializeField]
    [Range(0.0001f, 1.0f)]
    float helperOffset;

    private float t;

    void Start () {

	}
	
	void Update () {
        updateLineRend(leftTarget, rightTarget);
    }

    private void updateLineRend(float left, float right) {
        if(left > right) {
            lineRend.numPositions = 0;
            return;
        }
        // Update line width
        lineRend.widthMultiplier = width;
        float dist = Mathf.Abs(left - right); // [0, 1]
        int nPoints = (int)Mathf.Ceil(dist * maxPoints);
        // Create points for curve section
        float t = left;
        float inc = dist / nPoints;
        lineRend.numPositions = nPoints + 1;
        for (int i = 0; i <= nPoints; ++i) {            
            Vector2 point = new Vector2(t, curve.Evaluate(t)) * radius;
            lineRend.SetPosition(i + 1, point);
            t += inc;
        }
    }
}
