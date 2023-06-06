# Salty - Manual de instrucciones
Para poder **trabajar en local** con este repositorio necesitas cumplir con los siguientes requisitos:
## Requisitos

### Software
+ Visual Studio 2022
+ Una instancia de Sql Server
+ Se recomienda el uso de  SQL Server Management Studio - SSMS
### Ficheros de configuración
+ Script SQL para generar la [base de datos](https://github.com/PmmDam/BlazorEcommerce/blob/master/generate_salty_db.sql) desde cero con datos de prueba
  + Al usar Entity Framework, en el CLI del Packagme manager, podemos crear directamente el esquema de base de datos con el contenido del directorio Migrations en el proyecto de servidor con el siguiente comando. Es importante asegurarse de estar posicionado en el directorio del proyecto de servidor en el package manager.
   ```console 
   dotnet ef database update 
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
### CLIs Y Webhooks
El pago en la aplicación está implementado con Stripe. Para que el checkout funcione correctamente, tenemos que crear un webhook y escuchar los evenetos de Stripe siguiendo su [Manual de instalación](https://stripe.com/docs/stripe-cli#install). 
Una vez descargado y abierto el CLI tendremos que poner el siguiente comando:
``` console
stripe listen --forward-to https://localhost:5001/api/payment
```
### Autor
Yo soy Pablo Muñoz Martínez, Estudiante de DAM y este es mi TFG.
Este es el reposirtorio original aunque [aquí](https://github.com/PmmDam/Salty) puedes ver la versión migrada a MudBlazor
