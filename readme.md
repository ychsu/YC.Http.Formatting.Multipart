# YC.Http.Formatting.Multipart
[![Build status](https://ci.appveyor.com/api/projects/status/1298r6h1xs5yal1v?svg=true)](https://ci.appveyor.com/project/ychsu/yc-http-formatting-multipart)

## Description
讓WebApi可以接受 multipart/form-data 請求

## Usage
``` csharp
// ./App_Start/WebApiConfig.cs
public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        ...
        config.Formatters.Add(MultipartMediaTypeFormatter.Create());
        ...
    }
}
```
