using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TofuPlugin.Pathfinding
{
    public class Path
    {
        public readonly Vector3[] LookPoints;
        public readonly PathLine[] TurnBoundaries;
        public readonly int FinishLineIndex;
        public readonly int slowDownIndex;

        public Path(Vector3[] waypoints, Vector3 startPos, float turnDist, float stoppingDistance)
        {
            LookPoints = waypoints;
            TurnBoundaries = new PathLine[LookPoints.Length];
            FinishLineIndex = TurnBoundaries.Length - 1;

            Vector2 previousPoint = Vector3ToVector2(startPos);
            for (int i = 0; i < LookPoints.Length; i++)
            {
                Vector2 currentPoint = Vector3ToVector2(LookPoints[i]);
                Vector2 directionToCurrentPoint = (currentPoint - previousPoint).normalized;
                Vector2 turnBoundaryPoint = (i == FinishLineIndex) ? currentPoint : currentPoint - directionToCurrentPoint * turnDist;
                TurnBoundaries[i] = new PathLine(turnBoundaryPoint, previousPoint - directionToCurrentPoint * turnDist);
                previousPoint = turnBoundaryPoint;
            }

            float distanceFromEndPoint = 0;
            for (int i = LookPoints.Length - 1; i > 0; i--)
            {
                distanceFromEndPoint += Vector3.Distance(LookPoints[i], LookPoints[i - 1]);
                if (distanceFromEndPoint > stoppingDistance)
                {
                    slowDownIndex = i;
                    break;
                }
            }
        }

        Vector2 Vector3ToVector2(Vector3 v3)
        {
            return new Vector2(v3.x, v3.y);
        }

        public void DrawWithGizmos()
        {
            Gizmos.color = Color.black;
            foreach (Vector3 p in LookPoints)
            {
                Gizmos.DrawCube(p + Vector3.back, new Vector3(0.1f, 0.1f, 0.1f));
            }

            Gizmos.color = Color.white;
            foreach (PathLine line in TurnBoundaries)
            {
                line.DrawWithGizmos(1f);
            }
        }
    }
}

