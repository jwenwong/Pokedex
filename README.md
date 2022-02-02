# Pokedex REST API
This basic application retrieves Pokemon data from pokeapi.co.

## Endpoints
### Basic pokemon info /pokemon/{name}
Returns
- Pokemon name
- English description
- Habitat
- Legendary status

### Translated pokemon info /pokemon/translated/{name}
Returns
- Pokemon name
- Description - if the requested pokemon lives in a cave or is legendary, it will return a Yoda translated description. If not, it will return a Shakespeare translated description.
- Habitat
- Legendary status

## Build and run the application

### Command prompt
You can build this application manually by running the following commands in the /src directory

```console
dotnet build
dotnet run
```
Which should output the following:

```console
Hosting environment: Development
Content root path: C:\Users\Wen\source\repos\Pokedex\src\app\Pokedex
Now listening on: https://localhost:5001
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```
The application can then be accessed at the ports it is listening on.

### Docker
You can build this application using docker by running the follow commands in the /src directory

```console
docker build -t pokedex .
docker run -d -p 8080:80 --name pokedex pokedex
```

After the application starts, it can be accessed by navigating to http://localhost:8080 in your browser.

## Additional features if it were a production API
- Subscription to funtranslations to bypass the 5 calls per hour limit.
- Cache pokeapi responses to reduce calls.
- Resiliency - Retries + Circuit Breaker.
- Rate limiting to reduce number of calls and prevent api from being overwhelmed.
- Exception handler middlewar.
- CI/CD pipeline and integration with GitHub
- Additional translation options or endpoints to retrieve other data.
- Depending on scale of the API, potentially break down the solution into multiple projects.

## Links
- [Pokeapi](https://pokeapi.co/)
- Funtranslations - [Yoda](https://funtranslations.com/api/yoda) and [Shakespare](https://funtranslations.com/api/shakespeare)