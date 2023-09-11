# Skyscrapers & Backtracking
Skyscrapers also known as "Towers" is a logic puzzle with simple rules and challenging solutions.


## Overview

Skyscrapers is a Windows Forms-based puzzle game that challenges players to strategically place skyscrapers on a grid while adhering to specific rules and edge constraints. This README provides an overview of the project, its features, and how to get started with the game.
A solve button calculates the solution for each board using Backtracking algorithm.

## Features

- Random puzzle generation: The game generates random puzzle boards with varying skyscraper heights and edge constraints.
- Backtracking solver: A backtracking algorithm is implemented to solve the generated puzzles.
- Hint system: Players can receive hints to help them progress in the game.
- Rule enforcement: The game ensures that players follow the rules of skyscraper placement and edge constraints.
- Timer: A game clock keeps track of the time taken to complete a puzzle.

## Getting Started

1. **Prerequisites:** Ensure you have the .NET Framework installed on your Windows machine.

2. **Clone the Repository:** Clone this repository to your local machine.

    ```bash
    git clone https://github.com/your-username/skyscrapers-game.git
    ```

3. **Build the Project:** Open the solution file (`Skyscrapers.sln`) in Visual Studio and build the project.

4. **Run the Game:** Start the game by running the application. The main entry point is `Program.cs`.

5. **Game Rules:** Familiarize yourself with the game rules by clicking the "Rules" button within the game.

6. **Start a New Game:** Click the "New Game" button to begin a new puzzle.

7. **Place Skyscrapers:** Click on the grid cells to place skyscrapers. Follow the rules to ensure that skyscraper heights are unique in each row and column.

8. **Use Hints:** If you need assistance, you can use the "Hint" button to receive a hint. You have a limited number of hints available.

9. **Solve the Puzzle:** If you're stuck, click the "Solve" button to automatically solve the puzzle. Note that the computer's solution may not always be available for challenging puzzles.

10. **Complete the Game:** When you successfully place skyscrapers following the rules and satisfy the edge constraints, you'll receive a message indicating that you've completed the puzzle.

## Game Mechanics

- **Skyscraper Heights:** Heights of the skyscrapers range from 1 to the size of the grid (e.g., 1 to 4 for a 4x4 puzzle).
- **Unique Heights:** Each row and column must have unique skyscraper heights.
- **Edge Constraints:** The numbers on the sides of the grid indicate how many skyscrapers are visible from that perspective.

## Acknowledgments

This Skyscrapers game project is created by evya5.
