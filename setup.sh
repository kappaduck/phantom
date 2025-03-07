#!/bin/bash

# Cleanup build
find ./artifacts -type f -name "*.nupkg" -exec rm -f {} \;

# Build and Pack Boo
setupPath="./src/Boo/Boo.csproj"

dotnet build "$setupPath" --configuration release
dotnet pack "$setupPath" --configuration release

# Find the .nupkg file
nupkg=""

for f in ./release/Boo.*.nupkg; do
    nupkg="$f"
    nupkgFolder=$(dirname "$nupkg")
    break
done

if [[ -z "$nupkg" ]]; then
    echo "Could not find the .nupkg file"
    exit 1
fi

# Extract the version number from the .nupkg file
filename=$(basename "$nupkg")

# Remove the prefix 'Boo.' and suffix '.nupkg'
version="${filename//Boo./}"
version="${version//.nupkg/}"

dotnet tool install --local --add-source "$nupkgFolder" Boo --version "$version"

exit $?
