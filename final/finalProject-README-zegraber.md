## B481 / Fall 2022
## Final Project
### Zachary Graber
### zegraber@iu.edu
---

The prompt for this README says to refer to my written proposal for HW 5, then goes on to say "as explained in the tasks below." There are no tasks below, and the instructions are a bit unclear. I am interpreting this to mean to go through my original proposal and compare it to my final product.

**3D Scene**: 
- Due to severe time constrains, I didn't end up wrapping the map around a sphere, nor height-mapping a sphere instead of a flat surface. 
- I indeed ended up implementing height mapping with Perlin noise, and I am very pleased with how it turned out. A Perlin noise texture is generated in the CPU, then passed to the GPU. Inside, the vertex shader computes the mesh's object-space height by sampling that texture (according to several optional parameters like offsets, scales, etc.). The major complication is that computing new vertex positions in the vertex shader means the mesh's normals have to be recalculated. The simplest way I could find to do so was by using what are essentially partial derivatives. This re-sampling of normals on a finite-resolution height map means that you can see artifacting in the lighting, especially at high resolutions, but overall the effect is good.
- Animation was implemented by animating the Perlin noise parameters over time, giving a running/active water effect. Like some other students mentioned in their presentations, I could have probably achieved a better effect with Gerstner waves or similar, but this solution is *much* simpler since I had already implemented Perlin noise sampling.

**Camera**:
- The two camera modes I mentioned in my original proposal are fairly close to what I've ended up with. 
    1. The first, a first-person flying mode, is what I've implemented (only more thoroughly) with the "Minecraft Creative" camera mode in my scene. 
    2. The second is my orbit mode, which circles the camera around the scene

**Interaction**:
- I ended implementing a third camera mode, which positions the camera in a static location, then lets the user rotate the scene. This isn't quite the rolling ball that I had planned, since the movement only happens around the y-axis. I chose to take this approach since rotation in other directions isn't really desirable for my scene (for example, you wouldn't want to see the under-side of it).
- Users can interact with the static lights near the ground by clicking on/near them to toggle them on and off. This feature is admittedly a little buggy for some reason, since sometimes my script thinks the light isn't quite in the right screen-space coordinate.

**Illumination**:
- My proposal tells *almost* the whole story. I implemented Phong shading (never got around to changing it to use the Blinn optimization). I also implemented a basic distance attenuation. 
- The light sources I ended up including are a "Sun" that circles around the scene and two less powerful [other] point lights colored differently. The main goal with the 2 additional lights was to demonstrate the specular highlights off the surface of the water, as compared to the land.
- The lights in my scene can be turned off or adjusted through user interaction (in the case of the static ones), or via the Unity editor (for the "Sun").

**Mapping**:
- Texture mapping is used on the land and water meshes to make them look not so drab. All (both) the textures used in the scene were made by me, using Photoshop.
- I did not have time to implement bump mapping. This is a bit unfortunate, since I think it would have improved, for example, the look of the land by a lot if I used something like a rocky texture.
- The water texture needed to span the entire scene. I'm neither skilled nor artistic enough to make a tileable texture, so I just made it super high-resolution, which explains its absurd file size (sorry!).

**UI and Help Screen**:
- I forgot to mention the help screen in my original proposal
- For the sake of simplicity, I just created a static image help screen with info and controls, then used Unity's Canvas/UI system to draw it on the screen.
- Users can press "h" to toggle the help screen, or click the button in the top-right. 
- Users can swap through the 3 camera modes by clicking the button in the bottom-left; however, the first-person flying mode keeps the cursor captive, meaning either the 'C' key or 'H' key has to be used to change.

---

## Other notes

I don't have much more to say other than that this project took me an incredible amount of time/effort/debugging, and I'm proud of how it turned out, even if there are some things I wish I had more time to improve. 

It's been an incredible semester, and I've had a lot of fun with this class.