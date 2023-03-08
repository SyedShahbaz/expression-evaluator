# Set the base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

# Set the working directory
WORKDIR /app

# Copy the project files to the container
COPY src/ULSolutions.Core/ULSolutions.Core.csproj src/ULSolutions.Web/ULSolutions.Web.csproj ./
RUN dotnet restore ULSolutions.Web.csproj

# Copy the entire project to the container
COPY . .

# Build the application
RUN dotnet publish src/ULSolutions.Web/ULSolutions.Web.csproj -c Release -o out

# Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose port 80
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "ULSolutions.Web.dll"]



# dotnet build src/ULSolutions.Web/ULSolutions.Web.csproj -c Release -o out

# docker build -t my-app .
# docker run -p 8080:80 my-app
# docker run -e WATCHDOG_USERNAME=admin -e WATCHDOG_PASSWORD=admin -p 8080:80 my-app

# http://localhost:8080/swagger/index.html
# http://localhost:8080/watchdog


# Without docker
# https://localhost:5001/swagger/index.html