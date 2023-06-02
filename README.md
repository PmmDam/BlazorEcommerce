# Salty
## Manual de instrucciones
Para poder **trabajar en local** con este repositorio necesitas cumplir con los siguientes requisitos:
### Requisitos
+ Visual Studio 2022
+ Una instancia de Sql Server
  + y ejecutar este script.sql para crear la base de datos    
+ Un fichero de configuración, en la carpeta raiz del proyecto .Server, llamado appsetings.json con el siguiente contenido modificado. Para ello necesitas una cuenta de developer de Stripe. En caso contrario, no funcionará correctamente el pago del pedido.
  ```json
  {
    "ConnectionStrings": {
      "DefaultConnection": "Data Source=127.0.0.1;Initial Catalog=Nombre_DB;User ID=DB_User;Password=TuContraseña;TrustServerCertificate=True"
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
  }
