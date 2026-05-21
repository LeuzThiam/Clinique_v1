# ECommerce

Application e-commerce developpee en C# avec une architecture separee entre interface web, passerelle API et microservices metier.

## Vue d'ensemble

Le depot est organise en deux parties principales :

- `frontend/` : application web ASP.NET Core MVC `MaBoutique`
- `backend/` : passerelle Ocelot et microservices `utilisateurs`, `produits`, `panier`, `commandes`, `paiement`

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
- Ocelot
- Docker / Docker Compose
- SQL Server
- Stripe

## Architecture

Architecture active du projet :

- `Frontend ASP.NET Core MVC` pour l'interface utilisateur
- `API Gateway Ocelot` comme point d'entree unique
- `Service Utilisateur` pour l'inscription, la connexion et les profils
- `Service Produit` pour le catalogue
- `Service Panier` pour la gestion du panier
- `Service Commande` pour les commandes
- `Service Paiement` pour l'integration Stripe

Flux vise :

`Utilisateur -> Frontend MVC -> Gateway -> Microservices`

Le frontend `MaBoutique` consomme les API backend via la passerelle exposee sur `http://localhost:5000`.

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
dotnet run --project EC_Cart_Service/EC_Cart_Service.csproj
dotnet run --project EC_Order_Service/EC_Order_Service.csproj
dotnet run --project EC_Payment_Service/EC_Payment_Service.csproj
dotnet run --project EC_GateWay/EC_GateWay.csproj
```

## Lancement avec Docker

Fichiers utilises :

- `docker-compose.yml`
- `docker/dotnet-service.Dockerfile`
- `.dockerignore`

Preparation :

```powershell
Copy-Item .env.example .env
```

Renseigne ensuite dans `.env` :

- `SQL_SA_PASSWORD`
- `STRIPE_SECRET_KEY`
- `STRIPE_PUBLISHABLE_KEY`

Puis lance la stack :

```powershell
docker compose up --build
```

Si ta machine utilise encore l'ancienne commande :

```powershell
docker-compose up --build
```

## Adresses par defaut

- Frontend : `http://localhost:5212`
- Gateway : `http://localhost:5000`
- Service Utilisateur : `http://localhost:5001`
- Service Produit : `http://localhost:5002`
- Service Commande : `http://localhost:5003`
- Service Paiement : `http://localhost:5004`
- Service Panier : `http://localhost:5005`

La stack Docker utilise un conteneur SQL Server partage par :

- `frontend/MaBoutique`
- `backend/EC_MicroServices/EC_Product_Service`

## Configuration Stripe

Le service de paiement lit ses cles depuis :

- `backend/EC_MicroServices/EC_Payment_Service/appsettings.json`

Valeurs a remplacer en environnement reel :

- `CHANGE_ME_STRIPE_SECRET_KEY`
- `CHANGE_ME_STRIPE_PUBLISHABLE_KEY`

## Integration GitHub

Le depot est prepare pour GitHub avec :

- `.gitignore`
- `README.md`
- `.github/workflows/ci.yml`

Le workflow GitHub Actions :

- restaure le frontend `MaBoutique`
- restaure le backend `EC_MicroServices`
- build le frontend
- build le backend

Actions utilisees :

- `actions/checkout@v6`
- `actions/setup-dotnet@v5`
