FROM ubuntu:latest

ENV GIGANTIC_PATH /volume/gigantic

# Install dependencies
RUN apt-get update && apt-get install -y gettext-base
RUN dpkg --add-architecture i386
#RUN apt-get update && apt-get install -y xvfb unzip wget curl ca-certificates jq wine wine32 winbind && apt-get clean
RUN apt update 	&& DEBIAN_FRONTEND=noninteractive apt install -y xvfb unzip wget curl ca-certificates jq
RUN apt update 	&& DEBIAN_FRONTEND=noninteractive apt install -y wine
RUN apt update 	&& DEBIAN_FRONTEND=noninteractive apt install -y wine32 && apt clean && rm -rf /var/lib/apt/lists/*
RUN apt update && apt install --reinstall -y winbind

# Set working directory
WORKDIR /app

# Copy the compiled application files to the container
COPY . .

RUN apt-get update && apt-get install -y dos2unix && dos2unix /app/serverStartUp.sh && chmod +x /app/serverStartUp.sh && apt-get remove -y dos2unix

ENTRYPOINT ["/bin/bash", "/app/serverStartUp.sh"]