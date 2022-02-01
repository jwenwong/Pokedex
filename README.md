# Pokedex REST API
This basic application retrieves Pokemon data from pokeapi.co.

## Build and run the application
You can build this application using docker by running the follow commands in the /src directory

```console
docker build -t pokedex .
docker run -d -p 8080:80 --name pokedex pokedex
```

After the application starts, it can be accessed by navigating to http://localhost:8000 in your browser.