#!/bin/bash

# Check if the user provided a name
if [ -z "$1" ]; then
  echo "Usage: $0 <project-name> [<path>]"
  exit 1
fi

# Project name provided by the user
PROJECT_NAME=$1

# Get the directory where the script is located and move up one level (to project/)
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PARENT_DIR="$(dirname "$SCRIPT_DIR")"

# Optional path provided by the user, defaults to parent directory (project/) if not provided
TARGET_PATH=${2:-$PARENT_DIR}

# Define the source directory
SRC_DIR="$TARGET_PATH/src"

# Create target and src directories if they don't exist
mkdir -p "$SRC_DIR" || {
  echo "Failed to create directory: $SRC_DIR"
  exit 1
}

# Create the solution file in the root (TARGET_PATH)
cd "$TARGET_PATH" || {
  echo "Failed to change to directory: $TARGET_PATH"
  exit 1
}
if [ ! -f "${PROJECT_NAME}.sln" ]; then
  dotnet new sln --name "$PROJECT_NAME" || {
    echo "Failed to create solution: ${PROJECT_NAME}.sln"
    exit 1
  }
fi

# Move into SRC_DIR for project creation
cd "$SRC_DIR" || {
  echo "Failed to change to directory: $SRC_DIR"
  exit 1
}

# Define project types
PROJECT_TYPES=("Application" "Persistence" "Domain" "Infrastructure" "Presentation" "Contract")

# Create the Web API project
if [ ! -f "${PROJECT_NAME}.API" ]; then
  dotnet new webapi -n "${PROJECT_NAME}.API" -o "${PROJECT_NAME}.API" --framework net8.0 || {
    echo "Failed to create Web API project: ${PROJECT_NAME}.API"
    exit 1
  }
fi

# Create each project
for TYPE in "${PROJECT_TYPES[@]}"; do
  # Create the .NET project
  dotnet new classlib -n "${PROJECT_NAME}.${TYPE}" -o "${SRC_DIR}/${PROJECT_NAME}.${TYPE}" --framework net8.0

  # Check if the project was created successfully
  if [ $? -eq 0 ]; then
    echo "Created project: ${PROJECT_NAME}.${TYPE} in $SRC_DIR"
  else
    echo "Failed to create project: ${PROJECT_NAME}.${TYPE}"
    exit 1
  fi
done

# Enable recursive globbing
shopt -s globstar

# Add all .csproj files recursively to the solution (from TARGET_PATH)
cd "$TARGET_PATH" || {
  echo "Failed to change back to directory: $TARGET_PATH"
  exit 1
}
for CSPROJ in src/**/*.csproj; do
  if [ -f "$CSPROJ" ]; then
    dotnet sln "${PROJECT_NAME}.sln" add "$CSPROJ"
    
    if [ $? -eq 0 ]; then
      echo "Added $CSPROJ to ${PROJECT_NAME}.sln"
    else
      echo "Failed to add $CSPROJ to ${PROJECT_NAME}.sln"
      exit 1
    fi
  fi
done

# Create .gitignore
if [ ! -f ".gitignore" ]; then
  dotnet new gitignore || {
    echo "Failed to create .gitignore"
    exit 1
  }
fi

# Create .editorconfig
if [ ! -f ".editorconfig" ]; then
  dotnet new editorconfig || {
    echo "Failed to create .editorconfig"
    exit 1
  }
fi

echo "All projects created in $SRC_DIR and added to the solution in $TARGET_PATH successfully."
