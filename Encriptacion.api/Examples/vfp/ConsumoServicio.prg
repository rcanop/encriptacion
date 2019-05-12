LOCAL i
LOCAL lcURL,lcValue, lcFrase
LOCAL loXMLHttp AS MSXML.XMLHTTP
m.lcURL = "https://localhost:44379/api/encription"

CLEAR

*-- Frase a encriptar
m.lcFrase = "El pico de la cigüeña es largo."
m.lcFrase = STRCONV(m.lcFrase, 1) && Pasamos a DBCS
m.lcFrase = STRCONV(m.lcFrase, 9) && Pasamos a UTF-8
m.lcFrase = STRCONV(m.lcFrase, 13) && Pasamos a Base64

m.lcValue = '{value:"' + m.lcFrase + '"}' && Preparamos JSON



m.loXMLHttp = CREATEOBJECT("Microsoft.xmlhttp")

*-- Encriptar
m.loXMLHttp.OPEN("POST", m.lcURL + "/encript")
m.loXMLHttp.setRequestHeader("Content-Type", "application/json")
m.loXMLHttp.SEND(m.lcValue)

DO WHILE .T.
	IF m.loXMLHttp.readyState = 4 OR LASTKEY() = 27
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
	m.loXMLHttp.OPEN("POST", m.lcURL + "/decript")
	m.loXMLHttp.setRequestHeader("Content-Type", "application/json")
	m.loXMLHttp.SEND(m.lcValue)

	DO WHILE .T.
		IF m.loXMLHttp.readyState = 4 OR LASTKEY() = 27
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
