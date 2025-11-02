@echo off
echo Pulling and running Dating App from Docker Hub...

set DOCKERHUB_USERNAME=your-dockerhub-username

echo Creating network...
docker network create dating-network 2>nul

echo Pulling and starting services...
docker run -d --name dating-userservice --network dating-network -p 5001:5001 -e ASPNETCORE_URLS=http://+:5001 %DOCKERHUB_USERNAME%/dating-userservice:latest
docker run -d --name dating-photoservice --network dating-network -p 5002:5002 -e ASPNETCORE_URLS=http://+:5002 -v photo-uploads:/app/uploads %DOCKERHUB_USERNAME%/dating-photoservice:latest
docker run -d --name dating-matchservice --network dating-network -p 5003:5003 -e ASPNETCORE_URLS=http://+:5003 %DOCKERHUB_USERNAME%/dating-matchservice:latest
docker run -d --name dating-frontend --network dating-network -p 3000:3000 %DOCKERHUB_USERNAME%/dating-frontend:latest

echo All services started from Docker Hub!
echo Frontend: http://localhost:3000