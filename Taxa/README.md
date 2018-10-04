# TaxaCDC

The TaxaCDC web tool reads, manipulates, and queries taxonomy files.

## Data

### Data format

The data format is described in full at ftp://ftp.ncbi.nih.gov/pub/taxonomy/taxdump\_readme.txt.
These data describe taxonomy as represented by a tree.  Each line in `nodes.dmp`
has three important fields
* taxid - the taxonomy node identifier.
* parentid - this node\'s parent node.
* rank - a label for the class of node.  For example: "species".

There is a one-to-many relationship between `nodes.dmp` and `names.dmp`
so that many lines in `names.dmp` can describe a single node.  These
entries can describe taxonomy authorities for each node (e.g., who
first published on a given species). They can also describe the 
scientific name of the organism or synonyms.

All data are delimited by three characters `\t|\t` and each line ends with `|`.

### Comprehensive data

Taxonomy files are hosted at NCBI at ftp://ftp.ncbi.nih.gov/pub/taxonomy.  The main
files are `nodes.dmp` and `names.dmp`.  We have saved the taxonomy files from
October 4, 2018 in `data/nodes.dmp` and `data/names.dmp`.

### Test data

Test data can be obtained from `data/names.vibrio.dmp` and `data/nodes.vibrio.dmp`.
A perl script is also included in `data/` to show how we obtained these sample
data from the larger data hosted at NCBI.

## HOWTO

To start, open the URL at ____

### Delete taxon 

### Add taxon

### Query taxon

