B481 / Fall 2022
Lab 14
Zachary Graber (zegraber@iu.edu)

For the lab task, I implemented a non-flat surface, generated algorithmically. Specifically, I have a scalable "flat" mesh/plane that gets height-mapped to Unity's built-in Perlin noise function. What that means is that, currently, everything is happening on the CPU, but it would be simple to port the vertex height mapping to a vertex shader.

In the Unity project I included, you should find a game object that has my script attached to it. For the sake of the lab alone, I haven't integrated this into my own rendering pipeline yet (final project, started in PS04), but that should be dead simple. It just uses Unity's default diffuse material. Further, there is no camera control or anything in the actual Unity game scene, but everything should be visible in the editor window in play mode, especially if you select wireframe+shaded (or whatever it's actually called).

I plan to use this functionality in 2 places in my final project:
1. A "main" generated terrain and
2. scrolling the sample offset of the perlin noise gives a nice water-wave effect.