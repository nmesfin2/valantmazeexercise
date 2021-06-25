# General Implementation

- I have created a FileMode and MazeCell class under Model folder in the API folder. 
- Configured the startup for uploading data
- Created MazeResource folder to store Maze files (.txt). When uploaded, it will be stored in this file.
- modified angular app to display grid of maze and be able to navigate thru it.

# Controller class
- Read maze file names and expose them.
- Load the maze when requested using maze file name.
- handle all navigation api calls.
- return the current mazecell when requested.

# upload.service.ts
- methods in this .ts interact with the apiend point and return observables as requested.

# mazeCell.ts
- contains an interface equilvalent of MazeCell model class in the api. Makes it eassier to interact

# home 

![homevalant](https://user-images.githubusercontent.com/22404103/123484043-94af0c80-d5bc-11eb-8e0f-3a3cf953306d.PNG)

# loaded maze

![loaded](https://user-images.githubusercontent.com/22404103/123484173-ca53f580-d5bc-11eb-89f3-2ed66d43ddca.PNG)

# no path

![nopathtonorth](https://user-images.githubusercontent.com/22404103/123484400-20c13400-d5bd-11eb-91c0-e1fa72883f9d.PNG)

# reach end

![end](https://user-images.githubusercontent.com/22404103/123484565-6978ed00-d5bd-11eb-944f-63ef4e91ca2f.PNG)
