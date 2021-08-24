/************************************************************************************************************************************
* Developed by Mamadou Cisse  and Jasper Flick                                                                                      *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                                    *
*************************************************************************************************************************************/

using UnityEngine;
using InfinityEngine;
using InfinityEngine.Extensions;
using System.Collections.Generic;
using InfinityEngine.Attributes;

namespace IntoTheHole
{


    /// <summary>
    ///  Class that represent a Pipe generated proceduraly        
    /// </summary>
    [OverrideInspector]
    public class Pipe : MonoBehaviour
    {

        /// <summary>
        /// The shape of a pipe
        /// </summary>
        public enum Shape
        {
            /// <summary>
            /// Cube shape
            /// </summary>
            Cube = 4,
            /// <summary>
            /// Octogone shape
            /// </summary>
            Octogone = 8,

            /// <summary>
            /// Circle shape
            /// </summary>
            Circle = 16
        }

        #region Fields
        [InfinityHeader]
        [SerializeField]
        private bool __help__;

        [Accordion("Shape Parameters")]
        [SerializeField]
        private Shape shape = Shape.Circle;

        [Accordion("Shape Parameters")]
        [Message("The radius of the pipe.", MessageTypes.Info)]
        [SerializeField]
        private float pipeRadius = 1.0f;


        /// <summary>
        /// influences the distance betweewn the rings of the pipe.
        /// </summary>
        [Accordion("Shape Parameters")]
        [Message("Influence the distance betweewn the rings of the pipe.", MessageTypes.Info)]
        [SerializeField]
        private float ringDistance = 0.77f;

        /// <summary>
        /// Curve radius matches the radius between the two pipe ends.
        /// It control length of the pipe.
        /// </summary>
        [Accordion("Shape Parameters")]
        [Message("Curve radius matches the radius between the two pipe ends. It control length of the pipe.", MessageTypes.Info)]
        [MinMaxRange(1.0f, 30.0f)]
        [SerializeField]
        private MinMax curveRadius;


        /// <summary>
        ///  Controls pipe curvature.
        ///  More curveSegmentCount is big, more  pipe will be curved.
        ///  It control smoothing and curving of the pipe. 
        /// </summary>
        [Accordion("Shape Parameters")]
        [Message("More curveSegmentCount is big, more  pipe will be curved. It control smoothing and curving of the pipe.", MessageTypes.Info)]
        [MinMaxRange(1, 20)]
        [SerializeField]
        private MinMax curveSegmentCount;

        /// <summary>
        ///  List of all items to generated
        /// </summary>
        [Accordion("Prefabs")]
        [Message("List of all items to generated.", MessageTypes.Info)]
        [SerializeField]
        private List<BaseItem> itemPrefabs;

        /// <summary>
        /// The angle of the curve formed by the pipe
        /// </summary>
        public float CurveAngle { get; private set; }
        /// <summary>
        /// Controls length of the pipe.
        /// </summary>
        public float CurveRadius { get; private set; }
        /// <summary>
        /// The relative rotation of the pipe 
        /// </summary>
        public float RelativeRotation { get; private set; }

        /// <summary>
        /// The number of segment of the pipe (control the shape of the pipe)
        /// </summary>
        public int PipeSegmentCount { get; private set; }

        /// <summary>
        /// Controls smoothing and curving of the pipe. 
        /// </summary>
        public int CurveSegmentCount { get; private set; }

        private Mesh mesh;
        private Vector3[] vertices;
        private Vector2[] uv;
        private int[] triangles;

        /// <summary>
        /// Type/int dictionary where 'Type' is the type of the generator to use and 'int' is the number of instance of the generator to create.
        /// </summary>
        private Dictionary<System.Type, int> Generators = new Dictionary<System.Type, int>()
        {
            {typeof(RandomPlacer), 4 }, {typeof(SpiralPlacer), 8 }, {typeof(CirclePlacer), 10 }
        };

        private List<PipeItemGenerator> itemGenerators;
        private Controler controler;

        private bool IsPlaying;

        #endregion Fields

        #region Methods

        #region Unity

        private void OnEnable()
        {
            controler = controler ?? FindObjectOfType<Controler>();
            if (controler != null)
            {
                controler.RegisterEvent(GameEvent.Start, OnStart);
                controler.RegisterEvent(GameEvent.End, OnEnd);
            }
        }

        private void OnDisable()
        {
            if (controler != null)
            {
                controler.UnRegisterEvent(GameEvent.Start, OnStart);
                controler.UnRegisterEvent(GameEvent.End, OnEnd);
            }
        }

        private void Awake()
        {

            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "Pipe";
            PipeSegmentCount = (int)shape;
        }

        private void Start()
        {
            CreateGenerators();
        }

        #endregion Unity

        #region Gameplay

        void OnStart()
        {
            IsPlaying = true;
        }

        void OnEnd()
        {
            SoundManager.StopMusic();
            IsPlaying = false;
            RemoveItems();
        }

        /// <summary>
        /// Setup <see cref="itemGenerators"/> list
        /// </summary>
        private void CreateGenerators()
        {
            itemGenerators = new List<PipeItemGenerator>();

            PipeItemGenerator generator;
            Generators.ForEach((type, count) =>
            {
                for (int i = 0; i < count; i++)
                {
                    generator = (PipeItemGenerator)gameObject.AddComponent(type);
                    itemGenerators.Add(generator);
                }
            });

        }

        /// <summary>
        /// Align this pipe with the given <paramref name="pipe"/> in parameter of the function
        /// </summary>
        /// <param name="pipe">The pipe with which to align this pipe.</param>
        public void AlignWith(Pipe pipe)
        {
            RelativeRotation = Random.Range(0, CurveSegmentCount) * 360f / PipeSegmentCount;

            transform.SetParent(pipe.transform, false);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0f, 0f, -pipe.CurveAngle);
            transform.Translate(0f, pipe.CurveRadius, 0f);
            transform.Rotate(RelativeRotation, 0f, 0f);
            transform.Translate(0f, -CurveRadius, 0f);
            transform.SetParent(pipe.transform.parent);
            transform.localScale = Vector3.one;

        }

        /// <summary>
        /// Remove all items into this pipe
        /// </summary>
        private void RemoveItems()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).ToPool(); // push the game object to the pool manager
            }
        }

        /// <summary>
        /// Adds <see cref="BaseItem"/> elements to this pipe
        /// </summary>
        private void GenerateItems()
        {
            if (IsPlaying)
            {
                itemGenerators.Random().GenerateItems(this);
            }
        }

        /// <summary>
        /// Gets random <see cref="BaseItem"/> from the PoolManager
        /// </summary>
        /// <returns>Random BaseItem</returns>
        public BaseItem RandomItem
        {
            get
            {
                // Do not specify a position for the gameObject
                return itemPrefabs.Random().gameObject.FromPool((go) => { }).GetComponent<BaseItem>();
            }
        }

        #endregion Gameplay

        #region Mesh Generation

        /// <summary>
        /// Build this pipe mesh
        /// </summary>
        public void RebuildMesh()
        {

            CurveRadius = Random.Range((int)curveRadius.min, (int)curveRadius.max);
            CurveSegmentCount = Random.Range((int)curveSegmentCount.min, (int)curveSegmentCount.min + 1);

            mesh.Clear();
            SetVertices();
            SetUV();
            SetTriangles();
            mesh.RecalculateNormals();

            RemoveItems();

            if (IsPlaying)
            {
                GenerateItems();
            }
        }

        /// <summary>
        /// Creates this mesh vertices
        /// </summary>
        protected void SetVertices()
        {
            vertices = new Vector3[PipeSegmentCount * CurveSegmentCount * 4];

            float uStep = ringDistance / CurveRadius;
            CurveAngle = uStep * CurveSegmentCount * (360f / (2f * Mathf.PI));
            CreateFirstQuadRing(uStep);
            int iDelta = PipeSegmentCount * 4;
            for (int u = 2, i = iDelta; u <= CurveSegmentCount; u++, i += iDelta)
            {
                CreateQuadRing(u * uStep, i);
            }
            mesh.vertices = vertices;
        }

        /// <summary>
        /// Create the first ring of quads of this mesh vertices.
        /// </summary>
        /// <param name="u">Angle along the curve.</param>
        protected void CreateFirstQuadRing(float u)
        {
            float vStep = (2f * Mathf.PI) / PipeSegmentCount;

            Vector3 vertexA = GetPointOnTorus(0f, 0f);
            Vector3 vertexB = GetPointOnTorus(u, 0f);
            for (int v = 1, i = 0; v <= PipeSegmentCount; v++, i += 4)
            {
                vertices[i] = vertexA;
                vertices[i + 1] = vertexA = GetPointOnTorus(0f, v * vStep);
                vertices[i + 2] = vertexB;
                vertices[i + 3] = vertexB = GetPointOnTorus(u, v * vStep);
            }
        }

        /// <summary>
        /// Create the ring of quads of this mesh vertices.
        /// </summary>
        /// <param name="u">Angle along the curve.</param>
        /// <param name="i">current vertice</param>
        protected void CreateQuadRing(float u, int i)
        {
            float vStep = (2f * Mathf.PI) / PipeSegmentCount;
            int ringOffset = PipeSegmentCount * 4;

            Vector3 vertex = GetPointOnTorus(u, 0f);
            for (int v = 1; v <= PipeSegmentCount; v++, i += 4)
            {
                vertices[i] = vertices[i - ringOffset + 2];
                vertices[i + 1] = vertices[i - ringOffset + 3];
                vertices[i + 2] = vertex;
                vertices[i + 3] = vertex = GetPointOnTorus(u, v * vStep);
            }
        }

        /// <summary>
        /// Creates this mesh uv
        /// </summary>
        protected void SetUV()
        {
            uv = new Vector2[vertices.Length];
            for (int i = 0; i < vertices.Length; i += 4)
            {
                uv[i] = Vector2.zero;
                uv[i + 1] = Vector2.right;
                uv[i + 2] = Vector2.up;
                uv[i + 3] = Vector2.one;
            }
            mesh.uv = uv;
        }

        /// <summary>
        /// Creates this mesh triangles
        /// </summary>
        protected void SetTriangles()
        {
            triangles = new int[PipeSegmentCount * CurveSegmentCount * 6];
            for (int t = 0, i = 0; t < triangles.Length; t += 6, i += 4)
            {
                triangles[t] = i;
                triangles[t + 1] = triangles[t + 4] = i + 2;
                triangles[t + 2] = triangles[t + 3] = i + 1;
                triangles[t + 5] = i + 3;
            }
            mesh.triangles = triangles;
        }

        /// <summary>
        ///     Find points on the surface of a torus thanks to a 3d sinusoidal function.<br/>
        ///     x = (R + r cos v) cos u <br/>
        ///     y = (R + r cos v) sin u<br/>
        ///     z = cos v<br/>
        ///     
        /// r is the radius of the pipe.
        /// R is the radius of the curve.
        /// u the angle along the curve.
        /// v the angle along the pipe.
        /// 
        /// </summary>
        /// <param name="u">Angle along the curve.</param>
        /// <param name="v">Angle along the Pipe.</param>
        /// <returns></returns>
        protected Vector3 GetPointOnTorus(float u, float v)
        {
            Vector3 p;
            float r = (CurveRadius + pipeRadius * Mathf.Cos(v));
            p.x = r * Mathf.Sin(u);
            p.y = r * Mathf.Cos(u);
            p.z = pipeRadius * Mathf.Sin(v);
            return p;
        }

        #endregion Mesh Generation

        #endregion Methods
    }
}