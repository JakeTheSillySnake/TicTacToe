## Project: Tic-Tac-Toe
The project is created once and is used for all subsequent tasks.

## Task 0. Creating the Project
In Visual Studio, create a new project:

- In the startup window, select "Create New Project".
  
- In the "Create New Project" window, select "All Languages", then select C# from the drop-down menu.
- Select "Windows" from the "All Platforms" list.
- Next, select "Web" from the "All Project Types" list.
- Select "ASP.NET Core Web Application (Microsoft)".

## Task 1. Create the Project Structure
- Each layer is a separate package.
  
- The project structure should contain the following layers: web, domain, datasource, di.
- The web layer should contain at least the packages model, controller, mapper for interaction with the client.
- The domain layer should include at least the model, service packages to implement the business logic of the application.
- The datasource layer should include at least the packages model, repository, mapper for working with data (e.g. a database).
- The di layer describes dependency injection configurations.

## Task 2. Implementing the domain layer
- Describe the game board model as an integer matrix.
  
- Describe the current game model, which has a UUID and a game board.
- Describe a service interface with the following methods:
  - method to get the next move of the current game using the Minimax algorithm;
  
  - method to validate the current game's game board (check that previous moves have not been changed);
  
  - method to check if the game has ended.

Models, interfaces, and implementations should be located in separate files.

## Task 3. Implementing the datasource layer
- Implement a storage class for storing current games.
- Use thread-safe collections as the storage medium.
- Describe the board model and the current game model.
- Implement domain<->datasource mappers.
- Implement a repository with methods to work with the storage class:
  - method for saving the current game;
  
  - method for getting the current game.
- Create a class that implements the service interface, taking as a parameter a repository interface for working with the storage class.
  
Models, interfaces, and implementations should be in separate files.

## Task 4. Implementing the web layer
- Describe the game board model and the current game model.
  
- Implement the domain<->web mappers.
- Implement a controller using ASP.NET that has a method POST /game/{current game's UUID} that sends the current game with the board updated by the user and receives the current game with the board updated by the computer in response.
- If an incorrect current game with an updated board is sent, an error should be returned with a description.
- Multiple concurrent games should be supported.

Models, interfaces, and implementations should be in separate files.

## Task 5. Implementing the DI layer
- Implement a Configuration class that describes the dependency graph.
- It should contain:
  - the storage class as a singleton;
  - a repository for working with the storage class;
  - a service for working with the repository.