B481 / Fall 2022
Problem Set 05
Zachary Graber (zegraber@iu.edu)

1. The unity environment I'm using is standard operating procedure by now. Standard version, and everything is contained in logical folders. Obviously, some of the scripts and shaders were reused (and heavily modified) from Problem Set 04 and a lab or two. There are no assets from outside the project/class.

2. The only part of this problem set I'm questioning that I didn't complete is the "3D model object defined algorithmically, e.g. a regular solid." I have cubes defined manually with a mesh (verts and tris) in the `GetCubeMesh()` function of `MeshLogic.cs`, but I'm not sure if that's "non-trivial" enough.

3. Here's a quick explanation of what's going on in the project. In the scene "PS05," you should see a light (represented by a cube of the light's diffuse color) orbiting around some meshes. The red and blue cube are mostly to demonstrate the difference between a material with more specularity (blue cube) and less (red).

There are two grey meshes. They are both generated in my scripts and their height mapped in object space with Perlin noise. The one on the right is generated entirely on the CPU, while the one on the left uses a texture-height-map passed to the shader, where the object space heights are calculated. The GPU-determined mesh has notably lower-resolution lighting. This is due mostly to the finite precision in texture sampling. When the mesh's height is calculated in the vertex shader, it invalidates any vertex normals that were there. They therefore need to be recalculated in order to solve my Phong shading. I accomplish this by, in the fragment shader, passing/interpolating the object space position and sampling two object space points close to it: one in the positive x-direction and one in the positive z-direction. I then use these points to find 2 vectors approximately tangent to the surface, and can then get a normal vector by taking their cross product. 