using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowAnim : MonoBehaviour {
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
    float curveHeight = 1;
    [SerializeField]
    float curveWidth = 1;

    [SerializeField]
    bool offsetOrigin = true;

    [SerializeField]
    List<RainbowStripe> stripes;

    void Start () {

	}
	
	void Update () {
        updateLineRend(target);
    }

    private void updateLineRend(float target) {
        // Early return if target is zero setting clearing colliders and renderers
        if(Mathf.Approximately(target, 0)) {
            upperArcCol.points = new Vector2[0];
            lowerArcCol.points = new Vector2[0];
            originCol.points = new Vector2[0];
            endCol.points = new Vector2[0];
            foreach (RainbowStripe s in stripes) {
                s.Rend.numPositions = 0;
            }
        }
        // Create points for colliders
        // Lower and upper arcs
        float xLength = Mathf.Abs(target * curveWidth);
        float sign = Mathf.Sign(target);
        int nPoints = (int)Mathf.Ceil(xLength * nPointsColPerDistUnit);
        float x = 0;
        float inc = xLength / nPoints;
        float hLineWidth = lineWidth / 2;
        Vector2[] pointsLowerArc = new Vector2[nPoints + 1];
        Vector2[] pointsUpperArc = new Vector2[nPoints + 1];
        for (int i = 0; i <= nPoints; ++i) {
            pointsUpperArc[i] = ParallelCurve(x, offsetOrigin, -hLineWidth);
            pointsUpperArc[i].x *= sign;
            pointsLowerArc[i] = ParallelCurve(x, offsetOrigin, hLineWidth);
            pointsLowerArc[i].x *= sign;
            x += inc;
        }
        upperArcCol.points = pointsUpperArc;
        lowerArcCol.points = pointsLowerArc;
        // Origin and end
        Vector2[] pointsOrigin = new Vector2[2];
        pointsOrigin[0] = pointsUpperArc[0];
        pointsOrigin[1] = pointsLowerArc[0];
        originCol.points = pointsOrigin;
        Vector2[] pointsEnd = new Vector2[2];
        pointsEnd[0] = pointsUpperArc[pointsUpperArc.Length - 1];
        pointsEnd[1] = pointsLowerArc[pointsLowerArc.Length - 1];
        endCol.points = pointsEnd;





        // Calculate reference points
        nPoints = (int)Mathf.Ceil(xLength * nPointsLinePerDistUnit);
        x = 0;
        inc = xLength / nPoints;
        Vector2[] referencePoints = new Vector2[nPoints + 1];
        for (int i = 0; i <= nPoints; ++i) {
            Vector2 point = new Vector2(sign * (x), Curve(x, offsetOrigin));
            referencePoints[i] = point;
            x += inc;
        }
        // Calculate orthogonal directions along rainbow center
        Vector2[] orthogonalDir = new Vector2[nPoints + 1];
        Vector2 dir = originCol.points[1] - originCol.points[0];
        orthogonalDir[0] = sign * dir.normalized;
        //Debug.DrawLine(referencePoints[0], referencePoints[0] + orthogonalDir[0], Color.red, 10f);
        for (int i = 1; i < referencePoints.Length - 1; ++i) {
            dir = referencePoints[i + 1] - referencePoints[i];
            dir = - new Vector2(-dir.y, dir.x);
            orthogonalDir[i] = dir.normalized;
            //Debug.DrawLine(referencePoints[i], referencePoints[i] + orthogonalDir[i], Color.red, 10f);
        }
        dir = endCol.points[1] - endCol.points[0];
        orthogonalDir[orthogonalDir.Length - 1] = sign * dir.normalized;
        //Debug.DrawLine(referencePoints[referencePoints.Length - 1], referencePoints[referencePoints.Length - 1] + orthogonalDir[referencePoints.Length - 1], Color.red, 10f);
        float coveredWidth = 0;
        for (int i = 0; i < stripes.Count; ++i) {
            RainbowStripe stripe = stripes[i];
            float stripeWidth = lineWidth * stripe.WidthFraction;
            //                   rainbow upperArc   stripe upperArc  center of stripe 
            float stripeOffset = lineWidth / 2.0f - coveredWidth - stripeWidth / 2.0f;            
            // Add points to stripe
            x = 0;
            inc = xLength / nPoints;
            stripe.Rend.numPositions = nPoints + 1;
            for (int j = 0; j <= nPoints; ++j) {
                Vector2 pos = referencePoints[j] - orthogonalDir[j] * stripeOffset;
                stripe.Rend.SetPosition(j, pos);
                x += inc;
            }
            // Set stripe color
            stripe.Rend.startColor = stripe.Color;
            stripe.Rend.endColor = stripe.Color;
            // Set width
            stripe.Rend.widthMultiplier = stripeWidth;
            coveredWidth += stripeWidth;
        }
        
        
        //nPoints = (int)Mathf.Ceil(xLength * nPointsLinePerDistUnit);
        //// Create points for curve section
        //x = 0;
        //inc = xLength / nPoints;
        //lineRend.numPositions = nPoints + 1;
        //for (int i = 0; i <= nPoints; ++i) {            
        //    Vector2 point = new Vector2(sign * (x), Curve(x, offsetOrigin));
        //    lineRend.SetPosition(i, point);
        //    x += inc;
        //}


        
    }

    private float Curve(float x, bool offsetOrigin) {
        float xOffset = offsetOrigin ? curveWidth : 0;
        float yOffset = offsetOrigin ? 1 : 0;
        return curveHeight * (yOffset - Mathf.Pow((x - xOffset) / curveWidth, 2));
    }

    private Vector2 ParallelCurve(float t, bool offsetOrigin, float distance) {
        float xOffset = offsetOrigin ? curveWidth : 0;
        float yOffset = offsetOrigin ? 1 : 0;

        float x = t;
        float y = Curve(t, offsetOrigin);
        float dx = 1;
        float dy = -2 * (t - xOffset) / curveWidth;

        float denominator = Mathf.Sqrt(dx * dx + dy * dy);

        Vector2 p = new Vector2(x + (distance * dy) / (denominator),
                                y - (distance * dx) / (denominator));

        return p;
    }
}
