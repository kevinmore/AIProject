using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CS7056_AIToolKit
{
    public class Heap
    {
        public List<Node> closeList = new List<Node>();
        public List<Node> openList = new List<Node>();
        public List<float> heapList = new List<float>();

        public enum Operation { Insert, RemoveFirst };

        public void Configure(Operation op, Node NodeToInsert = null)
        {
            if (op == Operation.Insert)
            {
                openList.Add(NodeToInsert);
                heapList.Add(NodeToInsert.F);
                int pos = heapList.Count - 1;

                while (true)
                {
                    int parent = (pos - 1) / 2;
                    if (heapList[pos] <= heapList[parent])
                    {
                        float tempF = heapList[pos];
                        heapList[pos] = heapList[parent];
                        heapList[parent] = tempF;
                        Node tempFNode = openList[pos];
                        openList[pos] = openList[parent];
                        openList[parent] = tempFNode;
                        pos = parent;
                    }
                    else
                        break;
                    if (pos <= 0)
                        break;
                }

            }
            else if (op == Operation.RemoveFirst)
            {
                openList[0] = openList[openList.Count - 1];
                openList.RemoveAt(openList.Count - 1);
                heapList[0] = heapList[heapList.Count - 1];
                heapList.RemoveAt(heapList.Count - 1);
                int pos = 0;

                while (true)
                {
                    int pos1 = pos * 2 + 1;
                    int pos2 = pos * 2 + 2;
                    int pos3 = pos1;

                    if (pos2 < heapList.Count)
                    {
                        if (heapList[pos1] >= heapList[pos2])
                        {
                            pos3 = pos2;
                        }
                    }
                    if (pos3 < heapList.Count && heapList[pos] >= heapList[pos3])
                    {
                        float tempF = heapList[pos];
                        heapList[pos] = heapList[pos3];
                        heapList[pos3] = tempF;
                        Node tempFNode = openList[pos];
                        openList[pos] = openList[pos3];
                        openList[pos3] = tempFNode;
                        pos = pos3;
                    }
                    else
                        break;
                    if (pos >= heapList.Count)
                        break;
                }

            }
        }


    }
}