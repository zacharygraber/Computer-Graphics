## B481 / Fall 2022 <br /> Lab 08 <br /> Zachary E Graber <br /> zegraber@iu.edu

- I completed steps 1 and 2 of Task A

### Question Responses

1. Per-vertex data is passed to the vertex shader in a structure, defined by you. Typically, this is called `struct appdata`, and per the [unity documentation](https://docs.unity3d.com/Manual/SL-VertexProgramInputs.html) it can include:

    - `POSITION`, as a `float3` or `float4`
    - `NORMAL`
    - `TEXCOORD0`
    - `TEXCOORD1`, etc.
    - `TANGENT`
    - `COLOR`, typically as a `float4` (presumably `(r,g,b,a)`)
2. Data is usually given as output from the vertex shader in a `struct v2f` (**v**ertex to **f**ragment). Per [unity docs](https://docs.unity3d.com/Manual/SL-ShaderSemantics.html), this needs to have a `float4` called `SV_POSITION`. This can also include whatever other "varyings" you want (produced by the vertex shader, such as color), but these values will be interpolated between vertices and passed as inputs to the fragment shader.
