# Implementation Flow

- I have created a FileMode and MazeCell class under Model folder in the API folder. 
- Configured the startup for uploading data
- Created MazeResource folder to store Maze files (.txt). When uploaded, it will be stored in this file.

# Controller class
- Read maze file names and expose them.
- Load the maze when requested using maze file name.
- handle all navigation api calls.
- return the current mazecell when requested.

# upload.service.ts
- methods in this .ts interact with the apiend point and return observables as requested.

# mazeCell.ts
- contains an interface equilvalent of MazeCell model class in the api. Makes it eassier to interact
