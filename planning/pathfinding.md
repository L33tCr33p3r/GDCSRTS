Once a flowfield solution has been found, each unit is assigned a point from the set of *n* closest points to the target (according to the distance map), where *n* is the number of units using the solution divided by the number of units that can fit in a cell.

The closest points are found by starting at the target point, and picking the lowest values on the distance map that aren't already taken.