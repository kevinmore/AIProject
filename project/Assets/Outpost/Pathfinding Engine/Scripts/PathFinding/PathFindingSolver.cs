using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CS7056_AIToolKit
{
    /// <summary>
    /// This is the A* Pathing Finding Solver
    /// Create an empty game object in the scene, and attach this component to the game object.
    /// </summary>
    public class PathFindingSolver : MonoBehaviour
    {
        // Singleton of this class
        public static PathFindingSolver Instance;

        // Layers for the solver the work(MUST BE ASSIGNED)
        public LayerMask walkableLayer;
        public LayerMask obstacleLayer;
        public LayerMask dynamicObstacleLayer;
        public LayerMask agentLayer;

        [HideInInspector]
        public NavWorld world = new NavWorld(10, 10, 1, 1); // The navigation world
        [HideInInspector]
        public float inclinationMax = 45;                   // Max inclination angle allowed (in degrees) 
        [HideInInspector]
        public bool diagonalConnection = true;              // Whether linking two node diagonally
        [HideInInspector]
        public bool erode = true;                           // Whether erode the map
        [HideInInspector]
        public bool checkDynamicObstacle = true;            // Whether check dynamic obstacle
        [HideInInspector]
        public bool agentAvoidance = true;                  // Whether the agents should avoid each other
        [HideInInspector]
        public float updateRateForDynamicObstacles = 0.2f;  // Update frequency for checking dynamic obstacles

        private float mapUpdatedTime;

        // assign the static instance variable
        void Awake()
        {
            Instance = this;
        }

        // generate the Navigation Map (Node grid)
        void Start()
        {
            mapUpdatedTime = Time.time;
            GenerateStaticMap();
            UpdateMapByDynamicObstacles();
        }

        // update the Navigation Map after each frame
        // if the solver 
        void LateUpdate()
        {
            if (checkDynamicObstacle && Time.time - mapUpdatedTime > updateRateForDynamicObstacles)
            {
                UpdateMapByDynamicObstacles();
                mapUpdatedTime = Time.time;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Map Generation
        /// </summary>

        // Function to generate a static map according to collision detection
        public void GenerateStaticMap()
        {
            world.map = new Node[world.tilesInX, world.tilesInZ];
            for (int i = 0; i < world.tilesInX; ++i)
                for (int j = 0; j < world.tilesInZ; ++j)
                {
                    float x = transform.position.x + i * world.tileSize + ((float)world.tileSize) / 2;
                    float z = transform.position.z + j * world.tileSize + ((float)world.tileSize) / 2;
                    float y = transform.position.y;
                    world.map[i, j] = new Node(new Vector3(x, y, z));

                    bool walkable = true;
                    RaycastHit hit;
                    float height = world.height;

                    if (Physics.Raycast(new Vector3(x, y + height, z), Vector3.down, out hit, height))
                    {
                        if (Physics.Raycast(new Vector3(x, y + height, z), Vector3.down, out hit, height, walkableLayer))
                        {
                            y = hit.point.y;
                        }
                        if (Physics.Raycast(new Vector3(x, y + height, z), Vector3.down, out hit, height, obstacleLayer))
                        {
                            walkable = false;
                        }
                    }
                    else
                    {
                        walkable = false;
                    }
                    world.map[i, j].pos = new Vector3(x, y, z);
                    world.map[i, j].walkable = walkable;
                }

            if (erode)
            {
                for (int i = 0; i < world.tilesInX; ++i)
                    for (int j = 0; j < world.tilesInZ; ++j)
                    {
                        if (world.map[i, j].walkable)
                        {
                            Collider[] cols = Physics.OverlapSphere(world.map[i, j].pos, world.tileSize, obstacleLayer);
                            if (cols.Length > 0)
                            {
                                world.map[i, j].walkable = false;
                            }
                        }
                    }
            }

            for (int i = 0; i < world.tilesInX; ++i)
                for (int j = 0; j < world.tilesInZ; ++j)
                {
                    List<Node> neighbourList = new List<Node>();
                    world.map[i, j].neighbourNodes = neighbourList.ToArray();
                    Node nodeFrom = world.map[i, j];
                    Node nodeTo = world.map[i, j];
                    int links = 4;
                    if (j - 1 > -1)
                    {
                        nodeTo = world.map[i, j - 1];
                        neighbourList = AddInNeighbourList(neighbourList, nodeFrom, nodeTo);
                    }
                    if (j + 1 < world.tilesInZ)
                    {
                        nodeTo = world.map[i, j + 1];
                        neighbourList = AddInNeighbourList(neighbourList, nodeFrom, nodeTo);
                    }
                    if (i - 1 > -1)
                    {
                        nodeTo = world.map[i - 1, j];
                        neighbourList = AddInNeighbourList(neighbourList, nodeFrom, nodeTo);
                    }
                    if (i + 1 < world.tilesInX)
                    {
                        nodeTo = world.map[i + 1, j];
                        neighbourList = AddInNeighbourList(neighbourList, nodeFrom, nodeTo);
                    }

                    if (diagonalConnection)
                    {
                        links = 8;
                        if (i + 1 < world.tilesInX && j + 1 < world.tilesInZ)
                        {
                            nodeTo = world.map[i + 1, j + 1];
                            neighbourList = AddInNeighbourList(neighbourList, nodeFrom, nodeTo);
                        }
                        if (i - 1 > -1 && j + 1 < world.tilesInZ)
                        {
                            nodeTo = world.map[i - 1, j + 1];
                            neighbourList = AddInNeighbourList(neighbourList, nodeFrom, nodeTo);
                        }
                        if (i + 1 < world.tilesInX && j - 1 > -1)
                        {
                            nodeTo = world.map[i + 1, j - 1];
                            neighbourList = AddInNeighbourList(neighbourList, nodeFrom, nodeTo);
                        }
                        if (i - 1 > -1 && j - 1 > -1)
                        {
                            nodeTo = world.map[i - 1, j - 1];
                            neighbourList = AddInNeighbourList(neighbourList, nodeFrom, nodeTo);
                        }
                    }

                    world.map[i, j].neighbourNodes = neighbourList.ToArray();
                    world.map[i, j].walkableInitial = world.map[i, j].walkable;

                    if (erode)
                    {
                        if (world.map[i, j].neighbourNodes.Length < links)
                        {
                            world.map[i, j].walkableInitial = false;
                            world.map[i, j].walkable = false;
                        }
                    }
                }
        }



        public float AngleToTangent(float angle)
        {
            return (Mathf.Tan(Mathf.Deg2Rad * (angle)));
        }



        public List<Node> AddInNeighbourList(List<Node> neighbourList, Node nodeFrom, Node nodeTo)
        {
            float height = Mathf.Abs(nodeFrom.pos.y - nodeTo.pos.y);
            if (height < (AngleToTangent(inclinationMax)))
            {
                float dist = Mathf.Abs(Vector3.Distance(nodeFrom.pos, nodeTo.pos));
                nodeTo.cost = dist;
                neighbourList.Add(nodeTo);
            }
            return neighbourList;
        }


        // Function to update the map while checking the dynamic obstacles
        public void UpdateMapByDynamicObstacles()
        {

            for (int i = 0; i < world.tilesInX; ++i)
                for (int j = 0; j < world.tilesInZ; ++j)
                {
                    if (world.map[i, j].walkableInitial)
                    {
                        float x = transform.position.x + i * world.tileSize + ((float)world.tileSize) / 2;
                        float z = transform.position.z + j * world.tileSize + ((float)world.tileSize) / 2;
                        float y = transform.position.y;

                        RaycastHit hit;
                        float height = (float)world.height;

                        if (Physics.Raycast(new Vector3(x, y + height, z), Vector3.down, out hit, height, dynamicObstacleLayer))
                            world.map[i, j].walkable = false;
                        else
                            world.map[i, j].walkable = true;
                    }
                }
        }


        // Function to get a node from the map given the position vector
        public Node GetNodeFromPosition(Vector3 point)
        {
            Node[,] map = world.map;
            float startX = transform.position.x;
            float startZ = transform.position.z;

            int x = (int)((point.x - startX) / world.tileSize);
            int z = (int)((point.z - startZ) / world.tileSize);

            Node node = map[x, z];
            if (!node.walkable)
            {
                float distToNode = Mathf.Infinity;
                float distNear = Mathf.Infinity;
                for (int i = 0; i < world.tilesInX; ++i)
                    for (int j = 0; j < world.tilesInZ; ++j)
                    {
                        if (map[i, j].walkable)
                        {
                            distToNode = Vector3.Distance(point, map[i, j].pos);
                            if (distToNode < distNear)
                            {
                                distNear = distToNode;
                                node = map[i, j];
                            }
                        }
                    }
            }
            return node;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Path Finding
        /// </summary>

        static List<Agent> agentsToSearchPath = new List<Agent>();
        [HideInInspector]
        public int agentsToFindPathEveryFrame = 20;

        // Agents search for paths in each frame
        void Update()
        {
            int agentsCount = 0;
            while (agentsToSearchPath.Count > 0 && agentsCount < agentsToFindPathEveryFrame)
            {
                Agent agentToSearchPath = agentsToSearchPath[0];
                agentToSearchPath.startNode = GetNodeFromPosition(agentToSearchPath.pivot.transform.position);
                AStar(agentToSearchPath);
                agentToSearchPath.search = false;
                agentsToSearchPath.RemoveAt(0);
                ++agentsCount;
            }
        }

        // Static function to search the path to a given target position for the agent
        public static void SearchPath(Agent agent, Vector3 target)
        {
            agent.endNode = PathFindingSolver.Instance.GetNodeFromPosition(target);
            if (!agent.search)
            {
                agent.search = true;
                agentsToSearchPath.Add(agent);
            }
        }

        // Core Implementation of A* algorithm
        public void AStar(Agent agent)
        {
            Node node = agent.startNode;
            Heap heap = new Heap();

            while (true)
            {
                heap.closeList.Add(node);
                node.state = State.Close;

                for (int i = 0; i < node.neighbourNodes.Length; i++)
                {
                    if (node.neighbourNodes[i].walkable)
                    {
                        if (node.neighbourNodes[i].state == State.Clear)
                        {
                            node.neighbourNodes[i].G = node.G + node.neighbourNodes[i].cost;
                            node.neighbourNodes[i].H = Vector3.Distance(node.neighbourNodes[i].pos, agent.endNode.pos);
                            node.neighbourNodes[i].F = node.neighbourNodes[i].G + node.neighbourNodes[i].H;
                            node.neighbourNodes[i].parent = node;
                        }
                        else if (node.neighbourNodes[i].state == State.Open)
                        {
                            float tempG = node.G + node.neighbourNodes[i].cost;
                            if (node.neighbourNodes[i].G > tempG)
                            {
                                node.neighbourNodes[i].parent = node;
                                node.neighbourNodes[i].G = tempG;
                                node.neighbourNodes[i].F = node.neighbourNodes[i].G + node.neighbourNodes[i].H;
                            }
                        }
                    }
                }
                foreach (Node neighbour in node.neighbourNodes)
                {
                    if (neighbour.state == State.Clear && neighbour.walkable)
                    {
                        neighbour.state = State.Open;
                        heap.Configure(Heap.Operation.Insert, neighbour);
                    }
                }
                if (heap.openList.Count == 0)
                {
                    break;
                }
                else
                {
                    node = heap.openList[0];
                    heap.Configure(Heap.Operation.RemoveFirst);
                }
                if (node == agent.endNode)
                    break;

            }

            // If path is found....
            if (heap.openList.Count != 0)
                agent.path = RetracePath(node);
            // If path is not found agent path is empty.
            else
                agent.path = new List<Vector3>();

            world.ResetMap();
        }

        List<Vector3> RetracePath(Node node)
        {
            //Add all nodes from startNode to endNode in array p.
            List<Vector3> p = new List<Vector3>();

            while (node != null)
            {
                p.Add(node.pos);
                node = node.parent;
            }

            p.Reverse();
            return p;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gizmos Settings
        /// </summary>

        [HideInInspector]
        public bool showGizmo = true;
        [HideInInspector]
        public bool showNodes = true;
        [HideInInspector]
        public bool showLinks = false;
        [HideInInspector]
        public bool showUnwalkableNodes = false;
        [HideInInspector]
        public Color colorNode = Color.white;
        [HideInInspector]
        public Color colorLinks = Color.blue;
        [HideInInspector]
        public Color colorUnwalkableNode = Color.red;

        void OnDrawGizmos()
        {
            float GizmoNodeSize = 0.2f;
            Gizmos.color = Color.yellow;
            Vector3 halfExtents = new Vector3(0.5f * world.tilesInX, 0.5f * world.height, 0.5f * world.tilesInZ) * world.tileSize;
            Gizmos.DrawWireCube(transform.position + halfExtents, halfExtents * 2);
            //Gizmos.DrawWireCube(transform.position + new Vector3(((float)world.tilesInX * world.tileSize) / 2, (float)(world.height) / 2, ((float)world.tilesInZ * world.tileSize) / 2), new Vector3(world.tilesInX * world.tileSize, world.height * world.tileSize, world.tilesInZ * world.tileSize));
            //Gizmos.DrawCube (transform.position , Vector3.one);

            Vector3 yOffset = new Vector3(0, 0.01f, 0);

            if (showGizmo)
            {
                if (world.map != null)
                {
                    for (int i = 0; i < world.tilesInX; ++i)
                        for (int j = 0; j < world.tilesInZ; ++j)
                        {
                            if (world.map[i, j].walkable)
                            {
                                if (showNodes)
                                {
                                    Gizmos.color = colorNode;
                                    Gizmos.DrawWireCube(world.map[i, j].pos + yOffset, new Vector3(GizmoNodeSize, 0, GizmoNodeSize));
                                }
                                if (showLinks)
                                {
                                    foreach (Node n in world.map[i, j].neighbourNodes)
                                    {
                                        if (n.walkable)
                                        {
                                            Gizmos.color = colorLinks;
                                            Gizmos.DrawLine(world.map[i, j].pos + yOffset, n.pos + yOffset);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (showUnwalkableNodes)
                                {
                                    Gizmos.color = colorUnwalkableNode;
                                    Gizmos.DrawWireCube(world.map[i, j].pos + yOffset, new Vector3(GizmoNodeSize, 0, GizmoNodeSize));
                                }
                            }
                        }
                }
            }
        }

        
    }
}