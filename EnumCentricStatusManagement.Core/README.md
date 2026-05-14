# UmbrellaFrame.EnumCentricStatusManagement ![EnumCentricStatusManagement](https://raw.githubusercontent.com/ksomaz/EnumCentricStatusManagement/master/assets/umbrellaframe-enumcentricstatusmanagement-icon.png) [![NuGet](https://img.shields.io/nuget/v/UmbrellaFrame.EnumCentricStatusManagement.svg?style=flat-square)](https://www.nuget.org/packages/UmbrellaFrame.EnumCentricStatusManagement) [![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg?style=flat-square)](LICENSE) [![.NET Standard 2.0](https://img.shields.io/badge/.NET%20Standard-2.0-purple?style=flat-square)](https://learn.microsoft.com/dotnet/standard/net-standard)

**Language / Dil:** [English](#english) - [Turkce](#turkce)

---

## English

**Enum metadata layer for centralized status, message, and info management in .NET**

Zero database dependency - cached reflection - safe `Try...` APIs - typed metadata models.

### Installation

```bash
dotnet add package UmbrellaFrame.EnumCentricStatusManagement
```

### Quick Start

```csharp
using UmbrellaFrame.EnumCentricStatusManagement.Core;

public enum OperationStatus
{
    [Status("Operation completed.", StatusType.Success)]
    Completed = 0,

    [Status("Operation requires attention.", StatusType.Warning)]
    RequiresAttention = 1,

    [Status("Operation failed.", StatusType.Error)]
    Failed = 2
}

var metadata = OperationStatus.Failed.GetStatusMetadata();

Console.WriteLine(metadata.Message);
Console.WriteLine(metadata.IsError);
```

Safe boundary lookup:

```csharp
var status = (OperationStatus)externalStatusCode;

if (status.TryGetStatusMetadata(out var metadata))
{
    Console.WriteLine(metadata.Message);
}
```

### Included APIs

| API | Description |
|---|---|
| `[Status]` | Status message and severity metadata |
| `[Info]` | Ordered display metadata |
| `GetStatusMetadata()` | Typed status metadata |
| `TryGetStatusMetadata(out StatusMetadata)` | Safe status metadata lookup |
| `GetInfoMetadata()` | Typed info metadata |
| `TryGetInfoMetadata(out InfoMetadata)` | Safe info metadata lookup |
| `GetEnumInfoOrDefault(InfoType, string)` | Info value with fallback |

---

## Turkce

**.NET icin merkezi enum durum, mesaj ve bilgi metadata katmani**

Veritabani bagimliligi yok - cache'li reflection - guvenli `Try...` API'leri - tiplenmis metadata modelleri.

### Kurulum

```bash
dotnet add package UmbrellaFrame.EnumCentricStatusManagement
```

### Hizli Baslangic

```csharp
using UmbrellaFrame.EnumCentricStatusManagement.Core;

public enum OdemeDurumu
{
    [Status("Odeme basariyla tamamlandi.", StatusType.Success)]
    Tamamlandi = 0,

    [Status("Odeme kontrol bekliyor.", StatusType.Warning)]
    KontrolBekliyor = 1,

    [Status("Odeme basarisiz oldu.", StatusType.Error)]
    Basarisiz = 2
}

var metadata = OdemeDurumu.Basarisiz.GetStatusMetadata();

Console.WriteLine(metadata.Message);
Console.WriteLine(metadata.IsError);
```

### Lisans

MIT (c) UmbrellaFrame
