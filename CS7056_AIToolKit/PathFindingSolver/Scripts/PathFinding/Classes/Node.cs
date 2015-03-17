﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CS7056_AIToolKit
{
    public enum NodeState { Clear, Open, Close };

    /// <summary>
    /// Node Class to represent a Node in the A* Grid
    /// </summary>
    [HideInInspector]
    public class Node
    {
        public Vector3 pos;
        public Node[] neighbourNodes = new Node[0];
        public float cost;
        public Node parent;
        public bool walkable = true;
        public bool walkableInitial = true;
        public float G;
        public float H;
        public float F;
        public NodeState state = NodeState.Clear;

        public Node() { }

        public Node(Vector3 _pos)
        {
            pos = _pos;
        }

        public void Reset()
        {
            state = NodeState.Clear;
            parent = null;
        }

    }
}