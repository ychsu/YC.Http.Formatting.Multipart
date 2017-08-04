# YC.Http.Formatting.Multipart

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