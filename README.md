# ECommerce

Projet e-commerce C# structure en trois zones a la racine :

- `frontend/` : application web ASP.NET Core MVC `MaBoutique`
- `backend/` : microservices `users`, `products`, `orders`, `payments` et `gateway`
- `logs/` : fichiers temporaires de lancement et diagnostic

## Structure

```text
ECommerce/
├── .github/
│   └── workflows/
├── backend/
│   └── EC_MicroServices/
├── frontend/
│   └── MaBoutique/
└── logs/
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

Le frontend `MaBoutique` consomme les microservices backend via la passerelle Ocelot (`http://localhost:5000`).
Les appels produit, utilisateur, commande et paiement passent par le gateway, ce qui sépare la couche de présentation des services métiers.

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
