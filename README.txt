-----------Train Routes Finder---------------

Overview

The Train Routes Finder is a .NET console application that calculates distances, shortest routes, and trip counts between towns based on an input file. The program uses graph-based algorithms to process and analyze the train routes efficiently.

Features :

Distance Calculation: Calculate the total distance of a specific route.
Shortest Route: Find the shortest route between two towns.
Trip Count:
	Trips with a maximum number of stops.
	Trips with an exact number of stops.
	Trips within a specified distance.
Error Handling: Gracefully handles invalid routes or inputs.

Input File Format :
The input file should contain one route per line in the following format:
<From Town A>,<To Town B>,<Distance>

Example

A,B,5
B,C,4
C,D,8
D,C,8
D,E,6
A,D,5
C,E,2
E,B,3
A,E,7

Design Decisions

Graph Representation : The application uses an adjacency list to represent the graph, where each town points to a list of its connected routes.
This structure optimizes memory usage and facilitates traversal operations.

Separation of Concerns : The Route class encapsulates information about individual routes.
The Graph class manages all graph-related operations, such as route calculation and trip counting.

Recursive Algorithms : Recursive functions are used for counting trips and finding the shortest routes, ensuring clarity and modularity.

Error Handling : The application returns -1 or an appropriate message when a route doesn't exist.
Invalid input formats or missing files are handled gracefully.

Usage

Run the application.
Provide the path to the input file when prompted.
The program will execute predefined tests and display the results.

Example Output

Enter the path to the input file: input.txt
Test #1: 9
Test #2: 5
Test #3: 13
Test #4: 22
Test #5: Route doesn't exist
Test #6: 2
Test #7: 3
Test #8: 9
Test #9: 9
Test #10: 7

Unit Tests

The application includes unit tests to verify the functionality of all features. Tests cover:
Distance calculation for specific routes.
Counting trips with constraints.
Shortest route calculations.
Edge cases, such as non-existent routes.

How to Run

Open the solution in Visual Studio.
Build the solution to restore dependencies.
Run the TrainRoutesFinder project.

