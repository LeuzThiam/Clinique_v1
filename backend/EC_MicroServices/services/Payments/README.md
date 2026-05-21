# Payments

Contexte metier Payments organise en Clean Architecture.

Projets:
- src/MaBoutique.Payments.Domain
- src/MaBoutique.Payments.Application
- src/MaBoutique.Payments.Infrastructure
- src/MaBoutique.Payments.Api
- tests/MaBoutique.Payments.Tests.Unit
- tests/MaBoutique.Payments.Tests.Integration

Dossiers recommandes:
- Domain: Entities, ValueObjects, Enums, Exceptions
- Application: Abstractions, UseCases, Dtos, Validation
- Infrastructure: Persistence, Repositories, External, DependencyInjection.cs
- Api: Controllers, Contracts, Middlewares, Program.cs
