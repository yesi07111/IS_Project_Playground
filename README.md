# 🎮 PlayGround

Bienvenido al proyecto **PlayGround**. Este documento te guiará para configurar y ejecutar tanto el frontend como el backend en tu entorno local.

## 🌐 Frontend

El frontend de este proyecto está construido con React utilizando Vite. Sigue los pasos a continuación para configurarlo y ejecutarlo en tu máquina local.

### 📁 Estructura de Carpetas

- `Playground/frontend`: Contiene el código fuente del frontend.

### 🛠️ Configuración Inicial

1. **Crear archivo `.env`**: En la carpeta `frontend`, crea un archivo llamado `.env` con el siguiente contenido:

   ```plaintext
   VITE_BASE_URL=http://localhost:5173
   ```

2. **Instalar dependencias**: Navega a la carpeta del frontend y ejecuta el siguiente comando para instalar las dependencias necesarias:

   ```bash
   cd Playground/frontend
   npm install
   ```

   > **Nota**: Los `node_modules` ocupan aproximadamente ~560 MB.

### 🚀 Ejecutar el Frontend

Para iniciar el servidor de desarrollo, ejecuta:

```bash
npm run dev
```

Esto iniciará el servidor en `http://localhost:5173`.

## 🖥️ Backend

El backend está desarrollado con .NET. Sigue los pasos a continuación para configurarlo y ejecutarlo.

### 📁 Estructura de Carpetas

- `Playground/backend/src/WebApi`: Contiene el código fuente del backend.

### 🛠️ Configuración Inicial

1. **Restaurar paquetes**: Navega a la carpeta `WebApi` y ejecuta el siguiente comando para restaurar los paquetes necesarios:

   ```bash
   cd Playground/backend/src/WebApi
   dotnet restore
   ```

### 🚀 Ejecutar el Backend

Para iniciar el servidor backend, ejecuta:

```bash
dotnet run
```

Una vez iniciado, puedes acceder a la documentación de la API en `http://localhost:<puerto>/swagger`.

## 📜 Swagger

### ¿Qué es Swagger?

Swagger es una herramienta para documentar y probar APIs. Proporciona una interfaz web interactiva que permite a los desarrolladores explorar y probar los endpoints de la API.

### 🚀 Usar Swagger para Testear Endpoints

1. **Acceder a Swagger**: Una vez que el backend esté corriendo, abre tu navegador y ve a `http://localhost:<puerto>/swagger`.

2. **Explorar Endpoints**: Swagger te mostrará una lista de todos los endpoints disponibles en la API. Puedes expandir cada uno para ver detalles como los parámetros requeridos y las respuestas esperadas.

3. **Probar Endpoints**: Usa la interfaz de Swagger para enviar solicitudes a los endpoints y ver las respuestas directamente en el navegador.

Con estos pasos, deberías poder configurar y ejecutar tanto el frontend como el backend de tu proyecto PlayGround en tu entorno local. ¡Disfruta desarrollando! 🚀
