# Dating App

## Setup GitHub Actions for Docker Hub

1. Go to your GitHub repository settings
2. Add these secrets:
   - `DOCKERHUB_USERNAME`: Your Docker Hub username
   - `DOCKERHUB_TOKEN`: Your Docker Hub access token

## Local Development

```bash
# Build images locally
build.bat

# Run locally built images
run.bat

# Stop containers
stop.bat
```

## Deploy from Docker Hub

```bash
# Edit deploy.bat and set your Docker Hub username
# Then run:
deploy.bat
```

## Services

- Frontend: http://localhost:3000
- UserService: http://localhost:5001
- PhotoService: http://localhost:5002
- MatchService: http://localhost:5003