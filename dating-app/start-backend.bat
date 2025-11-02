@echo off
echo Starting Backend Services...

echo Starting UserService...
cd backend\UserService
start "UserService" cmd /k "dotnet run --urls=http://localhost:5001"
cd ..\..

timeout /t 5

echo Starting PhotoService...
cd backend\PhotoService
start "PhotoService" cmd /k "dotnet run --urls=http://localhost:5002"
cd ..\..

timeout /t 5

echo Starting MatchService...
cd backend\MatchService
start "MatchService" cmd /k "dotnet run --urls=http://localhost:5003"
cd ..\..

echo Backend services started!
echo UserService: http://localhost:5001
echo PhotoService: http://localhost:5002
echo MatchService: http://localhost:5003

pause