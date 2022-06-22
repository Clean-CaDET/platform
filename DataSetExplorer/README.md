## Requirements for Annotation Consistency tests
1. Python3 (developed with `Python3.8.x`)
2. virtualenv (for virtual environments): `pip install virtualenv`

## Setup
1. Move into folder platform\DataSetExplorer\AnnotationConsistency\
2. Create virtual environment: `virtualenv -p python3 venv`
3. Activate virtual environment:
   - (Unix) `source venv/bin/activate`
   - (Windows) Move into venv/Scripts folder and activate virtual environment: `activate`
4. In folder platform\DataSetExplorer\AnnotationConsistency\ install requirements: `pip install -r requirements.txt'

## Additional Setup for community detection
1. Move into folder platform\DataSetExplorer\Core\CommunityDetection\
2. Create virtual environment: `virtualenv -p python3 community`
3. Activate virtual environment:
   - (Unix) `source community/bin/activate`
   - (Windows) Move into community/Scripts folder and activate virtual environment: `activate`
4. In folder platform\DataSetExplorer\Core\CommunityDetection\ install requirements: `pip install networkx'
5. In folder platform\DataSetExplorer\Core\CommunityDetection\ install requirements: `pip install infomap'
