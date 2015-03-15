using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CS7056_AIToolKit
{
    public enum State { Clear, Open, Close };

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
        public State state = State.Clear;

        public Node() { }

        public Node(Vector3 _pos)
        {
            pos = _pos;
        }

    }
}