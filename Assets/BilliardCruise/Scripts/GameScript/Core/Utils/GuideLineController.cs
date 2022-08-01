using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BilliardCruise.Sava.Scripts
{
    public class GuideLineController : MonoBehaviour
    {

        [SerializeField] private GameObject linePrefab;
        [SerializeField] private GameObject ghostBallPrefab;
        [SerializeField] private Sprite allowedGhostBallSprite;
        [SerializeField] private Sprite restrictedGhostBallSprite;

        [HideInInspector]
        public GameObject lineMain;
        [HideInInspector]
        public GameObject lineSource;
        [HideInInspector]
        public GameObject lineTarget;
        [HideInInspector]
        public GameObject ghostBall;

        // private SpriteRenderer lineMainRenderer;
        private LineRenderer lineMainRenderer;
        private SpriteRenderer lineSourceRenderer;
        private SpriteRenderer lineTargetRenderer;
        private SpriteRenderer ghostBallRenderer;

        private float lineWidth = 0.1f;


        /// <summary>
        ///  Dotted Line Creator
        /// </summary>
        /// 
        public Sprite Dot;
        [Range(0.01f, 1f)]
        public float Size;
        [Range(0.1f, 2f)]
        public float Delta;

        public List<Vector3> positions = new List<Vector3>();
        List<GameObject> dots = new List<GameObject>();

        private void Awake()
        {
            if (lineMain == null)
            {
                lineMain = Instantiate(linePrefab);
            }

            if (lineSource == null)
            {
                //   lineSource = Instantiate(linePrefab);
            }

            if (lineTarget == null)
            {
                //    lineTarget = Instantiate(linePrefab);
            }

            if (ghostBall == null)
            {
                ghostBall = Instantiate(ghostBallPrefab);
                ghostBall.SetActive(false);
            }

            lineMainRenderer = lineMain.GetComponent<LineRenderer>();
            // lineSourceRenderer = lineSource.GetComponent<SpriteRenderer>();
            // lineTargetRenderer = lineTarget.GetComponent<SpriteRenderer>();
            ghostBallRenderer = ghostBall.GetComponent<SpriteRenderer>();
        }


        void FixedUpdate()
        {
            if (positions.Count > 0)
            {
                DestroyAllDots();
                positions.Clear();
            }
        }

        private void DestroyAllDots()
        {
            foreach (var dot in dots)
            {
                Destroy(dot);
            }

            dots.Clear();
        }

        GameObject GetOneDot()
        {
            // var gameObject = new GameObject();
            // gameObject.transform.localScale = Vector3.one * Size;
            // gameObject.transform.parent = transform;

            // var sr = gameObject.AddComponent<SpriteRenderer>();
            // sr.sprite = Dot;

            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.localScale = new Vector3(1f, 0.1f, 3f) * Size;

            obj.transform.parent = transform;
            obj.GetComponent<Collider>().enabled = false;

            return obj;
        }
        public void DrawDottedLine(Vector3 start, Vector3 end)
        {
            DestroyAllDots();

            Vector3 point = start;
            Vector3 direction = (end - start).normalized;

            while ((end - start).magnitude > (point - start).magnitude)
            {
                positions.Add(point);
                point += (direction * Delta);
            }

            positions.Add(end);

            Render(end);
        }

        private void Render(Vector3 end)
        {
            foreach (var position in positions)
            {
                var g = GetOneDot();
                g.transform.position = new Vector3(position.x, 2f, position.z);
                g.transform.LookAt(end);

                dots.Add(g);
            }
        }


        void Start()
        {

        }

        public void DrawLineMain(Vector3 point1, Vector3 point2)
        {
            DrawLine(lineMain, lineMainRenderer, point1, point2);
        }

        public void DrawLineSource(Vector3 point1, Vector3 point2)
        {
            //   DrawLine(lineSource, lineSourceRenderer, point1, point2);
        }

        public void DrawLineTarget(Vector3 point1, Vector3 point2)
        {
            //   DrawLine(lineTarget, lineTargetRenderer, point1, point2);
        }

        public void DrawGhostBall(Vector3 center, bool allowed)
        {
            if (ghostBall == null)
            {
                return;
            }

            ghostBall.SetActive(true);
            ghostBall.transform.position = center;

            if (ghostBallRenderer != null)
            {
                if (allowed)
                {
                    ghostBallRenderer.sprite = allowedGhostBallSprite;
                }
                else
                {
                    ghostBallRenderer.sprite = restrictedGhostBallSprite;
                }
            }
        }

        public void HideGuideLine()
        {
            HideLineMain();
            HideLineSource();
            HideLineTarget();

            if (ghostBall != null)
            {
                ghostBall.SetActive(false);
            }
        }

        public void SetGuideLineOpacity(float opacity)
        {
            // UnityEngine.Color clr = lineMainRenderer.color;
            // clr.a = opacity;

            // lineMainRenderer.color = clr;
            // lineSourceRenderer.color = clr;
            // lineTargetRenderer.color = clr;
            // ghostBallRenderer.color = clr;
        }

        public void HideLineMain()
        {
            if (lineMainRenderer != null)
            {
                lineMainRenderer.enabled = false;
            }
        }

        public void HideLineSource()
        {
            if (lineSourceRenderer != null)
            {
                lineSourceRenderer.enabled = false;
            }
        }

        public void HideLineTarget()
        {
            if (lineTargetRenderer != null)
            {
                lineTargetRenderer.enabled = false;
            }
        }

        private void DrawLine(GameObject line, LineRenderer lineRenderer, Vector3 point1, Vector3 point2)
        {
            lineRenderer.enabled = true;
            float distance = Vector3.Distance(point1, point2);
            Vector3[] positions = new Vector3[2];
            positions[0] = point1;
            positions[1] = point2;
            lineRenderer.SetPositions(positions);
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            lineRenderer.GetComponent<Renderer>().sharedMaterial.SetTextureScale("_MainTex", new Vector2(-distance, 1f));
        }

    }

}


