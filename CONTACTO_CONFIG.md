# Configuración del Formulario de Contacto

## Formulario de Contacto

Se ha añadido un formulario de contacto en la página `/Contacto` con los siguientes campos:

### Campos del formulario:
- **Nombre** (obligatorio)
- **Email** (obligatorio)
- **Teléfono** (opcional)
- **Asunto** (opcional)
- **Mensaje** (obligatorio)

### Pregunta de seguridad:
"Huevo, perro, abeja o viernes, ¿cuál es el día de la semana?"
- Respuesta correcta: **viernes**

## Configuración de Email

El formulario envía emails a `albert.capdevila@bahiacode.com` cuando se completa correctamente.

### Configuración SMTP

Para habilitar el envío de emails en producción, configura las siguientes variables en `appsettings.json` o mediante variables de entorno:

```json
{
  "Email": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": "587",
    "SmtpUser": "tu-email@gmail.com",
    "SmtpPassword": "tu-contraseña-o-app-password"
  }
}
```

### Notas importantes:

1. **Gmail App Password**: Si usas Gmail, necesitarás crear una "App Password" en lugar de usar tu contraseña normal. Visita: https://myaccount.google.com/apppasswords

2. **Desarrollo**: En ambiente de desarrollo (sin configuración SMTP), el sistema simulará el envío del email y registrará la información en los logs.

3. **Seguridad**: Nunca cometas las credenciales SMTP en el código fuente. Usa variables de entorno o Azure Key Vault en producción.

### Ejemplo con variables de entorno:

```bash
export Email__SmtpHost="smtp.gmail.com"
export Email__SmtpPort="587"
export Email__SmtpUser="tu-email@gmail.com"
export Email__SmtpPassword="tu-app-password"
```

## Validación del formulario

El formulario incluye:
- Validación del lado del cliente usando jQuery Validation
- Validación del lado del servidor usando Data Annotations
- Validación de la pregunta de seguridad para prevenir spam
- Mensajes de error descriptivos en español
