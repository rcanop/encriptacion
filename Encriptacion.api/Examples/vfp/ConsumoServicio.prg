CLEAR
LOCAL i
LOCAL lcURL,lcValue, lcFrase
LOCAL loXMLHttp AS MSXML.XMLHTTP

m.lcURL = "https://localhost:44379/api/encription"


*----------------------------------------------------------------------
*	Programa / Procedimiento / Función: Kk.prg
*	Programador (Máquina): Rafac (DELL-RAFA)
*	Fecha :		14/05/2019 19:11:03
*	Comentario: Usando datos XML
*----------------------------------------------------------------------

*-- Frase a encriptar
m.lcFrase = "El pico de la cigüeña es largo."
m.lcFrase = STRCONV(m.lcFrase, 1) && Pasamos a DBCS
m.lcFrase = STRCONV(m.lcFrase, 9) && Pasamos a UTF-8
m.lcFrase = STRCONV(m.lcFrase, 13) && Pasamos a Base64

? "---------------------------------------------"
? " 	Información en XML"
? "---------------------------------------------"
?

m.loXMLHttp = CREATEOBJECT("Microsoft.xmlhttp")
TEXT TO m.lcValue TEXTMERGE
<?xml version="1.0" encoding="UTF-8"?>
<ValueEncrypt xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
	<Value><<m.lcFrase>></Value>
</ValueEncrypt>
ENDTEXT 
*-- Encriptar
m.loXMLHttp.OPEN("POST", m.lcURL + "/encrypt")
m.loXMLHttp.setRequestHeader("Content-Type", "application/xml")
m.loXMLHttp.setRequestHeader("Accept", "text/xml")
m.loXMLHttp.SEND(m.lcValue)

DO WHILE .T.
	IF m.loXMLHttp.readyState = 4 OR LASTKEY() = 27
		=INKEY()
		EXIT

	ENDIF

ENDDO

IF m.loXMLHttp.STATUS = 200
	*-- Recibimos el valor encriptado
	m.lcFrase = STREXTRACT(m.loXMLHttp.responseText, '<Value>', '</Value>')

	? "Encriptado", m.lcFrase

	*-- Montamos un JSON para desencriptar
TEXT TO m.lcValue TEXTMERGE
<?xml version="1.0" encoding="UTF-8"?>
<ValueDecrypt>
	<Value><<m.lcFrase>></Value>
</ValueDecrypt>
ENDTEXT 

	_cliptext = m.lcFrase
	m.loXMLHttp.OPEN("POST", m.lcURL + "/decrypt")
	m.loXMLHttp.setRequestHeader("Content-Type", "text/xml")
	m.loXMLHttp.setRequestHeader("Accept", "text/xml")
	m.loXMLHttp.SEND(m.lcValue)

	DO WHILE .T.
		IF m.loXMLHttp.readyState = 4 OR LASTKEY() = 27
			=INKEY()
			EXIT

		ENDIF

	ENDDO
	IF m.loXMLHttp.STATUS = 200
		*-- Recibimos el valor desencriptado
		m.lcFrase = STREXTRACT(m.loXMLHttp.responseText, '<Value>', '</Value>')
		? "Desencriptado Base64", m.lcFrase
		m.lcFrase = STRCONV(STRCONV(STRCONV(m.lcFrase,14),11), 1)
		? "Desencriptado       ", m.lcFrase

	ELSE
		? m.loXMLHttp.STATUS, m.loXMLHttp.statusText
		InformaError(m.loXMLHttp.responseText)

	ENDIF
ELSE
	? m.loXMLHttp.STATUS, m.loXMLHttp.statusText
	InformaError(m.loXMLHttp.responseText)


ENDIF
?
?
?

*----------------------------------------------------------------------
*	Programa / Procedimiento / Función: Kk.prg
*	Programador (Máquina): Rafac (DELL-RAFA)
*	Fecha :		14/05/2019 19:11:03
*	Comentario: Usando datos JSON
*----------------------------------------------------------------------
? "---------------------------------------------"
? " 	Información en JSON"
? "---------------------------------------------"
?


*-- Frase a encriptar
m.lcFrase = "El pico de la cigüeña es largo."
m.lcFrase = STRCONV(m.lcFrase, 1) && Pasamos a DBCS
m.lcFrase = STRCONV(m.lcFrase, 9) && Pasamos a UTF-8
m.lcFrase = STRCONV(m.lcFrase, 13) && Pasamos a Base64

m.lcValue = '{value:"' + m.lcFrase + '"}' && Preparamos JSON
? "Peticion JSON", m.lcValue

*-- Encriptar
m.loXMLHttp.OPEN("POST", m.lcURL + "/encrypt")
m.loXMLHttp.setRequestHeader("Content-Type", "text/json")
m.loXMLHttp.SEND(m.lcValue)

DO WHILE .T.
	IF m.loXMLHttp.readyState = 4 OR LASTKEY() = 27
		=INKEY()
		EXIT

	ENDIF

ENDDO

IF m.loXMLHttp.STATUS = 200
	*-- Recibimos el valor encriptado
	m.lcFrase = STREXTRACT(m.loXMLHttp.responseText, '"value":"', '"')

	? "Encriptado", m.lcFrase

	*-- Montamos un JSON para desencriptar
	m.lcValue = '{value:"' + m.lcFrase + '"}'
	_cliptext = m.lcFrase
	m.loXMLHttp.OPEN("POST", m.lcURL + "/decrypt")
	m.loXMLHttp.setRequestHeader("Content-Type", "text/json")
	m.loXMLHttp.SEND(m.lcValue)

	DO WHILE .T.
		IF m.loXMLHttp.readyState = 4 OR LASTKEY() = 27
			=INKEY()
			EXIT

		ENDIF

	ENDDO
	IF m.loXMLHttp.STATUS = 200
		*-- Recibimos el valor desencriptado
		m.lcFrase = STREXTRACT(m.loXMLHttp.responseText, '"value":"', '"')
		? "Desencriptado Base64", m.lcFrase
		m.lcFrase = STRCONV(STRCONV(STRCONV(m.lcFrase,14),11), 1)
		? "Desencriptado       ", m.lcFrase

	ELSE
		? m.loXMLHttp.STATUS, m.loXMLHttp.statusText
		InformaError(m.loXMLHttp.responseText)

	ENDIF
ELSE
	? m.loXMLHttp.STATUS, m.loXMLHttp.statusText
	InformaError(m.loXMLHttp.responseText)


ENDIF
m.loXMLHttp = .NULL.

FUNCTION InformaError(m.tcMsg)

	IF VARTYPE(m.tcMsg) = "C"
		m.tcMsg = STREXTRACT(m.tcMsg, "<body>", "</body>")
		IF !EMPTY(m.tcMsg)
			MESSAGEBOX(m.tcMsg,16, "Información Error")

		ENDIF
	ENDIF

ENDFUNC

FUNCTION InformaError(m.tcMsg)

	IF VARTYPE(m.tcMsg) = "C"
		m.tcMsg = STREXTRACT(m.tcMsg, "<body>", "</body>")
		IF !EMPTY(m.tcMsg)
			MESSAGEBOX(m.tcMsg,16, "Información Error")

		ENDIF
	ENDIF

ENDFUNC