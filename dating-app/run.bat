@echo off
echo Starting Dating App...

echo Creating network...
docker network create dating-network 2>nul

echo Starting services...
docker run -d --name dating-userservice --network dating-network -p 5001:5001 -e ASPNETCORE_URLS=http://+:5001 dating-userservice
docker run -d --name dating-photoservice --network dating-network -p 5002:5002 -e ASPNETCORE_URLS=http://+:5002 -v photo-uploads:/app/uploads dating-photoservice
docker run -d --name dating-matchservice --network dating-network -p 5003:5003 -e ASPNETCORE_URLS=http://+:5003 dating-matchservice
docker run -d --name dating-frontend --network dating-network -p 3000:3000 dating-frontend

echo All services started!
echo Frontend: http://localhost:3000
echo UserService: http://localhost:5001
echo PhotoService: http://localhost:5002
echo MatchService: http://localhost:5003