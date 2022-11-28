B481 / Fall 2022
Problem Set 04
Zachary E Graber (zegraber@iu.edu)

1. I used the standard Unity version for this class. The project files should be laid out in folders that make sense (i.e. all C# scripts are in /assets/scripts, all shaders in /assets/shaders, etc.). Aside from a small amount of code copied from previous assignments for this class (and likely heavily modified), everything in this project is brand-new. 

2. Currently, I believe that everything short of the lighting bonus should be implemented. The cube and coordinate axes are created programmatically with a Unity Mesh, then assigned a custom material, which then passes their information to a custom shader. The whole process from mesh creation, to modeling transformation, to viewing transformation, to projection, is implemented. The shader simply colors vertices based on their normals (so I used a 24-vertex cube to get around interpolation) to achieve the effect of coloring each face of the cube and the coordinate axes. 

3. I don't think there's anything extraordinary in here. I kept encountering a strange issue with the projection matrix I'm using, where everything's z-depth seems to be inverted (so the furthest things away are rendered on top of closer objects). A temporary solution was just forcing the z-depth to a constant, since that shouldn't matter for the scope of this assignment. I'll have to fix that in the future, though.