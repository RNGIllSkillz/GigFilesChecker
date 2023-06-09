FROM ubuntu:latest

# Set working directory
WORKDIR /app

# Copy the compiled application files to the container
COPY . .

RUN apt-get update && apt-get install -y dos2unix && dos2unix /app/serverStartUp.sh && chmod +x /app/serverStartUp.sh && apt-get remove -y dos2unix

ENTRYPOINT ["/bin/bash", "/app/serverStartUp.sh"]