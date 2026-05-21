# ECommerce

Projet e-commerce C# structure en deux zones principales a la racine :

- `frontend/` : application web ASP.NET Core MVC `MaBoutique`
- `backend/` : API Gateway Ocelot et microservices `users`, `products`, `orders`, `payments`

## Structure

```text
ECommerce/
├── .github/
│   └── workflows/
├── backend/
│   └── EC_MicroServices/
└── frontend/
    └── MaBoutique/
```

## Solutions

- Frontend : `frontend/MaBoutique/MaBoutique.sln`
- Backend : `backend/EC_MicroServices/EC_MicroServices.sln`

## Technologies

- .NET 9
- ASP.NET Core MVC
- Razor Views
- Entity Framework Core
- Ocelot Gateway
- Stripe

## Lancement local

### Frontend

```powershell
cd frontend/MaBoutique
dotnet build MaBoutique.sln
dotnet run --project MaBoutique/MaBoutique.csproj
```

### Backend

```powershell
cd backend/EC_MicroServices
dotnet build EC_MicroServices.sln
dotnet run --project EC_User_Service/EC_User_Service.csproj
dotnet run --project EC_Product_Service/EC_Product_Service.csproj
dotnet run --project EC_Order_Service/EC_Order_Service.csproj
dotnet run --project EC_Payment_Service/EC_Payment_Service.csproj
dotnet run --project EC_GateWay/EC_GateWay.csproj
```

## Lancement Docker

Le projet contient maintenant :

- `docker-compose.yml`
- `docker/dotnet-service.Dockerfile`
- `.dockerignore`

Pour lancer toute la stack :

```powershell
Copy-Item .env.example .env
docker compose up --build
```

Puis remplace dans `.env` :

- `SQL_SA_PASSWORD`
- `STRIPE_SECRET_KEY`
- `STRIPE_PUBLISHABLE_KEY`

Services exposes :

- Frontend : `http://localhost:5212`
- Gateway : `http://localhost:5000`
- Users : `http://localhost:5001`
- Products : `http://localhost:5002`
- Orders : `http://localhost:5003`
- Payments : `http://localhost:5004`

Avant un vrai test Stripe, configure les valeurs dans `.env`.

La stack Docker utilise un conteneur SQL Server partage par :

- `frontend/MaBoutique`
- `backend/EC_MicroServices/EC_Product_Service`

## URLs par defaut

- Frontend : `http://localhost:5212`
- Gateway : `http://localhost:5000`
- Users : `http://localhost:5001`
- Products : `http://localhost:5002`
- Orders : `http://localhost:5003`
- Payments : `http://localhost:5004`

## Architecture

Architecture active du dépôt :

- `Frontend ASP.NET Core MVC` pour l'interface utilisateur
- `API Gateway Ocelot` comme point d'entrée unique
- `User Service` pour les comptes et l'authentification
- `Product Service` pour le catalogue produits
- `Order Service` pour les commandes
- `Payment Service` pour les paiements Stripe

Le frontend `MaBoutique` consomme les microservices backend via la passerelle Ocelot (`http://localhost:5000`).

Note : le dépôt a été nettoyé pour ne garder que les services réellement branchés à la solution, au gateway, à Docker et à la CI.

## Configuration Stripe

Le service de paiement lit ses cles dans :

- `backend/EC_MicroServices/EC_Payment_Service/appsettings.json`

Remplace les placeholders :

- `CHANGE_ME_STRIPE_SECRET_KEY`
- `CHANGE_ME_STRIPE_PUBLISHABLE_KEY`

## GitHub

Fichiers racine ajoutes pour preparer le depot :

- `.gitignore`
- `README.md`
- `.github/workflows/ci.yml`

Le workflow GitHub Actions restaure et build :

- le frontend `MaBoutique`
- le backend `EC_MicroServices`

Le workflow utilise maintenant :

- `actions/checkout@v6`
- `actions/setup-dotnet@v5`
