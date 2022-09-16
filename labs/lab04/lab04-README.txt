B481 / Fall 2022
Lab 04
Zachary Graber
zegraber

I completed the task by following the directions pretty much exactly. I used localPosition, localEulerAngles, and localScale for everything, and it seems to work fine. The provided animation is commented out.

I added an extra animation of the hand opening and closing; I accomplished this using two local variables `counter` and `period`, which essentially just counts from -period <= counter <= period, and use the sign of the counter to determine which way to rotate.
