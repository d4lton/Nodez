using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

	public bool isStartNode = false;
	public bool isEndNode = false;

    public Node parent = null;
    public bool visited = false;

	HashSet<Node> connectedNodes = new HashSet<Node>();

	void Start() {
	}
	
	void Update() {
	}

	public void AddConnection(Node node) {
		connectedNodes.Add(node);
	}

	public void Reset() {
		connectedNodes.Clear();
        parent = null;
        visited = false;
	}

	public HashSet<Node> GetConnectedNodes() {
		return connectedNodes;
	}

    public float GetCost(Node node, float totalCost = 0.0f) {
        if (node != null) {
            return Vector3.Distance(transform.position, node.transform.position) + totalCost;
        } else {
            return totalCost;
        }
    }

    public Node GetLeastCostConnectedNode(float totalCost) {
        float leastCost = Mathf.Infinity;
        Node leastCostNode = null;
        foreach (Node node in connectedNodes) {
            if (!node.visited) {
                float cost = GetCost(node, totalCost);
                if (cost < leastCost) {
                    leastCost = cost;
                    leastCostNode = node;
                }
            }
        }
        return leastCostNode;
    }

    public float SetCostAndParent(float totalCost, Node parent) {
        this.parent = parent;
        return GetCost(parent, totalCost);
    }

}
