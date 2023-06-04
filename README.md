# GigFilesChecker

GigFilesChecker is a program designed to be dockerized and used to set up and run a game server for Gigantic.

## Prerequisites
- Docker installed on the host machine.

## Getting Started
1. Download the latest release of GigFilesChecker.
2. Extract the downloaded `GigFilesChecker.zip` file.
3. Copy the contents of the extracted folder to the desired location on your host machine.
4. Build the Docker image:
docker build -t gig-server
5. Create Docker volume:
docker volume create gigvolume
6. Run the Docker container:
docker run -d
-e HTTP_PORT=<HTTP_PORT>
-e SERVER_URL=<SERVER_URL>
-e SERVER_PORT=<SERVER_PORT>
-e MAX_INSTANCES=<MAX_INSTANCES>
-e TITLE=<TITLE>
-e API_KEY=<API_KEY>
-v /path/to/game/on/host/machine:/GiganticHost
-v gigvolume:/volume/gigantic
--name gig-server gig-server
  
Replace the `<HTTP_PORT>`, `<SERVER_URL>`, `<SERVER_PORT>`, `<MAX_INSTANCES>`, `<TITLE>`, and `<API_KEY>` placeholders with the appropriate values.

This command will:
- Start the Docker container in detached mode (`-d` flag).
- Set the required environment variables.
- Mount the host folder with the game at `/path/to/game/on/host/machine` to the Docker container's `/GiganticHost`.
- Mount the Docker volume `gigvolume` to the Docker container's `/volume/gigantic`.
- Assign the name `gig-server` to the Docker container.

7. Access the game server:
The game server will be accessible at the specified `SERVER_URL` and `SERVER_PORT` in the environment variables.
## License
This project is licensed under the [MIT License](LICENSE).
  
Please note that you should update the instructions further based on the specific requirements and structure of your project, including any additional configuration or setup steps necessary for running the GigFilesChecker program.
