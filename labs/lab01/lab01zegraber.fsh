bool isOnCircle(float r, vec2 center, vec2 point);

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    // Problem 1: Set entire output to black
		//fragColor = vec4(0,0,0,1);
    
    // Problem 2: Set entire output to white
		//fragColor = vec4(1,1,1,1);
    
    // Problem 3: Set to 3 different colors by changing only 1 RGBA value:
    
		// a) Red
			//fragColor = vec4(1,0,0,1);
    
		// b) Green
			//fragColor = vec4(0,1,0,1);
    
		// c) Blue
			//fragColor = vec4(0,0,1,1);
			
	// Just for fun: here's a customizable filled circle I came up with
    
		// Circle parameters
		float radius = 150.0;
		vec2 centerPoint = vec2(400,225);
		vec3 circleColor = vec3(1,1,1);
		vec3 backgroundColor = vec3(0,0,0);

		// Circle logic
		vec3 outColor = backgroundColor;
		if (isOnCircle(radius, centerPoint, fragCoord)) {
			outColor = circleColor;
		}
		fragColor = vec4(outColor, 1);
}

// Helper function for the circle
bool isOnCircle(float r, vec2 center, vec2 point) {
    float diff = pow(point.x - center.x, 2.0) + pow(point.y - center.y, 2.0) - pow(r,2.0);
    return diff < 1.0;
}