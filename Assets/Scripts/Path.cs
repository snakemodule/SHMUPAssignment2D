using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Path : MonoBehaviour
{
    
    private Vector2Interpolation[] interpolators;

    private AnimationCurve ac;

    private Vector2 gizmosPosition;

    //[Serializable] public struct control_point {
    //    public float x, y, t;
    //};

    //[SerializeField] public List<control_point> controlPoints = new List<control_point>();

    [SerializeField] public List<Vector2> controlPoints = new List<Vector2>();
        

    struct CubicPolynomial
    {
        double c0, c1, c2, c3;

        public double eval(double t)
        {
            double t_squared = t * t;
            double t_cubed = t * t * t;
            return c0 + (c1 * t) + (c2 * t_squared) + (c3 * t_cubed);
        }

        void InitCubicPoly(double p_0, double p_1, double dp_0, double dp_1)
        {
            c0 = p_0;
            c1 = dp_0;
            c2 = (-3.0f * p_0) + (3 * p_1) - (2 * dp_0) - dp_1;
            c3 = (2 * p_0) - (2 * p_1) + dp_0 + dp_1;
        }

        public void InitNonuniformCatmullRom(double x0, double x1, double x2, double x3,
            double dt0, double dt1, double dt2)
        {
            // compute tangents when parameterized in [t1,t2]
            double t1 = (x1 - x0) / dt0 - (x2 - x0) / (dt0 + dt1) + (x2 - x1) / dt1;
            double t2 = (x2 - x1) / dt1 - (x3 - x1) / (dt1 + dt2) + (x3 - x2) / dt2;

            // rescale tangents for parametrization in [0,1]
            t1 *= dt1;
            t2 *= dt1;

            InitCubicPoly(x1, x2, t1, t2);
        }
    };

    struct Vector2Interpolation
    {
        CubicPolynomial x;
        CubicPolynomial y;
        //CubicPolynomial z;

        Vector3 eval(double t)
        {
            return new Vector3((float)x.eval(t), (float)y.eval(t));
        }

        static float vecDistSquared(Vector3 p, Vector3 q)
        {
            float dx = q.x - p.x;
            float dy = q.y - p.y;
            float dz = q.z - p.z;
            return dx * dx + dy * dy + dz * dz;
        }

        Vector2Interpolation(Vector3 p0, Vector3 p1,
        Vector3 p2, Vector3 p3)
        {
            float dt0 = Mathf.Pow(vecDistSquared(p0, p1), 0.25f);
            float dt1 = Mathf.Pow(vecDistSquared(p1, p2), 0.25f);
            float dt2 = Mathf.Pow(vecDistSquared(p2, p3), 0.25f);

            // safety check for repeated points
            if (dt1 < float.Epsilon) dt1 = 1.0f;
            if (dt0 < float.Epsilon) dt0 = dt1;
            if (dt2 < float.Epsilon) dt2 = dt1;

            x = new CubicPolynomial();
            y = new CubicPolynomial();

            x.InitNonuniformCatmullRom(p0.x, p1.x, p2.x, p3.x, dt0, dt1, dt2);
            y.InitNonuniformCatmullRom(p0.y, p1.y, p2.y, p3.y, dt0, dt1, dt2);
            //z.InitNonuniformCatmullRom(p0.z, p1.z, p2.z, p3.z, dt0, dt1, dt2);
        }
    };

}
