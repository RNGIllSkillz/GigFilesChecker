#!/bin/bash

# Create directories.txt
echo "/GiganticHost" > directories.txt
echo "/volume/gigantic" >> directories.txt

# Create config.json
echo "{\"http_port\": $HTTP_PORT, \"server_url\": \"$SERVER_URL\", \"server_port\": $SERVER_PORT, \"max_instances\": $MAX_INSTANCES, \"title\": \"$TITLE\", \"gigantic_path\": \"$GIGANTIC_PATH\", \"api_key\": \"$API_KEY\"}" > /volume/gigantic/config.json

# Run GigFilesChecker
chmod +x GigFilesChecker
./GigFilesChecker

# Retrieve the latest release version
release_url="https://api.github.com/repos/BigBoot/GiganticEmu/releases/latest"
response=$(curl -s "$release_url")
curl_exit_code=$?

if [[ $curl_exit_code -eq 0 ]]; then
    assets=$(echo "$response" | jq -r '.assets')

    # Define target directories for each file
    file_directories=(
        "/volume/gigantic/Binaries/Win64"   # Directory for ArcSDK.dll
        "/volume/gigantic"   # Directory for GiganticEmu.Agent
    )

    # Search for the specific files
    file_names=("ArcSDK.dll" "GiganticEmu.Agent")
    download_urls=()
    i=0
    for file_name in "${file_names[@]}"; do
        url=$(echo "$assets" | jq -r --arg file_name "$file_name" '.[] | select(.name == $file_name) | .browser_download_url')
        if [[ ! -z "$url" ]]; then
            download_urls+=("$url")

            # Set target directory for the file
            directory="${file_directories[$i]}"
            i=$((i+1))

            # Download the file
            echo "Downloading file: $file_name"
            wget -q -O "${directory}/${file_name}" "$url"

            # Check if download was successful
            download_exit_code=$?
            if [[ $download_exit_code -eq 0 ]]; then
                echo "Downloaded $file_name successfully."
            else
                echo "Failed to download $file_name. Curl exit code: $download_exit_code"
                exit 1
            fi
        fi
    done
else
    echo "Failed to fetch release data. Curl exit code: $curl_exit_code"
fi

# Start GiganticEmu.Agent
chmod +x /volume/gigantic/GiganticEmu.Agent
xvfb-run /volume/gigantic/GiganticEmu.Agent