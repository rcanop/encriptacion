# Proyecto de encriptación en .NET Core

Este proyecto lo he utilizado para aprender las a manejar la encriptación de cadenas y ficheros en .NET Core usando AES usando la especificación Rijndael, que trae .NET Core.

Se compone de los siguientes proyectos:

- **Encriptacion.Core** Conjunto de clases para manejar la encriptación. 
- **Encriptacion**  Aplicación consola para encriptar ficheros.

## Encriptacion.Core

Es la librería propiamente dicha de encriptación.

Por ahora contiene:
    
- Métodos para encriptar cadenas **UTF-8** devolviéndolas en formato **Base64**. 
  - `Encrypt` Para encriptar la cadena
  - `Decrypt` Para desencriptar la cadena codificada.

- Métodos pare encriptar ficheros en formato **UTF-8**. 
  - `EncryptFile` Cambiando su extensión a `.cod` cuando se ha codificado.
  - `DecryptFile` Desencripta el fichero encriptado anteriormente restableciendo su extensión y añadiendo otra `.decod`.

## Encriptacion

Utilidad consola que sirve para codificar ficheros. Su uso es el siguiente:

```
$ encriptacion <fichero.ext> [d]
    <fichero.ext>   fichero a procesar.
    d               opcional realiza la desencriptación de un fichero ya encriptado '*.cod'
```

## Encriptacion.api

Proyecto API en .NET Core 2.2 para encriptar y desencriptar cadenas que se le envían (ésto sólo sería medianamente seguro si se usa con **https**). 

las didecciones serían: 
- **Encriptar** https:/.../api/encription/encrypt
- **Desencriptar** https:/.../api/encription/decrypt
- **Swager** https:/.../swagger
  - Por ahora sólo funciona los mensajes en JSON, XML. aún no soportado.
### Ejemplo VFP de consumo de la API

Un pequeño programa para usar el API desde VFP 9.
  - Usando datos en XML y en JSON