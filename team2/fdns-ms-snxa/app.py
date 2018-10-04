from flask import Flask, Response
from flask import jsonify, request

import os.path

import osmnx as ox
from networkx.readwrite import json_graph

app = Flask(__name__)

def root_dir():  # pragma: no cover
    return os.path.abspath(os.path.dirname(__file__))

def get_file(filename):  # pragma: no cover
    try:
        src = os.path.join(root_dir(), filename)
        return open(src).read()
    except IOError as exc:
        return str(exc)

# ------------------------------------------------
#               / 
# ------------------------------------------------
@app.route('/')
def index():
    return 'Street Network Analysis'

# ------------------------------------------------
#               /basic_stats_from_point
# ------------------------------------------------
@app.route('/basic_stats_from_point', methods=['POST'])
def basic_stats_from_point():

    values = request.get_json()
    latitude = values.get('latitude')
    longitude = values.get('longitude')
    network_type = values.get('network_type')

    if network_type is None:
      return "Error, please supply a valid network_type", 400
    
    print(latitude, longitude)
    print(network_type)

    coord = (latitude, longitude)

    G = ox.graph_from_point(coord, network_type=network_type)

    basic_stats = ox.basic_stats(G)

    # ox.save_graphml(G, filename="graph_from_location.graphml", folder="/app")
    # content = get_file('graph_from_location.graphml')
    return Response(basic_stats, mimetype="application/json")


# ------------------------------------------------
#               /graph_from_point
# ------------------------------------------------
@app.route('/graph_from_point', methods=['POST'])
def graph_from_point():

    values = request.get_json()
    latitude = values.get('latitude')
    longitude = values.get('longitude')
    network_type = values.get('network_type')

    if network_type is None:
      return "Error, please supply a valid network_type", 400
    
    print(latitude, longitude)
    print(network_type)

    coord = (latitude, longitude)

    G = ox.graph_from_point(coord, network_type=network_type)

    ox.save_graphml(G, filename="graph_from_location.graphml", folder="/app/output")

    content = get_file('output/graph_from_location.graphml')
    return Response(content, mimetype="application/xml")

# ------------------------------------------------
#               /graph_from_place
# ------------------------------------------------
@app.route('/graph_from_place', methods=['POST'])
def graph_from_place():

    values = request.get_json()
    location = values.get('location')
    network_type = values.get('network_type')
    print(location)
    print(network_type)

    if location is None:
        return "Error, please supply a valid location", 400
    if network_type is None:
        return "Error, please supply a valid network_type", 400

    G = ox.graph_from_place(location, network_type=network_type)

    ox.save_graphml(G, filename="graph_from_place.graphml", folder="/app/output")

    #jdata = json_graph.tree_data(G,root=1)
    #graphml_json = json_graph.tree_graph(jdata)
    #return jsonify(graphml_json), 200
    # return 'ok', 200
    content = get_file('output/graph_from_place.graphml')
    return Response(content, mimetype="application/xml")


# ------------------------------------------------
#               app run
# ------------------------------------------------
if __name__ == '__main__':
    app.run(debug=True,host='0.0.0.0',  port=5000)
