using UnityEngine;

namespace TofuPlugin.Pathfinding
{
    public struct PathLine
    {
        private const float _verticalLineGradient = 1e5f;
        private float _gradient;
        private float _yIntercept;
        private float _gradientPerpendicular;
        private Vector2 _pointOnLine1;
        private Vector2 _pointOnLine2;
        private bool _approachSide;

        public PathLine(Vector2 pointOnLine, Vector2 pointPerpendicularToLine)
        {
            float dx = pointOnLine.x - pointPerpendicularToLine.x;
            float dy = pointOnLine.y - pointPerpendicularToLine.y;

            if (dx == 0)
            {
                _gradientPerpendicular = _verticalLineGradient;
            }
            else
            {
                _gradientPerpendicular = dy / dx;
            }

            if (_gradientPerpendicular == 0)
            {
                _gradient = _verticalLineGradient;
            }
            else
            {
                _gradient = -1 / _gradientPerpendicular;
            }

            _yIntercept = pointOnLine.y - _gradient * pointOnLine.x;
            _pointOnLine1 = pointOnLine;
            _pointOnLine2 = pointOnLine + new Vector2(1, _gradient);
            _approachSide = false;

            _approachSide = GetSide(pointPerpendicularToLine);
        }

        private bool GetSide(Vector2 p)
        {
            return (p.x - _pointOnLine1.x) * (_pointOnLine2.y - _pointOnLine1.y) >
                   (p.y - _pointOnLine1.y) * (_pointOnLine2.x - _pointOnLine1.x);
        }

        public bool HasCrossedLine(Vector2 p)
        {
            return GetSide(p) != _approachSide;
        }

        public float DistanceFromPoint(Vector2 p)
        {
            float yInterceptPerpendicular = p.y - _gradientPerpendicular * p.x;
            float intersectX = (yInterceptPerpendicular - _yIntercept) / (_gradient - _gradientPerpendicular);
            float intersectY = _gradient * intersectX + _yIntercept;
            return Vector2.Distance(p, new Vector2(intersectX, intersectY));
        }

        public void DrawWithGizmos(float length)
        {
            Vector3 lineDir = new Vector3(1, _gradient, 0).normalized;
            Vector3 lineCentre = new Vector3(_pointOnLine1.x, _pointOnLine1.y) + Vector3.up;
            Gizmos.DrawLine(lineCentre - lineDir * length/2f, lineCentre + lineDir * length/2f);
        }

    }
}
