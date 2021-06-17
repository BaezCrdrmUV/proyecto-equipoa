# TypeMe

La aplicación TypeMe, en todos sus clientes, tiene la finalidad de mantener y facilitar la comunicación entre dos o más personas que formen parte de la aplicación. Para esto, TypeMe permite el envío de mensajes a través de la creación de Grupos de al menos dos integrantes, el envío de imágenes, añadir y consultar los contactos, esto con el username que cada Typer haya definido. Además, la aplicación permite la actualización de los datos de la cuenta. 

La aplicación está pensada para ser usada desde un navegador web, como Chrome, Microsoft Edge y Mozilla Firefox en sus versiones más recientes. También podrá ser utilizada en computadoras laptops o de escritorios con sistema operativo de Windows, a partir de la versión 7 en adelante. De igual manera, podrá ser utilizada en dispositivos móviles con sistema operativo Android en sus versiones más recientes. 

Para que las aplicaciones clientes de TypeMe puedan funcionar, fue necesario el desarrollo de un back-end donde se diseñaron cuatro Microservicios y una API Gateway para centralizar la información de ellos. Es en este último donde los clientes tienen comunicación directa a través del protocolo HTTP.

## Inicializar los servicios

Se debe tener instalado [Docker](https://www.docker.com/products/docker-desktop) en su computadora para poder inicializar los servicios.

La estructura recomendada para tener el proyecto local es la siguiente:

`FOLDER es la carpeta donde estará todo el proyecto`

```bash
│───FOLDER
│   ├───apigateway
│   ├───db
├   ├───main
├   ├───MSContactos
├   ├───MSMensajes
├   ├───MSMultimedia
├   ├───MSTypers
├   ├───TypeMeDesktop
└───├───TypeMeWeb
```

Cada carpeta corresponde a una rama del presente repositorio, por lo que es recomendable que cuando se clone o descargue el repositorio, se cambie a cada rama correspondiente y se renombre la carpeta.

Para inicializar los servicios se debe estar dentro de la carpeta `main` donde se encuentra el archivo `docker-compose.yml` y se debe ejecutar el siguiente comando:

```bash
docker-compose up --build
```

## Ejecutar los clientes

### TypeMeDesktop

Para ejecutar el cliente de Desktop, se debe estrar dentro de la carpeta `TypeMeDesktop`, donde se encuentra la solución de C# (.sln). Lo recomendable es abrir dicho archivo con Visual Studio (Community 2019, recomendado) y ejecutar el proyecto con la tecla F5 o depurando el proyecto.
