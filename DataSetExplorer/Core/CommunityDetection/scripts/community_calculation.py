from networkx.algorithms import community
from infomap import Infomap

import infomap
import sys
import json
import networkx as nx
import math
import os

def calculate_communities(nodes, links, algorithm):
    graph = nx.Graph()
    graph.add_nodes_from(nodes)
    extracted_links = extract_links(links)
    graph.add_edges_from(extracted_links)
    coms = None
    if algorithm == "infomap":
        im = Infomap(silent=True)
        for edge in extracted_links:
            im.add_link(nodes.index(edge[0]), nodes.index(edge[1]))
        im.run()
        for node in im.tree:
            if node.is_leaf:
                print(nodes[node.node_id], node.module_id-1)
    elif algorithm == "label_propagation":
        coms = community.label_propagation_communities(graph)
        print_to_cli(coms)
    else:
        coms = community.girvan_newman(graph)
        print_newman(coms)
    return coms
    

def extract_links(links):
    ret = []
    for link in links:
        source = link["Source"]
        target = link["Target"]
        l = (source, target)
        ret.append(l)
    return ret


def print_to_cli(coms):
    i = 0
    for com in coms:
        for j in com:
            print(j, i)
        i += 1
        


def print_newman(coms):
    tup = tuple(sorted(c) for c in next(coms))
    for community in range(len(tup)):
        for i in range(len(tup[community])):
            print(tup[community][i], community)



if __name__ == "__main__":
    nodes_file = sys.argv[1]
    links_file = sys.argv[2]
    nodes = []
    with open(nodes_file) as f:
        nodes = json.load(f)
    links = []
    with open(links_file) as f:
        links = json.load(f)
    algorithm = sys.argv[3]
    calculate_communities(nodes["nodes"], links["links"], algorithm)