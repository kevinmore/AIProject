using UnityEngine;
using System.Collections;

[System.Serializable]
public class Area{
	[HideInInspector] 	public Node[,] 	map;
						public int 	 	tilesInX;
						public int 	 	tilesInZ;
						public int	 	tileSize;
						public float 	height;

	public Area(int _tilesInX, int _tilesInZ, int _tileSize, float _height){
		tilesInX = _tilesInX;
		tilesInZ = _tilesInZ;
		tileSize = _tileSize;
		height   = _height;
	}

	public void ResetMap(){
		if(map==null) return;
		for(int i=0; i<tilesInX; i++){
			for(int j=0; j<tilesInZ; j++){
				map[i,j].state = State.Clear;
				map[i,j].parent = null; 
			}
		}
	}
}

