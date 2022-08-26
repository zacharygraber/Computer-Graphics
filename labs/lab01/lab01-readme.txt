B481 / Fall 2022 Lab 01 Zachary Graber (zegraber)
--------------------------------------------------------------------------------------
How I organize my IU GitHub repository for B481:

Exactly how it was explained! I created folders "labs," "hw," and "problem-sets"
for each type of submission, and plan to use them as such. Inside these folders, I 
have/will create[d] subfolders for individual assignments, following the "hw01," 
"lab01," etc convention.

--------------------------------------------------------------------------------------

How I completed my Shadertoy task:

First of all, I made an account:
-username: zegraber
-email: zegraber@iu.edu

For each of the tasks, the solution is relatively the same. Since we want to set the
entire graphics context to a certain color, we can completely disregard the input 
(location); the output is constant, not dependent on the input at all.

As such, for each I just set the output (`fragColor`) to a new 4-value vector (vec4)
with the RGBA values corresponding to that color (just ignoring Alpha, leaving it at 1). 

So for...
black: (0,0,0,1)
white: (1,1,1,1)
red:   (1,0,0,1)
green: (0,1,0,1)
blue:  (0,0,1,1)