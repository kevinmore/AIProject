using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PathfindingEngine : MonoBehaviour {
	public static PathfindingEngine Instance;
	[HideInInspector] public  Area  area = new Area(10,10,10,1);
	[HideInInspector] public  float inclinationMax = 30;
	[HideInInspector] public  bool  diagonalConnection = true;
	[HideInInspector] public  bool  erode;
	[HideInInspector] public  bool  dynamicObstacle = false;
	[HideInInspector] public  bool  agentAvoidance = true;
	[HideInInspector] public  float updateTimeForDynamicObstacles = 0.2f;
	[HideInInspector] private float mapUpdateStartTime;
	public LayerMask walkableLayer;
	public LayerMask obstacleLayer;
	public LayerMask dynamicObstacleLayer;
	public LayerMask agentLayer;



	void Awake(){
		Instance=this;
	}



	void Start () {
		mapUpdateStartTime = Time.time;
		GenerateStaticMap();
		UpdateMapByDynamicObstacles();
	}



	void LateUpdate(){
		if( Time.time-mapUpdateStartTime > updateTimeForDynamicObstacles ){
			UpdateMapByDynamicObstacles();
			mapUpdateStartTime = Time.time;
		}
	}



	public void GenerateStaticMap(){
		area.map = new Node[area.tilesInX,area.tilesInZ];
		for(int i=0; i<area.tilesInX; i++){
			for(int j=0; j<area.tilesInZ; j++){
				float x = transform.position.x+i*area.tileSize + ((float)area.tileSize)/2;
				float z = transform.position.z+j*area.tileSize + ((float)area.tileSize)/2;
				float y = transform.position.y;
				area.map[i,j] = new Node(new Vector3(x, y, z));

				bool walkable = true;
				RaycastHit hit;
				float height = (float)area.height;

				if(Physics.Raycast (new Vector3(x, y+height, z), Vector3.down, out hit,height)) {
					if(Physics.Raycast (new Vector3(x, y+height, z), Vector3.down, out hit,height,walkableLayer)) {
						y = hit.point.y;
					}
					if(Physics.Raycast (new Vector3(x, y+height, z), Vector3.down, out hit,height,obstacleLayer)) {
						walkable = false;
					}
				}else{
					walkable = false;
				}
				area.map[i,j].pos = new Vector3(x,y,z);
				area.map[i,j].walkable = walkable;
			}
		}

		if(erode){
			for(int i=0; i<area.tilesInX; i++){
				for(int j=0; j<area.tilesInZ; j++){
					if(	area.map[i,j].walkable ){
						Collider[] cols = Physics.OverlapSphere(area.map[i,j].pos,area.tileSize,obstacleLayer);
						if(cols.Length>0){
							area.map[i,j].walkable = false;
						}
					}
				}
			}
		}

		for(int i=0; i<area.tilesInX; i++){
			for(int j=0; j<area.tilesInZ; j++){
				List<Node> neighbourList=new List<Node>();
				area.map[i,j].neighbour = neighbourList.ToArray();
				Node nodeFrom = area.map[i,j];
				Node nodeTo = area.map[i,j];
				int links = 4;
				if( j-1>-1){
					nodeTo = area.map[i,j-1];
					neighbourList = AddInNeighbourList(neighbourList,nodeFrom,nodeTo);
				}
				if( j+1<area.tilesInZ ){
					nodeTo = area.map[i,j+1];
					neighbourList = AddInNeighbourList(neighbourList,nodeFrom,nodeTo);
				}
				if( i-1>-1 ){
					nodeTo = area.map[i-1,j];
					neighbourList = AddInNeighbourList(neighbourList,nodeFrom,nodeTo);
				}
				if( i+1<area.tilesInX ){
					nodeTo = area.map[i+1,j];
					neighbourList = AddInNeighbourList(neighbourList,nodeFrom,nodeTo);
				}

				if(diagonalConnection){
					links = 8;
					if( i+1<area.tilesInX && j+1<area.tilesInZ ){
						nodeTo = area.map[i+1,j+1];
						neighbourList = AddInNeighbourList(neighbourList,nodeFrom,nodeTo);
					}
					if( i-1>-1 && j+1<area.tilesInZ ){
						nodeTo = area.map[i-1,j+1];
						neighbourList = AddInNeighbourList(neighbourList,nodeFrom,nodeTo);
					}
					if( i+1<area.tilesInX && j-1>-1 ){
						nodeTo = area.map[i+1,j-1];
						neighbourList = AddInNeighbourList(neighbourList,nodeFrom,nodeTo);
					}
					if( i-1>-1 && j-1>-1 ){
						nodeTo = area.map[i-1,j-1];
						neighbourList = AddInNeighbourList(neighbourList,nodeFrom,nodeTo);
					}
				}

				area.map[i,j].neighbour = neighbourList.ToArray();
				area.map[i,j].walkableInitial = area.map[i,j].walkable;

				if(erode){
					if(area.map[i,j].neighbour.Length<links){
						area.map[i,j].walkableInitial = false;
						area.map[i,j].walkable = false;
					}
				}
			}
		}
	}



	public float AngleToTangent(float angle){
		return (Mathf.Tan (Mathf.Deg2Rad*( angle )));
	}



	public List<Node> AddInNeighbourList(List<Node> neighbourList,Node nodeFrom,Node nodeTo){
		float height = Mathf.Abs(nodeFrom.pos.y - nodeTo.pos.y);
		if( height< (AngleToTangent(inclinationMax) ) ){
			float dist = Mathf.Abs( Vector3.Distance(nodeFrom.pos, nodeTo.pos) );
			nodeTo.cost = dist;
			neighbourList.Add(nodeTo);
		}
		return neighbourList;
	}



	public void UpdateMapByDynamicObstacles(){
		for(int i=0; i<area.tilesInX; i++){
			for(int j=0; j<area.tilesInZ; j++){
				if(area.map[i,j].walkableInitial){
					float x = transform.position.x+i*area.tileSize + ((float)area.tileSize)/2;
					float z = transform.position.z+j*area.tileSize + ((float)area.tileSize)/2;
					float y = transform.position.y;

					RaycastHit hit;
					float height = (float)area.height;
					
					if(Physics.Raycast (new Vector3(x, y+height, z), Vector3.down, out hit,height,dynamicObstacleLayer)) {
						area.map[i,j].walkable = false;
					}else{
						area.map[i,j].walkable = true;
					}	
				}
			}
		}
	}



	public Node Vector3ToNode(Vector3 point){
		Node[,]  map = area.map;
		float startX = transform.position.x;
		float startZ = transform.position.z;
		
		int x = (int)( (point.x - startX) / area.tileSize) ;
		int z = (int)( (point.z - startZ) / area.tileSize) ;
		
		Node node = map[x, z];
		if( !node.walkable ){
			float distToNode = Mathf.Infinity;
			float distNear	 = Mathf.Infinity;
			for(int i=0; i<area.tilesInX; i++){
				for(int j=0; j<area.tilesInZ; j++){
					if(map[i,j].walkable){
						distToNode = Vector3.Distance(point,map[i,j].pos);
						if(distToNode < distNear){
							distNear = distToNode;
							node = map[i,j];
						}
					}
				}
			}
		}
		return node;
	}



	[HideInInspector] public bool showGizmo=true;
	[HideInInspector] public bool showNodes=true;
	[HideInInspector] public bool showLinks=true;
	[HideInInspector] public bool showUnwalkableNodes=false;
	[HideInInspector] public Color colorNode = Color.white;
	[HideInInspector] public Color colorLinks = Color.magenta;
	[HideInInspector] public Color colorUnwalkableNode = Color.red;



	void OnDrawGizmos(){
		float GizmoNodeSize = 0.2f;
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube ( transform.position + new Vector3(((float)area.tilesInX*area.tileSize)/2,(float)(area.height)/2,((float)area.tilesInZ*area.tileSize)/2), new Vector3(area.tilesInX*area.tileSize,area.height*area.tileSize,area.tilesInZ*area.tileSize));
		Gizmos.DrawCube (transform.position , Vector3.one);
		
		Vector3 yOffset = new Vector3 (0, 0.01f, 0);
		
		if(showGizmo){
			if(area.map!=null){
				for(int i=0; i<area.tilesInX; i++){
					for(int j=0; j<area.tilesInZ; j++){
						if(area.map[i,j].walkable){
							if(showNodes){
								Gizmos.color = colorNode;
								Gizmos.DrawWireCube(area.map[i,j].pos + yOffset,new Vector3(GizmoNodeSize,0,GizmoNodeSize));
							}
							if(showLinks){
								foreach(Node n in area.map[i,j].neighbour){
									if(n.walkable){
										Gizmos.color = colorLinks;
										Gizmos.DrawLine(area.map[i,j].pos+yOffset,n.pos+yOffset);
									}
								}
							}
						}
						else{
							if(showUnwalkableNodes){
								Gizmos.color = colorUnwalkableNode;
								Gizmos.DrawWireCube(area.map[i,j].pos + yOffset,new Vector3(GizmoNodeSize,0,GizmoNodeSize));
							}
						}
					}
				}
			}			
		}
	}



	//*********************************************************************************************************************************************************************************
	//*********************************************************************************************************************************************************************************
	//*********************************************************************************************************************************************************************************




	static List<Agent> agentsToSearchPath = new List<Agent>();
	[HideInInspector] public int agentsToFindPathEveryFrame = 20;
	
	
	void Update(){
		int agentsCount  = 0;
		while( agentsToSearchPath.Count>0  && agentsCount<agentsToFindPathEveryFrame){
			Agent agentToSearchPath = agentsToSearchPath[0];
			agentToSearchPath.startNode	= Vector3ToNode( agentToSearchPath.pivot.transform.position );
			AStar(agentToSearchPath); 
			agentToSearchPath.search = false;
			agentsToSearchPath.RemoveAt(0);
			agentsCount+=1;
		}
	}
	
	
	
	public static void SearchPath(Agent agent , Vector3 target){
		agent.endNode 	= PathfindingEngine.Instance.Vector3ToNode( target );
		if(!agent.search){
			agent.search = true;
			agentsToSearchPath.Add (agent);
		}
	}
	
	
	
	
	public void AStar(Agent agent){
		Node node = agent.startNode;
		Heap heap = new Heap ();
		bool loop = true;
		
		while(loop){
			
			heap.closeList.Add(node);
			node.state = State.Close;
			
			for(int i=0; i<node.neighbour.Length; i++){
				if( node.neighbour[i].walkable){
					if( node.neighbour[i].state == State.Clear ){
						node.neighbour[i].G = node.G + node.neighbour[i].cost;
						node.neighbour[i].H = Vector3.Distance(node.neighbour[i].pos, agent.endNode.pos);
						node.neighbour[i].F = node.neighbour[i].G + node.neighbour[i].H;
						node.neighbour[i].parent = node;
					}else if( node.neighbour[i].state == State.Open ){
						float tempG = node.G+node.neighbour[i].cost;
						if( node.neighbour[i].G > tempG ){
							node.neighbour[i].parent = node;
							node.neighbour[i].G = tempG;
							node.neighbour[i].F = node.neighbour[i].G + node.neighbour[i].H;
						}
					}
				}
			}
			foreach(Node neighbour in node.neighbour){
				if(neighbour.state == State.Clear  &&  neighbour.walkable) {
					neighbour.state = State.Open;
					heap.Manager("insert",neighbour);
				}
			}
			if(heap.openList.Count == 0) {
				loop=false;
			}else{ 
				node = heap.openList[0];
				heap.Manager("remove0");
			}
			if(node==agent.endNode) {loop=false;}
			
		}
		
		if(heap.openList.Count != 0){ // If path is found....
			//Add all nodes from startNode to endNode in array p.
			List<Vector3> p = new List<Vector3>();
			while(node!=null){
				p.Add(node.pos);
				node = node.parent;
			}
			
			//Invert p.
			List<Vector3> pInverted = new List<Vector3>();
			for(int i=p.Count-1; i>=0; i--){
				pInverted.Add(p[i]);
			}
			agent.path = pInverted;	
		}else{ // If path is not found agent path is empty.
			agent.path = new List<Vector3>();
		}
		
		area.ResetMap();
	}



	
}

