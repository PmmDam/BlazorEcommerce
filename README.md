# Salty
## Manual de instrucciones
Para poder **trabajar en local** con este repositorio necesitas cumplir con los siguientes requisitos:
### Requisitos
+ Visual Studio 2022
+ Una instancia de Sql Server
  + y ejecutar este script.sql para crear la base de datos    
+ Un fichero de configuración llamado appsetings.json con los siguientes campos
  ```javascript
  {
    "ConnectionStrings": {
      "DefaultConnection": "Data Source=127.0.0.1;Initial Catalog=BlazorEcommerce;User ID=Login_Admin_User;Password=123456789/a;TrustServerCertificate=True"
    },
    "AppSettings": {
      "Token": "my top secret key",
      "StripeApiKey": "API Key de Stripe",
      "StripePaymentWebHook": "Webhook Key de Stripe"
    },
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
      }
    },
    "AllowedHosts": "*"
  }```
