# Tiny Shell - A POSIX Compliant Shell in C#

Welcome to **Tiny Shell**, a lightweight POSIX-compliant shell implemented in C#. This project is part of the Codecrafters Challenge and demonstrates how to build a functional shell capable of executing commands, running external programs, and handling built-in commands.

## Features

- **Command Execution**: Run external programs with arguments seamlessly.
- **Built-in Commands**: Includes basic shell commands such as:
  - `cd`: Change the current working directory.
  - `pwd`: Display the current working directory.
  - `echo`: Print text to the console.
  - Additional built-ins as specified in POSIX standards.
- **POSIX Compliance**: Adheres to POSIX standards for shell behavior.

## Getting Started

### Prerequisites

Ensure you have the following installed:

- **.NET SDK** (version 6.0 or later): [Download here](https://dotnet.microsoft.com/download)
- A POSIX-compliant operating system or environment.

### Installation

1. Clone this repository:
   ```bash
   git clone https://github.com/yourusername/tiny-shell.git
   cd tiny-shell
   ```
2. Build the project:
   ```bash
   dotnet build
   ```

### Running the Shell

To start the shell, run:
```bash
dotnet run
```
