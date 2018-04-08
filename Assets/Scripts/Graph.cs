using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {

    public float maxNodeDistance = 10.0f;
    public float updateGraphSeconds = 2.0f;

    private List<Node> nodes = new List<Node>();

    void Start() {
        AddNodeGameObjects();
        StartCoroutine("UpdateGraphSolution");
    }

    void Update() {
    }

    void AddNodeGameObjects() {
        GameObject[] nodeGameObjects = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject nodeGameObject in nodeGameObjects) {
            AddNodeGameObject(nodeGameObject);
        }
    }

    void AddNodeGameObject(GameObject nodeGameObject) {
        Node node = nodeGameObject.GetComponent<Node>();
        nodes.Add(node);
    }

    void ConnectGameObjects() {
        foreach (Node node in nodes) {
            node.Reset();
        }
        foreach (Node nodeA in nodes) {
            foreach (Node nodeB in nodes) {
                if (nodeA != nodeB) {
                    float distance = Vector3.Distance(nodeA.transform.position, nodeB.transform.position);
                    if (distance <= maxNodeDistance) {
                        nodeA.AddConnection(nodeB);
                        nodeB.AddConnection(nodeA);
                        Debug.DrawLine(nodeA.transform.position, nodeB.transform.position, Color.red, updateGraphSeconds);
                    }
                }
            }
        }
    }

    List<Node>BuildPathList(Node end) {
        List<Node> path = new List<Node>();
        if (end.parent != null) {
            Node node = end;
            while (node != null) {
                path.Add(node);
                node = node.parent;
            }
        }
        return path;
    }

    List<Node> FindPath(Node start, Node end) {
        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(start);
        float totalCost = start.SetCostAndParent(0.0f, null);
        while (queue.Count > 0) {
            Node currentNode = queue.Dequeue();
            currentNode.visited = true;
            Node leastCostNode = currentNode.GetLeastCostConnectedNode(totalCost);
            if (leastCostNode != null) {
                totalCost = leastCostNode.SetCostAndParent(totalCost, currentNode);
            }
            foreach (Node node in currentNode.GetConnectedNodes()) {
                if (!node.visited) {
                    queue.Enqueue(node);
                    node.visited = true;
                    node.parent = currentNode;
                }
            }
        }
        return BuildPathList(end);
    }

    IEnumerator UpdateGraphSolution() {
        while (true) {
            ConnectGameObjects();
            Node start = null;
            Node end = null;
            foreach (Node node in nodes) {
                if (node.isStartNode) {start = node;}
                if (node.isEndNode) {end = node;}
            }
            if (start != null && end != null) {
                List<Node> path = FindPath(start, end);
                if (path != null && path.Count > 0) {
                    Node lastNode = path[0];
                    foreach (Node node in path) {
                        Debug.DrawLine(lastNode.transform.position, node.transform.position, Color.green, updateGraphSeconds);
                        lastNode = node;
                    }
                }
            }
            yield return new WaitForSeconds(updateGraphSeconds);
        }
    }

}
