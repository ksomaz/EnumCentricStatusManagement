# EnumCentricStatusManagement

EnumCentricStatusManagement is a lightweight enum metadata package for .NET.

It keeps status messages, severity values, and display metadata close to the enum values that own them. The package is useful for API responses, database status codes, workflow results, validation outcomes, and other enum-based state flows.

## Install

```sh
dotnet add package EnumCentricStatusManagement
```

## Status Metadata

```csharp
using EnumCentricStatusManagement.Core;

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

Use safe lookup for values coming from external systems:

```csharp
var status = (OperationStatus)externalStatusCode;

if (status.TryGetStatusMetadata(out var metadata))
{
    Console.WriteLine(metadata.Message);
}
```

## Info Metadata

```csharp
public enum AccountState
{
    [Info("Active", "The account can use the system.")]
    Active,

    [Info("Suspended", "The account requires manual review.")]
    Suspended
}

var info = AccountState.Active.GetInfoMetadata();

Console.WriteLine(info.Name);
Console.WriteLine(info.Description);
```

## Included APIs

- `GetEnumStatus`
- `TryGetEnumStatus`
- `GetStatusMetadata`
- `TryGetStatusMetadata`
- `GetEnumInfos`
- `TryGetEnumInfos`
- `GetEnumInfo`
- `GetEnumInfoOrDefault`
- `GetInfoMetadata`
- `TryGetInfoMetadata`
- `GetLocalizedMessage`

## Türkçe Özet

EnumCentricStatusManagement, enum değerlerine merkezi mesaj, durum tipi ve açıklayıcı metadata eklemek için kullanılır.

Bu paket özellikle veritabanından, API'den veya farklı sistemlerden gelen sayısal durum kodlarını daha okunabilir domain durumlarına çevirmek için faydalıdır.

```csharp
public enum OdemeDurumu
{
    [Status("Ödeme başarıyla tamamlandı.", StatusType.Success)]
    Tamamlandi = 0,

    [Status("Ödeme kontrol bekliyor.", StatusType.Warning)]
    KontrolBekliyor = 1,

    [Status("Ödeme başarısız oldu.", StatusType.Error)]
    Basarisiz = 2
}

var metadata = OdemeDurumu.Basarisiz.GetStatusMetadata();

Console.WriteLine(metadata.Message);
Console.WriteLine(metadata.IsError);
```

## Target Framework

The package targets `netstandard2.0`, generates XML documentation, uses cached reflection internally, and has no database dependency.

## License

MIT.
