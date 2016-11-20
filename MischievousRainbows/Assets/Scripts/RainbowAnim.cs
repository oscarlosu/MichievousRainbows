using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowAnim : MonoBehaviour {
    [SerializeField]
    LineRenderer lineRend;
    [SerializeField]
    int nPointsLinePerDistUnit; // Line will actually have maxPoints + 1 points
    [SerializeField]
    [Range(0.1f, 2.0f)]
    float lineWidth;

    [SerializeField]
    EdgeCollider2D lowerArcCol;
    [SerializeField]
    EdgeCollider2D upperArcCol;
    [SerializeField]
    EdgeCollider2D originCol;
    [SerializeField]
    EdgeCollider2D endCol;

    [SerializeField]
    int nPointsColPerDistUnit;




    [SerializeField]
    [Range(-10.0f, 10.0f)]
    float target;

    [SerializeField]
    float height = 1;
    [SerializeField]
    float width = 1;

    [SerializeField]
    bool offsetOrigin = true;

    void Start () {

	}
	
	void Update () {
        updateLineRend(target);
    }

    private void updateLineRend(float target) {
        // Update line width
        lineRend.widthMultiplier = lineWidth;

        float xLength = Mathf.Abs(target * width);
        float sign = Mathf.Sign(target);
        int nPoints = (int)Mathf.Ceil(xLength * nPointsLinePerDistUnit);
        // Create points for curve section
        float x = 0;
        float inc = xLength / nPoints;
        lineRend.numPositions = nPoints + 1;
        for (int i = 0; i <= nPoints; ++i) {            
            Vector2 point = new Vector2(sign * (x), Curve(x, offsetOrigin));
            lineRend.SetPosition(i, point);
            x += inc;
        }


        // Create points for colliders
        nPoints = (int)Mathf.Ceil(xLength * nPointsColPerDistUnit);
        x = 0;
        inc = xLength / nPoints;
        float hWidth = width / 2;
        Vector2[] points = new Vector2[nPoints + 1];
        for (int i = 0; i < nPoints; ++i) {
            points[i] = ParallelCurve(x, offsetOrigin, hWidth);
            x += inc;
        }
        upperArcCol.points = points;
    }

    private float Curve(float x, bool offsetOrigin) {
        float xOffset = offsetOrigin ? width : 0;
        float yOffset = offsetOrigin ? 1 : 0;
        return height * (yOffset - Mathf.Pow((x - xOffset) / width, 2));
    }

    private Vector2 ParallelCurve(float t, bool offsetOrigin, float distance) {
        float xOffset = offsetOrigin ? width : 0;
        float yOffset = offsetOrigin ? 1 : 0;

        float x = t;
        float y = Curve(t, offsetOrigin);
        float dx = 1;
        float dy = -2 * (t - xOffset) / width;

        float denominator = Mathf.Sqrt(dx * dx + dy * dy);

        Vector2 p = new Vector2(x + (distance * dy) / (denominator),
                                y - (distance * dx) / (denominator));

        return p;
    }
}
