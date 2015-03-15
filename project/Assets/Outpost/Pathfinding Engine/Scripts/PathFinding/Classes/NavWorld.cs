using UnityEngine;
using System.Collections;
namespace CS7056_AIToolKit
{
    /// <summary>
    /// Navigation World Class, which is a box that bounds the geometry world.
    /// The navigation map (A* grid) will be generated inside the world
    /// </summary>
    [System.Serializable]
    public class NavWorld
    {
        [HideInInspector]
        public Node[,] map;
        public int tilesInX;
        public int tilesInZ;
        public int tileSize;
        public float height;

        public NavWorld(int _tilesInX, int _tilesInZ, int _tileSize, float _height)
        {
            tilesInX = _tilesInX;
            tilesInZ = _tilesInZ;
            tileSize = _tileSize;
            height = _height;
        }

        public void ResetMap()
        {
            if (map == null) return;
            for (int i = 0; i < tilesInX; ++i)
                for (int j = 0; j < tilesInZ; ++j)
                    map[i, j].Reset();
        }
    }

}