@echo off
echo Starting Dating App Services...

echo Starting UserService on port 5001...
start "UserService" cmd /k "cd backend\UserService && dotnet run"

timeout /t 3

echo Starting PhotoService on port 5002...
start "PhotoService" cmd /k "cd backend\PhotoService && dotnet run"

timeout /t 3

echo Starting MatchService on port 5003...
start "MatchService" cmd /k "cd backend\MatchService && dotnet run"

timeout /t 3

echo Starting React Frontend...
start "Frontend" cmd /k "cd frontend\dating-frontend && npm start"

echo All services started!
echo UserService: http://localhost:5001
echo PhotoService: http://localhost:5002
echo MatchService: http://localhost:5003
echo Frontend: http://localhost:3000

pause