# RazboiCarti War Card Game -LAN Multiplayer in C#

RazboiCarti is a C# WinForms application that implements the classic card game War, extended with LAN multiplayer support. The project demonstrates object-oriented programming principles, real-time client-server communication, and UI interaction.
The game allows two players to compete over a local network:
Player 1 (Host) – manages game logic and acts as the server
Player 2 (Client) – connects to the host and mirrors the game state

# Features
- *Automatic deck generation from image assets*
- *Fisher–Yates shuffle algorithm*
- *Two-player LAN gameplay (TCP-based)*
- *Card comparison system (2–10, J, Q, K, A)*
- *Round-based gameplay with synchronization*
- *Real-time score tracking*
- *Restart system (requires both players’ agreement)*
- *Visual representation of cards using images*

# Architecture
The project follows a simple and modular architecture, making it easy to understand, maintain, and extend.

### Structure

- **Main Module**
  - Entry point of the application
  - Handles initialization and execution flow

- **Core Logic**
  - Contains the main functionality of the application
  - Processes data and implements business rules

- **Utilities / Helpers**
  - Reusable helper functions
  - Handles common tasks (e.g., formatting, validation)

- **Configuration**
  - Stores constants and configuration variables
  - Allows easy customization without modifying core code

### Design Principles

- **Separation of Concerns** – Each component has a clear responsibility  
- **Modularity** – Code is split into smaller, reusable parts  
- **Scalability** – Easy to extend with new features  
- **Readability** – Clean and simple structure for better understanding  

### Flow

1. The application starts from the main module  
2. Input is received and validated  
3. Core logic processes the data  
4. Results are returned or displayed  

This structure ensures the project remains clean, maintainable, and easy to scale.

# Technologies Used
- *C# (.NET Framework / WinForms)*
- *TCP Networking (System.Net.Sockets)*
- *Multithreading (System.Threading)*
- *File I/O (System.IO)*
