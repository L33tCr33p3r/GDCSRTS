A pathfinding flowfield is created and stored

A path through the flowfield is created and stored

Store the begining of the path as the "current position". For each node in the original flowfield path, starting from the end, ("node"), check all nodes on the flowfield between the node and the beginning and see if that path is traversable.

Once a flowfield solution has been found, each unit is assigned a point from the set of *n* closest points to the target (according to the distance map), where *n* is the number of units using the solution divided by the number of units that can fit in a cell.

The closest points are found by starting at the target point, and picking the lowest values on the distance map that aren't already taken.