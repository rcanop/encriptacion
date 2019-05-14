# Proyecto de encriptaci�n en .NET Core

Este proyecto lo he utilizado para aprender las a manejar la encriptaci�n de cadenas y ficheros en .NET Core usando AES usando la especificaci�n Rijndael, que trae .NET Core.

Se compone de los siguientes proyectos:

- **Encriptacion.Core** Conjunto de clases para manejar la encriptaci�n. 
- **Encriptacion**  Aplicaci�n consola para encriptar ficheros.

## Encriptacion.Core

Es la librer�a propiamente dicha de encriptaci�n.

Por ahora contiene:
    
- M�todos para encriptar cadenas **UTF-8** devolvi�ndolas en formato **Base64**. 
  - `Encrypt` Para encriptar la cadena
  - `Decrypt` Para desencriptar la cadena codificada.

- M�todos pare encriptar ficheros en formato **UTF-8**. 
  - `EncryptFile` Cambiando su extensi�n a `.cod` cuando se ha codificado.
  - `DecryptFile` Desencripta el fichero encriptado anteriormente restableciendo su extensi�n y a�adiendo otra `.decod`.

## Encriptacion

Utilidad consola que sirve para codificar ficheros. Su uso es el siguiente:

```
$ encriptacion <fichero.ext> [d]
    <fichero.ext>   fichero a procesar.
    d               opcional realiza la desencriptaci�n de un fichero ya encriptado '*.cod'
```

## Encriptacion.api

Proyecto API en .NET Core 2.2 para encriptar y desencriptar cadenas que se le env�an (�sto s�lo ser�a medianamente seguro si se usa con **https**). 

las didecciones ser�an: 
- **Encriptar** https:/.../api/encription/encrypt
- **Desencriptar** https:/.../api/encription/decrypt
- **Swager** https:/.../swagger
  - Por ahora s�lo funciona los mensajes en JSON, XML. a�n no soportado.
### Ejemplo VFP de consumo de la API

Un peque�o programa para usar el API desde VFP 9.
  - Usando datos en XML y en JSON