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
 