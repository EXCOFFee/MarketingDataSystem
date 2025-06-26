# 📚 Guía Completa de Swagger UI - Sistema Marketing Data

*Guía paso a paso para usar la interfaz de Swagger sin conocimientos técnicos*

---

## 🤔 ¿Qué es Swagger?

**Swagger** es una interfaz web que te permite **probar la API sin escribir código**. Es como tener un "laboratorio" donde puedes:
- 📋 Ver todos los endpoints disponibles
- 🧪 Probar cada endpoint directamente desde el navegador
- 📊 Ver ejemplos de requests y responses
- 🔐 Autenticarte automáticamente
- 📖 Leer documentación interactiva

**¡Es perfecto para principiantes!** No necesitas Postman, curl, ni conocimiento avanzado.

---

## 🚀 Paso 1: Iniciar el Sistema

1. **Abrir terminal/PowerShell** en la carpeta del proyecto
2. **Ejecutar el proyecto:**
   ```bash
   dotnet run --project MarketingDataSystem.API --urls "http://localhost:5056"
   ```
3. **Esperar a ver este mensaje:**
   ```
   ✅ Usuario admin ya existe en la base de datos.
   Now listening on: http://localhost:5056
   Application started. Press Ctrl+C to shut down.
   ```

---

## 🌐 Paso 2: Acceder a Swagger

1. **Abrir tu navegador** (Chrome, Firefox, Edge, etc.)
2. **Ir a la URL:** `http://localhost:5056/swagger`
3. **Deberás ver la interfaz de Swagger** con una lista de endpoints

---

## 🎯 Paso 3: Entender la Interfaz

Cuando abras Swagger verás **secciones colapsables**:

```
🔐 Auth          - Autenticación (login, registro)
👥 Cliente       - Gestión de clientes
📦 Producto      - Catálogo de productos
💰 Venta         - Registro de ventas
📊 Stock         - Control de inventario
🏠 FuenteDeDatos - Configuración de fuentes
📈 Reports       - Generación de reportes
⚙️  Admin        - Funciones administrativas
🔄 Ingestion     - Procesos ETL
❤️  Health       - Estado del sistema
```

---

## 🔑 Paso 4: Autenticarse (MUY IMPORTANTE)

**ANTES de usar cualquier endpoint protegido, DEBES autenticarte:**

### 4.1 Hacer Login:
1. **Buscar la sección "Auth"** y hacer clic para expandirla
2. **Buscar "POST /api/Auth/login"** 
3. **Hacer clic en "Try it out"** (botón azul)
4. **En el campo de texto, pegar:**
   ```json
   {
     "email": "admin@marketingdata.com",
     "password": "admin123"
   }
   ```
5. **Hacer clic en "Execute"** (botón azul grande)
6. **Copiar el token** de la respuesta (empieza con "eyJ...")

### 4.2 Configurar Autorización:
1. **Buscar el botón "Authorize" 🔐** (arriba a la derecha)
2. **Hacer clic en "Authorize"**
3. **En el campo "Value", escribir:** `Bearer ` + `tu-token`
   ```
   Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhZG1pb...
   ```
4. **Hacer clic en "Authorize"**
5. **Hacer clic en "Close"**

**¡Ahora ya puedes usar todos los endpoints!** 🎉

---

## 📋 Guía de Cada Endpoint

### 🔐 **AUTH - Autenticación** (No requieren token)

**🔑 POST /api/Auth/login** - Iniciar sesión
- **Para qué:** Obtener token de acceso
- **Datos necesarios:**
  ```json
  {
    "email": "admin@marketingdata.com",
    "password": "admin123"
  }
  ```

**👤 POST /api/Auth/register** - Registrar nuevo usuario
- **Para qué:** Crear una cuenta nueva
- **Datos necesarios:**
  ```json
  {
    "username": "nuevo_usuario",
    "email": "usuario@empresa.com",
    "password": "MiPassword123!",
    "nombre": "Juan Pérez"
  }
  ```

### 👥 **CLIENTE - Gestión de Clientes**

**📋 GET /api/Cliente** - Ver todos los clientes
- **Para qué:** Lista completa de clientes
- **Cómo usar:** Clic en "Try it out" → "Execute"

**🔍 GET /api/Cliente/{id}** - Ver cliente específico
- **Para qué:** Detalles de un cliente
- **Cómo usar:** Escribir ID del cliente (ej: `1`) → "Execute"

**➕ POST /api/Cliente** - Crear cliente nuevo
- **Para qué:** Agregar cliente al sistema
- **Datos necesarios:**
  ```json
  {
    "nombre": "María González",
    "email": "maria@email.com",
    "telefono": "+54 11 1234-5678",
    "direccion": "Av. Córdoba 1234, CABA",
    "fechaNacimiento": "1985-03-15"
  }
  ```

**✏️ PUT /api/Cliente/{id}** - Actualizar cliente
- **Para qué:** Modificar datos existentes
- **Cómo usar:** ID del cliente + datos actualizados

**🗑️ DELETE /api/Cliente/{id}** - Eliminar cliente
- **Para qué:** Borrar cliente del sistema
- **Cómo usar:** Solo escribir el ID → "Execute"

### 📦 **PRODUCTO - Catálogo**

**📋 GET /api/Producto** - Ver todos los productos
- **Para qué:** Catálogo completo con precios y stock

**🔍 GET /api/Producto/search** - Buscar productos
- **Para qué:** Encontrar productos por nombre
- **Ejemplo:** `nombre = "Laptop"`

**➕ POST /api/Producto** - Crear producto nuevo
- **Datos necesarios:**
  ```json
  {
    "nombre": "Laptop Dell Inspiron",
    "descripcion": "Laptop para oficina",
    "precio": 85000.00,
    "categoria": "Computadoras",
    "codigoBarras": "1234567890123"
  }
  ```

### 💰 **VENTA - Registro de Ventas**

**📋 GET /api/Venta** - Ver todas las ventas
- **Para qué:** Historial completo de ventas

**📊 GET /api/Venta/estadisticas** - Métricas de ventas
- **Para qué:** KPIs y estadísticas del negocio

**📅 GET /api/Venta/por-fecha** - Filtrar por fechas
- **Para qué:** Ver ventas en período específico
- **Parámetros:** 
  - `fechaInicio`: "2024-01-01"
  - `fechaFin`: "2024-12-31"

**➕ POST /api/Venta** - Registrar venta nueva
- **Datos necesarios:**
  ```json
  {
    "clienteId": 1,
    "productoId": 1,
    "cantidad": 2,
    "precioUnitario": 85000.00,
    "fechaVenta": "2024-06-19T15:30:00"
  }
  ```

### 📊 **STOCK - Control de Inventario**

**📋 GET /api/Stock** - Ver inventario completo
- **Para qué:** Estado actual de todos los productos

**⚠️ GET /api/Stock/stock-bajo** - Productos con stock bajo
- **Para qué:** Identificar productos que necesitan reposición
- **Parámetro:** `stockMinimo` (ej: 10)

**🔧 POST /api/Stock/{id}/ajustar** - Ajustar cantidad
- **Para qué:** Modificar stock de un producto

### 📈 **REPORTS - ¡LO MÁS IMPORTANTE!**

**🎯 POST /api/Reports/generate** - **¡GENERAR REPORTE EXCEL!**
- **Para qué:** Crear archivo Excel profesional con 4 hojas
- **Cómo usar:** 
  1. Clic en "Try it out"
  2. Clic en "Execute"
  3. **El archivo se guarda en:** `MarketingDataSystem.API/reportes/`
- **Contenido del Excel:**
  - 📊 **Hoja 1:** Ventas del día con totales
  - 👥 **Hoja 2:** Resumen de clientes  
  - 📦 **Hoja 3:** Stock actual con alertas
  - 📈 **Hoja 4:** Resumen ejecutivo con KPIs

**📋 GET /api/Reports** - Ver reportes generados
- **Para qué:** Lista de archivos Excel creados

### ⚙️ **ADMIN - Funciones Administrativas**

**❤️ GET /api/Admin/system/status** - Estado del sistema
- **Para qué:** Verificar que todo funcione correctamente

**💾 POST /api/Admin/backup/completo** - Crear backup
- **Para qué:** Generar copia de seguridad

**🚨 POST /api/Admin/alerta/prueba** - Enviar alerta de prueba
- **Para qué:** Verificar notificaciones

### ❤️ **HEALTH - Estado del Sistema**

**✅ GET /health** - Verificar salud del sistema
- **Para qué:** Monitoreo (no requiere token)
- **Respuesta:** "Healthy" si todo está bien

---

## 🎯 Casos de Uso Prácticos

### **Escenario 1: Primera venta completa**
1. **Crear cliente** → POST /api/Cliente
2. **Crear producto** → POST /api/Producto
3. **Verificar stock** → GET /api/Stock
4. **Registrar venta** → POST /api/Venta
5. **Generar reporte** → POST /api/Reports/generate
6. **Abrir Excel** en carpeta `reportes/`

### **Escenario 2: Control de inventario**
1. **Ver stock actual** → GET /api/Stock
2. **Identificar productos bajos** → GET /api/Stock/stock-bajo
3. **Ajustar cantidades** → POST /api/Stock/{id}/ajustar

### **Escenario 3: Análisis de ventas**
1. **Ver estadísticas** → GET /api/Venta/estadisticas
2. **Filtrar por fechas** → GET /api/Venta/por-fecha
3. **Generar reporte Excel** → POST /api/Reports/generate

---

## 🔧 Interpretando las Respuestas

### **✅ Respuesta Exitosa (Verde - 200):**
```json
{
  "id": 1,
  "nombre": "Cliente creado",
  "email": "cliente@email.com"
}
```
**¡Todo salió bien!**

### **❌ Error de Autenticación (Rojo - 401):**
```json
{
  "error": "Token JWT inválido o expirado"
}
```
**Solución:** Hacer login nuevamente y actualizar el token.

### **🚫 Error de Validación (Rojo - 400):**
```json
{
  "errors": {
    "Email": ["El campo Email es requerido"],
    "Nombre": ["El nombre debe tener al menos 2 caracteres"]
  }
}
```
**Solución:** Corregir los datos y intentar de nuevo.

### **❓ Recurso No Encontrado (Rojo - 404):**
```json
{
  "error": "Cliente con ID 999 no encontrado"
}
```
**Solución:** Verificar que el ID existe.

---

## 🚨 Problemas Comunes y Soluciones

### **🔐 "Unauthorized" en todos los endpoints:**
- **Problema:** No te autenticaste
- **Solución:** 
  1. Login en `/api/Auth/login`
  2. Copiar token completo
  3. "Authorize" → `Bearer tu-token`

### **⏰ "Token expired":**
- **Problema:** Token venció (1 hora)
- **Solución:** Hacer login nuevamente

### **🌐 "Failed to fetch":**
- **Problema:** Servidor no está corriendo
- **Solución:** Verificar `http://localhost:5056`

### **📋 Respuestas vacías []:**
- **Problema:** No hay datos en la base
- **Solución:** Crear registros primero (POST)

---

## 💡 Consejos Pro

1. **🔖 Usa "Schemas"** al final de Swagger para ver modelos
2. **📋 Copia ejemplos** automáticos de Swagger
3. **🔄 Recarga** si algo no funciona
4. **📊 Prueba GET** antes de crear datos
5. **💾 Genera reportes** frecuentemente
6. **⚡ Usa Health Check** para verificar estado

---

## 🎉 ¡Listo para usar!

Con esta guía puedes:
- ✅ Autenticarte correctamente
- ✅ Probar todos los endpoints
- ✅ Crear, leer, actualizar y eliminar datos
- ✅ Generar reportes Excel profesionales
- ✅ Monitorear el sistema
- ✅ Solucionar problemas

**¡Swagger te da control total de la API sin escribir código!** 🚀

---

## 📞 Contacto

- **Proyecto:** Sistema de Marketing Data
- **URL Swagger:** `http://localhost:5056/swagger`
- **Credenciales:** `admin@marketingdata.com` / `admin123` 