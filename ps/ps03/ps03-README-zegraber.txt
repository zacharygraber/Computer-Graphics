B481 / Fall 2022
Problem Set 03
Zachary Graber
zegraber@iu.edu

- Unity version: 2020.3.16f1
- All scripts are contained in Assets/Scripts, including:
	- DragObject.cs (included with PS02)
	- SplineDerivatives.cs (A helper script to contain derivative-calculation and display junk)
	- SplineParameters.cs (copied from PS02, changed namespace)
	- SplineSegmentGPUCompute.cs (starter code from PS03)

- I completed all the mandatory parts of the assignment (I hope), but not the bonus.
- Thing I'm proud of:
	- The UI stays anchored decently well
	- Separate toggles for 1st and 2nd derivative
	- I noticed it was hard to look at the derivatives since they were very long, so I added an option to normalize them

*Just because I felt like it*, I built the project for WebGL and threw it online:
https://zegraber.pages.iu.edu/b481/splines/