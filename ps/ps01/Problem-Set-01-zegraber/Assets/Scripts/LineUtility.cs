/*  CSCI-B481/B581 - Fall 2022 - Mitja Hmeljak
    This script provides a library of "utility" methods,
    that may be useful to solve Problem Set 01.
    
    However, you may have to complete the parts marked as TODO ...
    Original demo code by CSCI-B481 alumnus Rajin Shankar, IU Computer Science.
 */

using UnityEngine;

namespace PS01 {

    public static class LineUtility {
    
        // DirectionNormal() --- returns the normal to a given direction vector:
        public static Vector2 DirectionNormal(Vector2 direction) {
            Vector2 normal = (Vector2.Perpendicular(direction)).normalized;
            return normal;
        } // end of DirectionNormal() 

        // LineSegmentNormal() --- returns the normal to a line segment:
        public static Vector2 LineSegmentNormal(Vector2 start, Vector2 end) {
            Vector2 direction = new Vector2(end.x - start.x, end.y - start.y); // Direction as a difference of points
            return DirectionNormal(direction);
        } // end of LineSegmentNormal()

        // Helper for below functions --- does a scalar projection of v (see inside of func) onto direction
        private static float FindLocalX(Vector2 pointOnLine, Vector2 direction, Vector2 point) {
            // We want to project the vector spanning from pointOnLine to point onto the direction vector.
            Vector2 v = new Vector2(point.x - pointOnLine.x, point.y - pointOnLine.y);
            return Vector2.Dot(v, direction.normalized);
        }

        // ClosestPointOnLine() --- returns the closest point on a line to a given query point:
        public static Vector2 ClosestPointOnLine(Vector2 pointOnLine, Vector2 direction, Vector2 point) {
            float localX = FindLocalX(pointOnLine, direction, point);
            // So the point on the line is given by pointOnLine + localX*direction
            return pointOnLine + (localX * direction.normalized);
        } // end of ClosestPointOnLine()


        // ClosestPointOnSegment() --- returns the closest point (on a line segment)
        //                             to a given subject point:
        public static Vector2 ClosestPointOnSegment(Vector2 start, Vector2 end, Vector2 point) {
            Vector2 direction = new Vector2(end.x - start.x, end.y - start.y);
            float localX = FindLocalX(start, direction, point);

            if (localX < 0) {
                return start;
            } else if (localX > direction.magnitude) {
                return end;
            } else {
                return ClosestPointOnLine(start, direction, point);
            }

        } // end of ClosestPointOnSegment()


        // // ClosestPointOnPolygon() --- returns the closest point (on a polygon)
        // //                             to a given subject point.
        // //  Note:
        // //      the polygon is given as array of transforms
        // //      with vertex[n-1] connecting back to vertex[0]
        // //
        public static Vector2 ClosestPointOnPolygon(Transform[] polygonVertices, Vector2 point) {
        
            Vector2 result = Vector2.zero;
            float minSqrDistance = float.PositiveInfinity;
            
            for (int i = 0; i < polygonVertices.Length; i++) {
                int j = (i + 1) % polygonVertices.Length;
                Vector2 side = polygonVertices[j].position - polygonVertices[i].position;
                float sideLength = side.magnitude;
                Vector2 sideDirection = side / sideLength;


                Vector2 pointOnPolygon;
                float localX = FindLocalX(polygonVertices[i].position, sideDirection, point);
                if (localX < 0) {
                    pointOnPolygon = polygonVertices[i].position;
                } else if (localX > sideLength) {
                    pointOnPolygon = polygonVertices[j].position;
                } else {
                    pointOnPolygon = ((Vector2) polygonVertices[i].position) + (localX * sideDirection);
                }

                Vector2 delta = point - pointOnPolygon;
                float sqrDistance = delta.sqrMagnitude;

                if (sqrDistance < minSqrDistance) {
                    result = pointOnPolygon;
                    minSqrDistance = sqrDistance;
                }
            }
            return result;
        } // end of ClosestPointOnPolygon()


    } // end of static class LineUtility

} // end of namespace PS01