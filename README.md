# Enum-Centric Status Management

[![CI](https://github.com/ksomaz/EnumCentricStatusManagement/actions/workflows/ci.yml/badge.svg)](https://github.com/ksomaz/EnumCentricStatusManagement/actions/workflows/ci.yml)

EnumCentricStatusManagement is a lightweight .NET library that turns enum values into a centralized status metadata layer.

It helps teams keep status messages, severity information, and descriptive metadata close to the enum members that own them, instead of spreading repetitive `switch`, `if`, and magic-number mapping logic across services, controllers, database adapters, and UI code.

## Why It Exists

Many business applications already communicate state through enum values or numeric status codes:

- stored procedure output parameters,
- external API response codes,
- queue event states,
- workflow results,
- validation and domain operation outcomes.

Those codes are stable, but the meaning around them often becomes scattered. EnumCentricStatusManagement keeps that meaning in one place.

## Key Capabilities

- Attribute-based status metadata for enum members.
- Centralized message and severity handling.
- Safe `Try...` APIs for unknown or missing metadata.
- Typed convenience models: `StatusMetadata` and `InfoMetadata`.
- Cached reflection after the first lookup.
- Optional info metadata for display names and descriptions.
- Localization helper through `ResourceManager`.
- `netstandard2.0` target for broad compatibility.
- No database dependency in the core package.
- XML documentation support for IDE and NuGet consumers.

## Installation

```sh
dotnet add package EnumCentricStatusManagement
```

## Quick Example

```csharp
using EnumCentricStatusManagement.Core;

public enum UserRegistrationStatus
{
    [Status("User created successfully.", StatusType.Success)]
    Created = 0,

    [Status("Email address is already in use.", StatusType.Warning)]
    DuplicateEmail = 1,

    [Status("User information could not be verified.", StatusType.Error)]
    VerificationFailed = 2
}
```

Read required metadata when the enum value is trusted:

```csharp
var metadata = UserRegistrationStatus.VerificationFailed.GetStatusMetadata();

Console.WriteLine(metadata.Message); // User information could not be verified.
Console.WriteLine(metadata.IsError); // True
```

Use safe lookup at system boundaries:

```csharp
var status = (UserRegistrationStatus)statusCodeFromDatabase;

if (!status.TryGetStatusMetadata(out var metadata))
{
    return Results.Problem("Unknown status code returned by the database.");
}

return metadata.IsError
    ? Results.BadRequest(metadata.Message)
    : Results.Ok(metadata.Message);
```

## Info Metadata

Use `InfoAttribute` when enum values also need display metadata:

```csharp
public enum AccountState
{
    [Info("Active", "The account can sign in and use the system.")]
    Active,

    [Info("Suspended", "The account is blocked until manual review.")]
    Suspended
}

var info = AccountState.Active.GetInfoMetadata();

Console.WriteLine(info.Name);
Console.WriteLine(info.Description);
```

Fallback reads are available for partial metadata:

```csharp
var description = AccountState.Active.GetEnumInfoOrDefault(
    InfoType.Description,
    "No description available.");
```

## When To Use It

Use this package when:

- your application has repeated enum-to-message mapping logic,
- numeric status codes need to become readable domain states,
- API or database results need consistent success/warning/error handling,
- status metadata should be close to the enum definition,
- you want a small package instead of a full result-handling framework.

Avoid it when:

- status rules require complex runtime behavior,
- messages must be fully dynamic and data-driven,
- your project already has a mature result/error abstraction.

## API Overview

| API | Purpose |
| --- | --- |
| `GetEnumStatus()` | Returns the required `StatusAttribute`; throws a clear exception when missing. |
| `TryGetEnumStatus(out StatusAttribute)` | Safe status attribute lookup. |
| `GetStatusMetadata()` | Returns a typed `StatusMetadata` convenience model. |
| `TryGetStatusMetadata(out StatusMetadata)` | Safe typed status metadata lookup. |
| `GetEnumInfos()` | Returns all required `InfoAttribute` entries. |
| `TryGetEnumInfos(out string[])` | Safe info array lookup. |
| `GetEnumInfo(InfoType)` | Returns one required info entry by index. |
| `GetEnumInfoOrDefault(InfoType, string)` | Returns one info entry or a fallback value. |
| `GetInfoMetadata()` | Returns a typed `InfoMetadata` convenience model. |
| `TryGetInfoMetadata(out InfoMetadata)` | Safe typed info metadata lookup. |
| `GetLocalizedMessage(ResourceManager, string)` | Reads a resource value keyed by enum member name. |

## Runnable Sample

```sh
dotnet run --project samples/StatusMappingExample/StatusMappingExample.csproj
```

Expected output:

```text
Error: User information could not be verified.
```

## Development

```sh
dotnet restore
dotnet build
dotnet test
```

Create a release package:

```sh
dotnet build EnumCentricStatusManagement.Core/EnumCentricStatusManagement.Core.csproj -c Release
dotnet pack EnumCentricStatusManagement.Core/EnumCentricStatusManagement.Core.csproj -c Release --no-build
```

## Türkçe

EnumCentricStatusManagement, enum değerlerine merkezi durum mesajı, durum tipi ve açıklayıcı metadata eklemek için geliştirilmiş hafif bir .NET kütüphanesidir.

Özellikle veritabanı, harici API, kuyruk sistemi veya domain operasyonlarından gelen sayısal durum kodlarını daha okunabilir ve yönetilebilir hale getirmek için kullanılır.

### Ne Sağlar?

- Enum değerinin mesajını enum tanımının yanında tutar.
- `Success`, `Warning`, `Error` gibi durum tiplerini standartlaştırır.
- Dağınık `switch` / `if` bloklarını azaltır.
- Bilinmeyen enum değerlerinde güvenli `Try...` metotları sunar.
- Reflection maliyetini ilk okumadan sonra cache ile azaltır.
- NuGet paketi olarak bağımsızdır; veritabanı bağımlılığı içermez.

### Türkçe Kullanım Örneği

```csharp
using EnumCentricStatusManagement.Core;

public enum SiparisDurumu
{
    [Status("Sipariş başarıyla oluşturuldu.", StatusType.Success)]
    Olusturuldu = 0,

    [Status("Sipariş onay bekliyor.", StatusType.Warning)]
    OnayBekliyor = 1,

    [Status("Sipariş oluşturulamadı.", StatusType.Error)]
    Olusturulamadi = 2
}

var durum = SiparisDurumu.Olusturulamadi;
var metadata = durum.GetStatusMetadata();

Console.WriteLine(metadata.Message);
Console.WriteLine(metadata.IsError);
```

Harici sistemlerden gelen kodlarda güvenli kullanım:

```csharp
var durum = (SiparisDurumu)veritabanindanGelenDurumKodu;

if (!durum.TryGetStatusMetadata(out var metadata))
{
    return Results.Problem("Bilinmeyen durum kodu döndü.");
}

return metadata.IsError
    ? Results.BadRequest(metadata.Message)
    : Results.Ok(metadata.Message);
```

### Ne Zaman Tercih Edilmeli?

Bu paket; küçük, net ve merkezi bir enum metadata çözümü gerektiğinde uygundur. Büyük bir result framework yerine, mevcut enum tabanlı akışınızı daha okunabilir ve bakımı kolay hale getirmek için tasarlanmıştır.

## Release Notes

See [CHANGELOG.md](CHANGELOG.md).

## License

MIT. See [LICENSE.txt](LICENSE.txt).
