# Auth

Contexte metier Auth organise en Clean Architecture.

Projets:
- src/MaBoutique.Auth.Domain
- src/MaBoutique.Auth.Application
- src/MaBoutique.Auth.Infrastructure
- src/MaBoutique.Auth.Api
- tests/MaBoutique.Auth.Tests.Unit
- tests/MaBoutique.Auth.Tests.Integration

Dossiers recommandes:
- Domain: Entities, ValueObjects, Enums, Exceptions
- Application: Abstractions, UseCases, Dtos, Validation
- Infrastructure: Persistence, Repositories, External, DependencyInjection.cs
- Api: Controllers, Contracts, Middlewares, Program.cs
