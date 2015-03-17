﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace CS7056_AIToolKit
{
    /// <summary>
    /// This is an agent component that should be attached to a path finding seeker
    /// </summary>
    public class PathFindingAgent : MonoBehaviour
    {

        public Agent agent = new Agent(5, 0, true);
        private int stepsToSmooth = 4;
        private int stepsToCheck = 6;
        public bool smoothPath = true;
        private LayerMask dynamicObstacleLayer;
        private LayerMask agentLayer;
        private float height = 1;
        private Node nodeWithAgent;
        private float radius = 0.5f;
        private bool agentAvoidance = true;
        private bool aceleration = false;

        void Start()
        {
            agent.Launch(transform);
            dynamicObstacleLayer = PathFindingSolver.Instance.dynamicObstacleLayer;
            agentLayer = PathFindingSolver.Instance.agentLayer;
            height = PathFindingSolver.Instance.world.height;
            radius = PathFindingSolver.Instance.world.tileSize * 0.45f;
            agentAvoidance = PathFindingSolver.Instance.agentAvoidance;
        }


        void Update()
        {
            if (agent.path.Count > 0)
            {
                bool walkable = true;
                //Check if exist some dynamic obstacle in our path.
                int stepsCount = 0;
                foreach (Vector3 step in agent.path)
                {
                    if (stepsCount < stepsToCheck)
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(step + new Vector3(0, height / 2, 0), Vector3.down, out hit, height, dynamicObstacleLayer))
                        {
                            walkable = false;
                            break;
                        }
                    }
                    else { break; }
                    stepsCount++;
                }

                if (agentAvoidance)
                {
                    stepsCount = 0;
                    Vector3 stepBefore = agent.pivot.transform.position;
                    foreach (Vector3 step in agent.path)
                    {
                        if (stepsCount < stepsToCheck / 2)
                        {
                            RaycastHit hit;
                            Vector3 dir = step - stepBefore;
                            float dist = Vector3.Distance(step, stepBefore);
                            if (Physics.SphereCast(step + new Vector3(0, 0.1f, 0), radius, dir, out hit, dist, agentLayer))
                            {
                                walkable = false;
                                nodeWithAgent = PathFindingSolver.Instance.GetNodeFromPosition(step);
                                nodeWithAgent.walkable = false;
                                aceleration = true;
                            }
                            stepBefore = step;
                        }
                        else { break; }
                        stepsCount++;
                    }
                }

                if (walkable)
                {
                    //Smooth path
                    List<Vector3> Points = new List<Vector3>();
                    stepsCount = 0;
                    foreach (Vector3 step in agent.path)
                    {
                        if (stepsCount < stepsToSmooth)
                        {
                            Points.Add(step);
                        }
                        else { break; }
                        stepsCount++;
                    }
                    if (smoothPath) { Points = MakeSmooth(Points); }
                    for (int i = stepsToSmooth; i < agent.path.Count; i++) { Points.Add(agent.path[i]); }
                    agent.path = Points;
                    //Agent go to next step
                    GotoNextStep();
                }
                else
                {
                    //Re-Find the path.
                    agent.GoTo(agent.endNode.pos);
                }
            }
        }



        public void GotoNextStep()
        {
            //if there's a path.
            if (agent.path.Count > 0)
            {
                bool nextStep = false;


                //Correction to solve the step back that appeared if we generate new target points in small time.
                if (agent.path.Count > 1)
                {
                    Vector3 pa = agent.pivot.transform.position;
                    Vector3 p0 = agent.path[0];
                    Vector3 p1 = agent.path[1];
                    float angleTo0 = HorizontalAngle(pa.x, pa.z, p0.x, p0.z);
                    float angleTo1 = HorizontalAngle(pa.x, pa.z, p1.x, p1.z);
                    if (Mathf.Abs(angleTo0 - angleTo1) > 44)
                    {
                        if (Vector3.Distance(pa, p1) < Vector3.Distance(p1, p0))
                        {
                            agent.path.RemoveAt(0);
                            nextStep = true;
                        }
                    }
                }


                if (!nextStep)
                {
                    //Get the next waypoint...
                    Vector3 point = agent.path[0];
                    //...and rotate pivot towards it.
                    Vector3 dir = point - agent.pivot.transform.position;
                    if (dir != Vector3.zero)
                    {
                        agent.pivot.transform.rotation = Quaternion.Slerp(agent.pivot.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 15);
                    }
                    //Calculate the distance between current pivot position and next waypoint.
                    float dist = Vector3.Distance(agent.pivot.transform.position, point);
                    //Move towards the waypoint.
                    Vector3 direction = (point - agent.pivot.transform.position).normalized;
                    float speed = agent.speed;
                    if (aceleration)
                    {
                        speed = 3 * agent.speed;
                        aceleration = false;
                    }
                    agent.pivot.transform.Translate(direction * Mathf.Min(dist, speed * Time.deltaTime), Space.World);
                    //Assign transform position with height and pivot position.
                    transform.parent = agent.pivot.transform;
                    transform.position = agent.pivot.transform.position + new Vector3(0, agent.yOffset, 0);
                    if (dir != Vector3.zero)
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 15);
                    }
                    //If the agent arrive to waypoint position, delete waypoint from the path.
                    if (Vector3.Distance(agent.pivot.transform.position, point) < 0.1f)
                    {
                        agent.path.RemoveAt(0);
                    }
                }
            }
        }




        public float HorizontalAngle(float X1, float Y1, float X2, float Y2)
        {
            if (Y2 == Y1) { return (X1 > X2) ? 180 : 0; }
            if (X2 == X1) { return (Y2 > Y1) ? 90 : 270; }
            float tangent = (X2 - X1) / (Y2 - Y1);
            double ang = (float)Mathf.Atan(tangent) * 57.2958;
            if (Y2 - Y1 < 0) ang -= 180;
            return (float)ang;
        }




        public List<Vector3> MakeSmooth(List<Vector3> _points)
        {
            int curvedLength = ((_points.Count) * Mathf.RoundToInt(1.4f)) - 1;
            List<Vector3> curvedPoints = new List<Vector3>(curvedLength);

            for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedLength + 1; pointInTimeOnCurve++)
            {
                float t = Mathf.InverseLerp(0, curvedLength, pointInTimeOnCurve);

                for (int j = curvedLength; j > 0; j--)
                {
                    for (int i = 0; i < j; i++)
                    {
                        _points[i] = (1 - t) * _points[i] + t * _points[i + 1];
                    }
                }
                curvedPoints.Add(_points[0]);
            }

            return (curvedPoints);
        }



        //draw  current path.
        public bool showPath = true;
        public bool fadeoutPath = false;
        public Color PathColor = Color.green;
        void OnDrawGizmos()
        {

            if (showPath)
            {
                if (agent.path != null && agent.path.Count > 0)
                {
                    Vector3 offset = new Vector3(0, 0.1f, 0);
                    Gizmos.color = PathColor;
                    Gizmos.DrawLine(transform.position + offset, agent.path[0] + offset);
                    for (int i = 1; i < agent.path.Count; ++i)
                    {
                        if (fadeoutPath && i > stepsToSmooth - 2)
                            Gizmos.color = new Color(1 - PathColor.r, 1 - PathColor.g, 1 - PathColor.b, 0.1f);
                        Gizmos.DrawLine(agent.path[i - 1] + offset, agent.path[i] + offset);
                    }
                }
            }
        }
    }
}