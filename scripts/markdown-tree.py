#!/usr/bin/env python3

import os
import subprocess
import argparse

def escape_path(path):
    """Escape special characters in the path for use in URLs."""
    return path.replace(" ", "%20")

DEFAULT_IGNORE = "\"*~|node_modules|bin|obj|*.md|*.csproj|package-lock.json|tsconfig.json|tsconfig.node.json|vite.config.ts|launch_settings.json|*.svg|*.png|*.ico|*.jpeg|*.jpg\""

def generate_markdown_tree(directory='.', ignore=DEFAULT_IGNORE):
    # ignoreArg = f"-I {ignore}"
    # print(ignoreArg)
    try:
        # Run the tree command and get the result as a list of paths
        result = subprocess.check_output(["tree", "-if", "--noreport", "--gitignore", "-I", ignore, directory], universal_newlines=True)
        paths = result.splitlines()
        
        for path in paths:
            # Remove the leading './' from the paths
            path = path[2:]

            if path == ".":
                continue

            # Determine the level of indentation based on slashes
            slashes = path.count("/")
            spaces = slashes * 2

            # Extract the current file/directory name for display text
            displaytext = os.path.basename(path)

            # Create the markdown link
            linkpath = escape_path(path)
            print(f"{' ' * spaces}- [{displaytext}]({linkpath})")
            
    except subprocess.CalledProcessError:
        print("Error: 'tree' command not found. Please install it first.")
        return

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Convert the output of the Linux 'tree' command to markdown.")
    parser.add_argument("directory", nargs="?", default=".", help="Directory to generate markdown tree for (default: current directory).")
    
    args = parser.parse_args()
    generate_markdown_tree(args.directory)
