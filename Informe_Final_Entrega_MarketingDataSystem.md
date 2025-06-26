# Informe Final de Entrega  
**Sistema de Centralización y Automatización de Datos de Marketing**

---

## 1. Resumen Ejecutivo

El proyecto consiste en el desarrollo de un sistema que automatiza la recolección, validación, normalización, enriquecimiento y consolidación de datos de ventas y stock provenientes de múltiples fuentes heterogéneas (APIs, archivos, bases de datos). El sistema centraliza la información en una base de datos relacional, genera reportes automáticos en Excel y expone una API REST segura y documentada.  
Se implementaron todos los requisitos funcionales y no funcionales definidos en el SRS, aplicando principios SOLID, patrones de diseño y buenas prácticas de arquitectura.

---

## 2. Cumplimiento de la Consigna y SRS

- **Modelado y migraciones:** Todas las entidades y relaciones están modeladas según el DER y UML. Migraciones y consistencia referencial aseguradas.
- **Lógica ETL:** Servicios para ingestión, validación, limpieza, normalización, enriquecimiento y deduplicación, con logs y reintentos automáticos.
- **Persistencia:** Uso de transacciones explícitas en operaciones críticas.
- **Automatización:** HostedService ejecuta el ETL diariamente a las 23:00, con reintentos y logs.
- **EventBus:** Arquitectura basada en eventos para desacoplar la generación de reportes.
- **Generación de reportes:** Reportes automáticos en Excel tras cada ingestión exitosa.
- **API REST:** Endpoints para ventas, stock, clientes, reportes, ingestión y autenticación JWT. Documentación Swagger publicada.
- **Pruebas:** Cobertura ≥ 80% con pruebas unitarias, integración y de endpoints.
- **Logs y auditoría:** Serilog con logs rotativos diarios y trazabilidad de eventos críticos.
- **Backups:** Instrucciones para backups automáticos documentadas.
- **Documentación:** SRS, README y guía de presentación actualizados y completos.

---

## 3. Arquitectura y Componentes

- **Capas:** Core (entidades/DTOs), Application (servicios/ETL), Infrastructure (persistencia/repositorios), API (controladores/endpoints), Tests.
- **Patrones:** Repository, Unit of Work, EventBus (Observer), DTO, Mapper, HostedService.
- **Tecnologías:** .NET Core, Entity Framework Core, SQL Server, Serilog, ClosedXML, JWT, Swagger, xUnit.

---

## 4. Automatización y Seguridad

- **ETL automático:** Proceso diario a las 23:00, configurable.
- **Reintentos:** Hasta 3 intentos automáticos ante fallos, con logs.
- **Seguridad:** JWT, HTTPS, control de acceso, logs de intentos fallidos.
- **Backups:** Recomendación y ejemplo de automatización.

---

## 5. Pruebas y Calidad

- **Pruebas unitarias y de integración:** Servicios, DTOs, mappers, endpoints.
- **Cobertura:** Validación de respuestas, errores y reglas de negocio.
- **Logs y auditoría:** Trazabilidad completa de procesos críticos.

---

## 6. Documentación y Presentación

- **README:** Instrucciones de instalación, uso, automatización, backups y logs.
- **Guía de presentación:** Resumen ejecutivo, arquitectura, automatización, pruebas y contacto.
- **Swagger:** Documentación interactiva de la API disponible en `/swagger`.

---

## 7. Estado Final

**El sistema cumple con todos los requisitos funcionales y no funcionales, está probado, documentado y listo para ser desplegado y presentado.**

---

# Preguntas y Respuestas para la Defensa

### 1. ¿Por qué se eligió una arquitectura en capas?
**Respuesta:**  
Permite separar responsabilidades, facilita el mantenimiento, la escalabilidad y el testing. Cada capa tiene una función clara: Core (modelo), Application (lógica), Infrastructure (persistencia), API (exposición de servicios).

---

### 2. ¿Cómo se asegura la integridad y consistencia de los datos?
**Respuesta:**  
Mediante validaciones en el pipeline ETL, uso de claves foráneas en la base de datos, y transacciones explícitas en operaciones críticas. Los registros inválidos o duplicados se descartan y se registran en logs.

---

### 3. ¿Cómo funciona la automatización diaria del proceso ETL?
**Respuesta:**  
Un HostedService ejecuta el proceso ETL todos los días a las 23:00. Si ocurre un error, se realizan hasta 3 reintentos automáticos, registrando cada intento y error en los logs.

---

### 4. ¿Qué ocurre si una fuente de datos falla?
**Respuesta:**  
El sistema realiza hasta 3 reintentos automáticos con intervalos de 10 minutos. Si persiste el error, se registra en los logs y se notifica el fallo, sin afectar el resto del proceso.

---

### 5. ¿Cómo se realiza la generación automática de reportes?
**Respuesta:**  
Al finalizar exitosamente el ETL, se emite un evento interno que dispara la generación de un archivo Excel con los datos consolidados, disponible para el equipo de marketing.

---

### 6. ¿Cómo se protege la API y los datos sensibles?
**Respuesta:**  
Se utiliza autenticación JWT, HTTPS, control de acceso por roles y logs de intentos fallidos. Las credenciales y cadenas de conexión se almacenan de forma segura.

---

### 7. ¿Cómo se gestionan los backups?
**Respuesta:**  
Se recomienda y documenta la automatización de backups de la base de datos mediante tareas programadas (ejemplo con `sqlcmd` en Windows).

---

### 8. ¿Cómo se asegura la trazabilidad y auditoría?
**Respuesta:**  
Todos los eventos críticos, inicios y finales de procesos, errores y reintentos se registran en logs rotativos diarios mediante Serilog, permitiendo auditoría y diagnóstico.

---

### 9. ¿Qué pruebas se implementaron y cómo se asegura la calidad?
**Respuesta:**  
Pruebas unitarias, de integración y de endpoints, cubriendo servicios, DTOs, mappers y controladores. La cobertura es ≥ 80% y se validan tanto casos de éxito como de error.

---

### 10. ¿Cómo se puede escalar o mantener el sistema?
**Respuesta:**  
La arquitectura modular y el uso de patrones permiten agregar nuevas fuentes, reglas o reportes sin afectar el resto del sistema. El código está documentado y las pruebas automatizadas aseguran que los cambios no rompan funcionalidades existentes.

---

### 11. ¿Cómo se maneja el enriquecimiento de datos?
**Respuesta:**  
Si un producto tiene información incompleta, el sistema consulta automáticamente una API externa para completar los datos antes de almacenarlos.

---

### 12. ¿Qué ocurre si se detectan registros duplicados?
**Respuesta:**  
El pipeline ETL incluye un servicio de deduplicación que compara los registros nuevos con los existentes y descarta los duplicados antes de la persistencia.

---

### 13. ¿Cómo se accede a la documentación de la API?
**Respuesta:**  
La documentación interactiva está disponible en `/swagger` y permite probar los endpoints directamente desde el navegador.

---

### 14. ¿Qué tecnologías y librerías principales se utilizaron?
**Respuesta:**  
.NET Core, Entity Framework Core, SQL Server, Serilog, ClosedXML, JWT, Swagger, xUnit.

---

### 15. ¿Cómo se puede modificar o extender el sistema?
**Respuesta:**  
Gracias a la arquitectura desacoplada y el uso de interfaces, se pueden agregar nuevas fuentes de datos, reglas de validación o reportes con cambios mínimos y pruebas automatizadas.

---

# Glosario

**API REST:**  
Interfaz de programación que permite la comunicación entre sistemas a través de HTTP siguiendo el estilo arquitectónico REST. Expone recursos mediante endpoints (URLs) y métodos HTTP (GET, POST, PUT, DELETE).

**Automatización ETL:**  
Proceso programado que ejecuta automáticamente la recolección (Extract), transformación (Transform) y carga (Load) de datos desde múltiples fuentes hacia una base de datos central.

**Backups Automáticos:**  
Copias de seguridad programadas de la base de datos para prevenir pérdida de información ante fallos o errores.

**Base de Datos Relacional:**  
Sistema de almacenamiento de datos estructurados en tablas relacionadas entre sí mediante claves primarias y foráneas (ejemplo: SQL Server).

**ClosedXML:**  
Librería de .NET para crear y manipular archivos Excel (.xlsx) de forma sencilla y eficiente.

**Consistencia Referencial:**  
Propiedad de la base de datos que asegura que las relaciones entre tablas sean válidas, evitando registros huérfanos o referencias inválidas.

**DTO (Data Transfer Object):**  
Objeto simple que transporta datos entre capas del sistema, evitando exponer directamente las entidades del dominio.

**Deduplicación:**  
Proceso de eliminar registros duplicados para asegurar que cada dato esté almacenado una sola vez.

**Enriquecimiento de Datos:**  
Proceso de completar o mejorar la información de un registro consultando fuentes externas (por ejemplo, obtener nombre y categoría de un producto a partir de su ID).

**Entity Framework Core:**  
ORM (Object-Relational Mapper) de .NET que permite mapear clases de C# a tablas de una base de datos relacional y realizar operaciones CRUD de forma sencilla.

**EventBus (Bus de Eventos):**  
Mecanismo interno que permite la comunicación desacoplada entre componentes mediante la publicación y suscripción a eventos.

**HostedService:**  
Servicio en segundo plano de .NET que permite ejecutar tareas programadas o continuas, como la automatización diaria del ETL.

**JWT (JSON Web Token):**  
Estándar para autenticación y autorización basado en tokens firmados digitalmente, que se transmiten en las cabeceras HTTP para acceder a recursos protegidos.

**Logs Rotativos:**  
Archivos de registro que se generan y archivan automáticamente por día, permitiendo auditoría y diagnóstico sin saturar el almacenamiento.

**Mapper:**  
Componente que transforma datos entre diferentes representaciones, por ejemplo, de entidad a DTO y viceversa.

**Migraciones:**  
Conjunto de scripts o instrucciones que permiten crear, modificar o actualizar la estructura de la base de datos de forma controlada y reproducible.

**Patrones de Diseño:**  
Soluciones reutilizables a problemas comunes de arquitectura y desarrollo de software. Ejemplos: Repository, Unit of Work, Observer/EventBus.

**Pipeline ETL:**  
Secuencia de etapas por las que pasan los datos: validación, transformación, enriquecimiento, deduplicación y persistencia.

**Pruebas de Integración:**  
Tests que validan el funcionamiento conjunto de varios componentes o servicios, asegurando que interactúan correctamente.

**Pruebas Unitarias:**  
Tests que validan el comportamiento de una función, método o clase de forma aislada.

**Repository:**  
Patrón que abstrae el acceso a los datos, permitiendo operar sobre entidades sin exponer detalles de la base de datos.

**Serilog:**  
Librería de logging para .NET que permite registrar información estructurada y rotar archivos de logs automáticamente.

**SOLID:**  
Conjunto de principios de diseño orientados a mejorar la mantenibilidad, escalabilidad y calidad del software:
- S: Responsabilidad Única
- O: Abierto/Cerrado
- L: Sustitución de Liskov
- I: Segregación de Interfaces
- D: Inversión de Dependencias

**Swagger:**  
Herramienta que genera documentación interactiva para APIs REST, permitiendo probar los endpoints desde el navegador.

**Transacción:**  
Operación atómica sobre la base de datos que asegura que un conjunto de cambios se realice completamente o no se realice en absoluto.

**Unit of Work:**  
Patrón que coordina la escritura de cambios en varias entidades como una única transacción.

**xUnit:**  
Framework de pruebas unitarias para .NET. 