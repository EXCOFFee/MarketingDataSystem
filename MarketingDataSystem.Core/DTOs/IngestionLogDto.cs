// ==================== DTO DE LOG DE INGESTA ETL - AUDITORÍA Y MONITOREO ====================
// PROPÓSITO: Representa logs críticos de procesos ETL para auditoría y troubleshooting
// CRITICIDAD: ALTA - Información vital para compliance, diagnóstico y continuidad de negocio
// COMPLIANCE: Sujeto a regulaciones de auditoría, retención de logs y trazabilidad operacional
// SEGURIDAD: Información sensible sobre fuentes de datos y operaciones críticas del sistema
// ESCALABILIDAD: Diseñado para manejar millones de logs diarios en pipelines ETL empresariales

using System;

namespace MarketingDataSystem.Core.DTOs
{
    /// <summary>
    /// DTO DE LOG DE INGESTA ETL PARA AUDITORÍA EMPRESARIAL
    /// ==================================================
    /// 
    /// PROPÓSITO EMPRESARIAL:
    /// Representa el registro completo de ejecución de procesos de ingesta de datos ETL,
    /// incluyendo información crítica para auditoría, compliance, troubleshooting, monitoreo
    /// operacional y análisis de performance. Es fundamental para garantizar la integridad
    /// y trazabilidad de los datos empresariales críticos.
    /// 
    /// CASOS DE USO EMPRESARIALES CRÍTICOS:
    /// 
    /// 1. **AUDITORÍA Y COMPLIANCE REGULATORIO**
    ///    - Auditorías SOX para trazabilidad de datos financieros desde fuentes originales
    ///    - Compliance GDPR para demostrar procesamiento legítimo de datos personales
    ///    - Auditorías internas para verificar integridad de procesos de datos críticos
    ///    - Reportes regulatorios que requieren evidencia de procesamiento de datos
    ///    - Documentación para auditorías externas de sistemas de información financiera
    ///    - Compliance con ISO 27001 para gestión de seguridad de información
    /// 
    /// 2. **OPERACIONES ETL Y DATA ENGINEERING**
    ///    - Monitoreo en tiempo real de salud de pipelines ETL críticos para el negocio
    ///    - Alertas automáticas por fallos en ingesta de datos de sistemas ERP/CRM
    ///    - Análisis de performance de procesos ETL para optimización de infraestructura
    ///    - Planificación de capacidad basada en volúmenes históricos de procesamiento
    ///    - Detección proactiva de degradación en performance de fuentes de datos
    ///    - Coordinación de procesos ETL durante ventanas de mantenimiento
    /// 
    /// 3. **TROUBLESHOOTING Y RESOLUCIÓN DE INCIDENTES**
    ///    - Diagnóstico rápido de fallos en procesos críticos de actualización de datos
    ///    - Análisis de root cause para incidentes de calidad de datos empresariales
    ///    - Identificación de patrones de fallo en fuentes de datos específicas
    ///    - Timeline de eventos para investigación de incidentes de datos
    ///    - Correlación de errores entre múltiples fuentes de datos corporativas
    ///    - Análisis forense de modificaciones no autorizadas en datos críticos
    /// 
    /// 4. **BUSINESS INTELLIGENCE Y REPORTING**
    ///    - Verificación de freshness de datos para reportes ejecutivos críticos
    ///    - Validación de completitud de datos antes de generación de reportes financieros
    ///    - Métricas de SLA para procesos de actualización de dashboards ejecutivos
    ///    - Análisis de disponibilidad de datos para decisiones empresariales críticas
    ///    - Monitoreo de dependencias entre fuentes para reportes consolidados
    ///    - Alertas de datos obsoletos que podrían impactar decisiones estratégicas
    /// 
    /// 5. **CONTINUIDAD DE NEGOCIO Y DISASTER RECOVERY**
    ///    - Identificación de fuentes de datos críticas que requieren backup prioritario
    ///    - Análisis de impacto de fallos en fuentes para planes de contingencia
    ///    - Métricas de RTO/RPO para procesos de recuperación de datos críticos
    ///    - Validación de integridad de datos tras procedimientos de disaster recovery
    ///    - Monitoreo de replicación de datos entre centros de datos corporativos
    ///    - Alertas de indisponibilidad que pueden afectar operaciones críticas
    /// 
    /// 6. **PERFORMANCE Y OPTIMIZACIÓN EMPRESARIAL**
    ///    - Análisis de cuellos de botella en procesamiento de datos de alta prioridad
    ///    - Optimización de ventanas de procesamiento para minimizar impacto al negocio
    ///    - Identificación de oportunidades de paralelización de procesos ETL
    ///    - Análisis de costo-beneficio de optimizaciones de infraestructura
    ///    - Métricas de eficiencia para justificación de inversiones en tecnología
    ///    - Benchmarking de performance contra SLAs empresariales establecidos
    /// 
    /// 7. **DATA GOVERNANCE Y CALIDAD**
    ///    - Monitoreo de calidad de datos desde fuentes corporativas críticas
    ///    - Alertas de anomalías en volúmenes de datos que pueden indicar problemas
    ///    - Tracking de linaje de datos para compliance con políticas de governance
    ///    - Validación de conformidad con estándares de calidad empresariales
    ///    - Métricas de completitud y consistencia de datos por fuente
    ///    - Documentación de transformaciones para auditorías de calidad
    /// 
    /// 8. **INTEGRACIÓN EMPRESARIAL Y APIS**
    ///    - Monitoreo de salud de conexiones con sistemas ERP (SAP, Oracle, Dynamics)
    ///    - Alertas de fallos en integración con plataformas CRM empresariales
    ///    - Tracking de rate limiting y quotas en APIs de terceros críticos
    ///    - Análisis de latencia en integración con sistemas cloud corporativos
    ///    - Monitoreo de cambios en esquemas de APIs que pueden afectar ETL
    ///    - Validación de autenticación y autorización en fuentes externas
    /// 
    /// TIPOS DE FUENTES MONITOREADAS:
    /// - **ERP Systems**: SAP, Oracle EBS, Microsoft Dynamics, NetSuite
    /// - **CRM Platforms**: Salesforce, HubSpot, Microsoft Dynamics CRM
    /// - **Financial Systems**: QuickBooks, Sage, SAP FICO, Oracle Financials
    /// - **E-commerce**: Shopify, Magento, WooCommerce, Amazon Marketplace
    /// - **Marketing Platforms**: Google Analytics, Adobe Analytics, Marketo
    /// - **Cloud Storage**: AWS S3, Azure Blob Storage, Google Cloud Storage
    /// - **Databases**: SQL Server, PostgreSQL, MySQL, Oracle, MongoDB
    /// - **APIs**: REST, GraphQL, SOAP, WebSocket, Message Queues
    /// 
    /// ESTADOS DE PROCESAMIENTO EMPRESARIAL:
    /// - **INICIADO**: Proceso ETL iniciado exitosamente, recursos asignados
    /// - **EN_PROGRESO**: Procesamiento activo, datos siendo transformados
    /// - **COMPLETADO**: Proceso finalizado exitosamente, datos disponibles
    /// - **FALLIDO**: Error crítico que impide completar procesamiento
    /// - **ADVERTENCIA**: Completado con issues que requieren atención
    /// - **CANCELADO**: Proceso cancelado manualmente por operaciones
    /// - **TIMEOUT**: Proceso excedió tiempo límite configurado
    /// - **REINTENTANDO**: Reintento automático tras fallo temporal
    /// 
    /// CLASIFICACIÓN DE ERRORES:
    /// - **CRÍTICO**: Fallo que impacta operaciones de negocio críticas
    /// - **ALTO**: Error que afecta reportes o análisis empresariales
    /// - **MEDIO**: Issue que puede resolverse automáticamente
    /// - **BAJO**: Advertencia que no afecta funcionamiento normal
    /// - **INFORMACIÓN**: Log informativo para análisis de tendencias
    /// 
    /// COMPLIANCE Y REGULACIONES:
    /// - **SOX (Sarbanes-Oxley)**: Trazabilidad de datos financieros críticos
    /// - **GDPR**: Registro de procesamiento de datos personales europeos
    /// - **CCPA**: Compliance con California Consumer Privacy Act
    /// - **HIPAA**: Si procesa datos de salud, logging específico requerido
    /// - **PCI DSS**: Si maneja datos de pago, auditoría de acceso requerida
    /// - **ISO 27001**: Gestión de logs de seguridad de información
    /// 
    /// RETENCIÓN DE LOGS EMPRESARIAL:
    /// - **Logs Críticos**: 10 años para compliance regulatorio
    /// - **Logs Operacionales**: 5 años para análisis histórico
    /// - **Logs de Performance**: 3 años para optimización
    /// - **Logs de Debug**: 1 año para troubleshooting
    /// - **Logs de Auditoría**: Retención permanente según regulaciones
    /// 
    /// PERFORMANCE Y ESCALABILIDAD:
    /// - **High-Volume Logging**: Manejo de millones de logs diarios
    /// - **Real-time Processing**: Procesamiento en tiempo real para alertas
    /// - **Batch Processing**: Procesamiento batch eficiente para análisis
    /// - **Partitioning**: Particionamiento temporal para performance
    /// - **Indexing**: Índices optimizados para consultas frecuentes
    /// - **Archiving**: Archivado automático de logs históricos
    /// 
    /// SEGURIDAD DE LOGS:
    /// - **Encryption**: Encriptación de logs con información sensible
    /// - **Access Control**: Control granular de acceso a logs por rol
    /// - **Audit Trail**: Registro de acceso a logs para compliance
    /// - **Integrity**: Verificación de integridad de logs críticos
    /// - **Anonymization**: Anonimización de datos sensibles en logs
    /// - **Secure Storage**: Almacenamiento seguro con respaldo encriptado
    /// 
    /// ALERTAS Y MONITOREO:
    /// - **Real-time Alerts**: Alertas inmediatas por fallos críticos
    /// - **Threshold Monitoring**: Monitoreo de umbrales de performance
    /// - **Anomaly Detection**: Detección de anomalías en patrones de datos
    /// - **Escalation**: Escalación automática según severidad de errores
    /// - **Dashboard Integration**: Integración con dashboards operacionales
    /// - **Mobile Notifications**: Notificaciones móviles para eventos críticos
    /// 
    /// INTEGRACIÓN CON HERRAMIENTAS EMPRESARIALES:
    /// - **SIEM Systems**: Integración con herramientas de seguridad empresarial
    /// - **Monitoring Tools**: Splunk, Datadog, New Relic, AppDynamics
    /// - **Ticketing Systems**: Jira, ServiceNow, BMC Remedy
    /// - **Communication**: Slack, Microsoft Teams, email corporativo
    /// - **BI Platforms**: Power BI, Tableau, Qlik para análisis de logs
    /// - **Cloud Logging**: AWS CloudWatch, Azure Monitor, Google Cloud Logging
    /// </summary>
    public class IngestionLogDto
    {
        // ========== IDENTIFICADOR ÚNICO DEL LOG ==========
        /// <summary>
        /// IDENTIFICADOR ÚNICO DEL LOG DE INGESTA
        /// ====================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Identificador único e inmutable del registro de log de ingesta ETL.
        /// Es fundamental para trazabilidad, correlación de eventos, troubleshooting
        /// y compliance con auditorías regulatorias que requieren identificación
        /// precisa de procesos de datos empresariales.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - AUDIT TRAIL: Identificación única en auditorías de procesos ETL
        /// - CORRELATION: Correlación de eventos entre diferentes sistemas
        /// - TROUBLESHOOTING: Referencia específica para análisis de problemas
        /// - COMPLIANCE: Identificación en reportes regulatorios de datos
        /// - MONITORING: Clave para dashboards de monitoreo operacional
        /// - FORENSICS: Investigación de incidentes de seguridad de datos
        /// - PERFORMANCE: Análisis de performance por proceso específico
        /// - ALERTING: Referencia en alertas y notificaciones automáticas
        /// 
        /// CONSIDERACIONES TÉCNICAS:
        /// - IMMUTABLE: Permanece constante durante todo el ciclo de vida
        /// - UNIQUE: Garantiza unicidad absoluta en todo el sistema
        /// - INDEXED: Índice primario para consultas de alta performance
        /// - SEQUENTIAL: Facilita ordenamiento temporal de eventos
        /// 
        /// COMPLIANCE Y AUDITORÍA:
        /// - SOX: Trazabilidad de procesos financieros críticos
        /// - GDPR: Identificación de procesamiento de datos personales
        /// - AUDIT LOGS: Referencia permanente en logs de compliance
        /// </summary>
        public int Id { get; set; }

        // ========== IDENTIFICACIÓN DE FUENTE DE DATOS ==========
        /// <summary>
        /// IDENTIFICADOR DE FUENTE DE DATOS EMPRESARIAL
        /// ==========================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Identificador de la fuente de datos específica procesada en esta ejecución
        /// del pipeline ETL. Es crítico para identificar qué sistema empresarial
        /// (ERP, CRM, Financial, etc.) está siendo procesado y para análisis de
        /// dependencies y impacto en operaciones de negocio.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - SOURCE DEPENDENCY: Identificación de dependencias entre fuentes críticas
        /// - IMPACT ANALYSIS: Análisis de impacto de fallos por fuente específica
        /// - PERFORMANCE TRACKING: Métricas de performance por fuente empresarial
        /// - CAPACITY PLANNING: Planificación de recursos por fuente de datos
        /// - SLA MONITORING: Monitoreo de SLAs específicos por fuente crítica
        /// - SECURITY AUDIT: Auditoría de acceso a fuentes sensibles por ID
        /// - COST ALLOCATION: Asignación de costos de procesamiento por fuente
        /// - BUSINESS CONTINUITY: Identificación de fuentes críticas para BC/DR
        /// 
        /// TIPOS DE FUENTES EMPRESARIALES:
        /// - ERP_SAP: Sistema SAP corporativo (ID: 1001-1099)
        /// - CRM_SALESFORCE: Plataforma Salesforce (ID: 2001-2099)
        /// - FINANCIAL_ORACLE: Oracle Financials (ID: 3001-3099)
        /// - ECOMMERCE_SHOPIFY: Plataforma e-commerce (ID: 4001-4099)
        /// - MARKETING_GA: Google Analytics (ID: 5001-5099)
        /// - CLOUD_S3: AWS S3 Storage (ID: 6001-6099)
        /// 
        /// CRITICIDAD POR RANGO:
        /// - 1000-1999: Fuentes críticas de nivel 1 (ERP, Financial)
        /// - 2000-2999: Fuentes importantes de nivel 2 (CRM, Sales)
        /// - 3000-3999: Fuentes operacionales de nivel 3 (Marketing, Analytics)
        /// - 4000-4999: Fuentes de soporte de nivel 4 (Logs, Monitoring)
        /// 
        /// RELACIÓN CON BUSINESS IMPACT:
        /// - Permite identificar rápidamente el impacto al negocio de fallos
        /// - Facilita priorización de resolución de problemas por criticidad
        /// - Habilita alertas específicas según importancia de la fuente
        /// - Soporta análisis de costo-beneficio de optimizaciones
        /// </summary>
        public int IdFuente { get; set; }

        // ========== TIMESTAMPS DE EJECUCIÓN ==========
        /// <summary>
        /// TIMESTAMP DE INICIO DE PROCESAMIENTO
        /// ==================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Fecha y hora exacta de inicio del proceso de ingesta ETL en UTC.
        /// Es fundamental para análisis de performance, cumplimiento de SLAs,
        /// troubleshooting temporal y compliance con auditorías que requieren
        /// trazabilidad temporal precisa de operaciones críticas.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - SLA COMPLIANCE: Verificación de cumplimiento de SLAs de procesamiento
        /// - PERFORMANCE ANALYSIS: Análisis de patrones temporales de performance
        /// - SCHEDULING OPTIMIZATION: Optimización de ventanas de procesamiento
        /// - BUSINESS HOURS IMPACT: Análisis de impacto en horarios de negocio
        /// - CAPACITY PLANNING: Planificación de capacidad basada en patrones temporales
        /// - INCIDENT TIMELINE: Timeline de eventos para resolución de incidentes
        /// - COMPLIANCE REPORTING: Timestamps exactos para reportes regulatorios
        /// - AUDIT TRAIL: Evidencia temporal para auditorías de procesos
        /// 
        /// PRECISIÓN Y ESTÁNDARES:
        /// - UTC TIMEZONE: Consistencia global en ambientes multi-región
        /// - MILLISECOND PRECISION: Precisión para análisis detallado de performance
        /// - ISO 8601 FORMAT: Estándar internacional para interoperabilidad
        /// - BUSINESS CALENDAR: Consideración de calendarios corporativos
        /// 
        /// ANÁLISIS EMPRESARIAL:
        /// - PEAK HOURS: Identificación de horas pico de procesamiento
        /// - SEASONAL PATTERNS: Análisis de patrones estacionales de datos
        /// - BUSINESS CYCLES: Correlación con ciclos de negocio (fin de mes, quarter)
        /// - OPTIMIZATION OPPORTUNITIES: Identificación de ventanas de optimización
        /// 
        /// ALERTAS Y MONITOREO:
        /// - LATE START: Alertas por procesos que inician tarde
        /// - SCHEDULING CONFLICTS: Detección de conflictos de programación
        /// - RESOURCE CONTENTION: Identificación de competencia por recursos
        /// - BUSINESS IMPACT: Alertas por procesamiento en horarios críticos
        /// </summary>
        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// TIMESTAMP DE FINALIZACIÓN DE PROCESAMIENTO
        /// ========================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Fecha y hora exacta de finalización del proceso de ingesta (exitoso o fallido).
        /// Puede ser null si el proceso está en ejecución. Es crítico para cálculo
        /// de duración, análisis de performance, detección de procesos colgados
        /// y verificación de completitud de procesos críticos de negocio.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - DURATION CALCULATION: Cálculo exacto de duración de procesamiento
        /// - HUNG PROCESS DETECTION: Detección de procesos que no terminan
        /// - SLA VERIFICATION: Verificación de cumplimiento de tiempos de SLA
        /// - THROUGHPUT ANALYSIS: Análisis de throughput y capacidad de procesamiento
        /// - BUSINESS WINDOW COMPLIANCE: Verificación de finalización en ventanas permitidas
        /// - DEPENDENCY MANAGEMENT: Coordinación de procesos dependientes
        /// - RESOURCE RELEASE: Liberación de recursos tras finalización
        /// - COMPLETION ALERTS: Alertas de finalización para procesos críticos
        /// 
        /// ESTADOS DE FINALIZACIÓN:
        /// - NULL: Proceso en ejecución, no ha finalizado
        /// - TIMESTAMP: Proceso finalizado (exitoso, fallido, o cancelado)
        /// - EXPECTED TIME: Comparación con tiempos esperados de finalización
        /// 
        /// ANÁLISIS DE PERFORMANCE:
        /// - DURATION TRENDS: Tendencias de duración a lo largo del tiempo
        /// - BOTTLENECK IDENTIFICATION: Identificación de cuellos de botella
        /// - EFFICIENCY METRICS: Métricas de eficiencia de procesamiento
        /// - OPTIMIZATION IMPACT: Medición de impacto de optimizaciones
        /// 
        /// BUSINESS CONTINUITY:
        /// - PROCESSING WINDOWS: Verificación de ventanas de procesamiento
        /// - DOWNSTREAM DEPENDENCIES: Coordinación con procesos dependientes
        /// - BUSINESS DEADLINES: Cumplimiento de deadlines empresariales
        /// - CONTINGENCY PLANNING: Planificación de contingencias por retrasos
        /// </summary>
        public DateTime? FechaFin { get; set; }

        // ========== ESTADO Y CONTROL DE EJECUCIÓN ==========
        /// <summary>
        /// ESTADO EMPRESARIAL DEL PROCESO ETL
        /// =================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Estado actual del proceso de ingesta que indica la situación operacional
        /// y el impacto potencial en operaciones de negocio. Es fundamental para
        /// monitoreo en tiempo real, alertas automáticas, escalación de incidentes
        /// y toma de decisiones operacionales críticas.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - REAL-TIME MONITORING: Monitoreo en tiempo real del estado de procesos críticos
        /// - AUTOMATED ALERTING: Generación automática de alertas según estado
        /// - ESCALATION MANAGEMENT: Escalación automática de incidentes críticos
        /// - DASHBOARD VISUALIZATION: Visualización del estado en dashboards ejecutivos
        /// - BUSINESS IMPACT ASSESSMENT: Evaluación de impacto al negocio por estado
        /// - DEPENDENCY COORDINATION: Coordinación de procesos dependientes
        /// - RESOURCE ALLOCATION: Asignación de recursos según estado de procesos
        /// - STAKEHOLDER COMMUNICATION: Comunicación del estado a stakeholders
        /// 
        /// ESTADOS EMPRESARIALES DEFINIDOS:
        /// 
        /// - **INICIADO** (Estado Operacional)
        ///   * Proceso ETL iniciado exitosamente con recursos asignados
        ///   * Impacto: Proceso activo, recursos en uso, monitoreo requerido
        ///   * Acciones: Monitoreo continuo, verificación de progreso
        ///   * SLA: Dentro de ventana esperada de inicio
        /// 
        /// - **EN_PROGRESO** (Estado Activo)
        ///   * Procesamiento activo de datos, transformaciones en ejecución
        ///   * Impacto: Recursos críticos en uso, datos siendo procesados
        ///   * Acciones: Monitoreo de performance, verificación de memoria/CPU
        ///   * SLA: Progreso dentro de ventana de tiempo esperada
        /// 
        /// - **COMPLETADO** (Estado Exitoso)
        ///   * Proceso finalizado exitosamente, datos disponibles para uso
        ///   * Impacto: Datos actualizados disponibles para reportes/análisis
        ///   * Acciones: Notificación a usuarios, activación de procesos dependientes
        ///   * SLA: Cumplimiento de tiempo de procesamiento acordado
        /// 
        /// - **FALLIDO** (Estado Crítico)
        ///   * Error crítico que impide completar el procesamiento
        ///   * Impacto: Datos obsoletos, reportes afectados, decisiones comprometidas
        ///   * Acciones: Alerta inmediata, troubleshooting, escalación
        ///   * SLA: Violación de SLA, impacto en disponibilidad de datos
        /// 
        /// - **ADVERTENCIA** (Estado de Atención)
        ///   * Proceso completado con issues que requieren revisión
        ///   * Impacto: Datos disponibles pero con calidad potencialmente comprometida
        ///   * Acciones: Revisión de calidad, validación de datos, notificación
        ///   * SLA: Cumplimiento parcial, requiere validación adicional
        /// 
        /// - **CANCELADO** (Estado Manual)
        ///   * Proceso cancelado manualmente por operaciones o mantenimiento
        ///   * Impacto: Datos no actualizados según programación normal
        ///   * Acciones: Reprogramación, comunicación a stakeholders
        ///   * SLA: Excepción planificada o no planificada según contexto
        /// 
        /// - **TIMEOUT** (Estado de Falla Temporal)
        ///   * Proceso excedió tiempo límite configurado sin completar
        ///   * Impacto: Recursos potencialmente bloqueados, datos no actualizados
        ///   * Acciones: Terminación forzada, diagnóstico de performance
        ///   * SLA: Violación de SLA de tiempo de procesamiento
        /// 
        /// - **REINTENTANDO** (Estado de Recuperación)
        ///   * Reintento automático tras fallo temporal o error recuperable
        ///   * Impacto: Procesamiento retrasado pero en recuperación automática
        ///   * Acciones: Monitoreo de reintentos, escalación si falla múltiples veces
        ///   * SLA: Potencial cumplimiento si reintento es exitoso
        /// 
        /// CLASIFICACIÓN POR CRITICIDAD:
        /// - **CRÍTICO**: FALLIDO, TIMEOUT (requiere intervención inmediata)
        /// - **ADVERTENCIA**: ADVERTENCIA, REINTENTANDO (requiere atención)
        /// - **OPERACIONAL**: INICIADO, EN_PROGRESO (operación normal)
        /// - **EXITOSO**: COMPLETADO (operación exitosa)
        /// - **MANUAL**: CANCELADO (intervención manual)
        /// 
        /// INTEGRACIÓN CON ALERTAS:
        /// - Estados críticos generan alertas inmediatas a equipos de soporte
        /// - Estados de advertencia generan notificaciones para revisión
        /// - Estados exitosos confirman operación normal a stakeholders
        /// - Estados manuales requieren comunicación con equipos operacionales
        /// </summary>
        public string Estado { get; set; } = string.Empty;

        // ========== INFORMACIÓN DE ERRORES Y DIAGNÓSTICO ==========
        /// <summary>
        /// MENSAJE DE ERROR EMPRESARIAL
        /// ===========================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Descripción detallada de errores, warnings o información diagnóstica
        /// crítica para troubleshooting, análisis de root cause, resolución de
        /// incidentes y mejora continua de procesos ETL empresariales.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - ROOT CAUSE ANALYSIS: Identificación de causa raíz de fallos críticos
        /// - TROUBLESHOOTING: Información detallada para diagnóstico técnico
        /// - INCIDENT MANAGEMENT: Descripción de incidentes para gestión
        /// - KNOWLEDGE BASE: Construcción de base de conocimiento de errores
        /// - AUTOMATED REMEDIATION: Información para remediation automática
        /// - STAKEHOLDER COMMUNICATION: Comunicación clara de problemas
        /// - PERFORMANCE OPTIMIZATION: Identificación de oportunidades de mejora
        /// - COMPLIANCE REPORTING: Documentación de errores para auditorías
        /// 
        /// TIPOS DE MENSAJES EMPRESARIALES:
        /// 
        /// - **ERRORES CRÍTICOS DE CONECTIVIDAD**
        ///   * "Error de conexión a SAP ERP: Timeout después de 30 segundos"
        ///   * "Autenticación fallida en Salesforce CRM: Token expirado"
        ///   * "Base de datos Oracle no disponible: Conexión rechazada"
        ///   * Impacto: Interrupción completa de procesamiento de datos críticos
        /// 
        /// - **ERRORES DE CALIDAD DE DATOS**
        ///   * "Datos duplicados detectados en Customer Master: 1,245 registros"
        ///   * "Valores nulos en campos obligatorios: CustomerID, Amount"
        ///   * "Formato de fecha inválido en 15% de registros de transacciones"
        ///   * Impacto: Compromete integridad y confiabilidad de análisis
        /// 
        /// - **ERRORES DE CONFIGURACIÓN**
        ///   * "Configuración de fuente de datos inválida: Missing API endpoint"
        ///   * "Credenciales de acceso expiradas para Azure Data Lake"
        ///   * "Esquema de datos modificado en fuente sin actualizar mapping"
        ///   * Impacto: Requiere intervención manual para corrección
        /// 
        /// - **ERRORES DE RECURSOS Y PERFORMANCE**
        ///   * "Memoria insuficiente para procesar dataset de 50M registros"
        ///   * "Timeout de procesamiento: 6 horas exceden límite de 4 horas"
        ///   * "Espacio en disco insuficiente para archivos temporales"
        ///   * Impacto: Degradación de performance y potencial fallo de sistema
        /// 
        /// - **ADVERTENCIAS DE NEGOCIO**
        ///   * "Volumen de datos 40% menor que promedio histórico"
        ///   * "Datos de fin de semana procesados en día laboral"
        ///   * "Fuente de datos no actualizada en últimas 24 horas"
        ///   * Impacto: Potencial impacto en calidad de reportes empresariales
        /// 
        /// ESTRUCTURA DE MENSAJES EMPRESARIALES:
        /// - **CONTEXTO**: Qué proceso o componente falló
        /// - **SÍNTOMA**: Qué error específico ocurrió
        /// - **CAUSA**: Por qué ocurrió el error (si se conoce)
        /// - **IMPACTO**: Qué sistemas o procesos se ven afectados
        /// - **ACCIÓN**: Qué pasos se recomiendan para resolución
        /// 
        /// CLASIFICACIÓN DE SEVERIDAD:
        /// - **CRÍTICO**: Errores que detienen procesamiento de datos críticos
        /// - **ALTO**: Errores que afectan calidad o completitud de datos
        /// - **MEDIO**: Warnings que pueden escalarse a errores
        /// - **BAJO**: Información que ayuda a optimización
        /// - **INFO**: Mensajes informativos para tracking normal
        /// 
        /// INTEGRACIÓN CON SISTEMAS EMPRESARIALES:
        /// - Mensajes formateados para integración con ITSM (ServiceNow, Remedy)
        /// - Clasificación automática para routing a equipos especializados
        /// - Integración con sistemas de monitoreo (Splunk, Datadog)
        /// - Generación automática de tickets de soporte según severidad
        /// - Alimentación de dashboards ejecutivos con métricas de calidad
        /// </summary>
        public string MensajeError { get; set; } = string.Empty;

        // ========== MÉTRICAS DE PROCESAMIENTO ==========
        /// <summary>
        /// CANTIDAD DE REGISTROS PROCESADOS
        /// ===============================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Número total de registros procesados exitosamente durante la ejecución
        /// del pipeline ETL. Es una métrica crítica para validación de completitud,
        /// análisis de volumetría, planificación de capacidad y verificación de
        /// integridad de procesos de datos empresariales.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - DATA COMPLETENESS: Verificación de completitud de datos procesados
        /// - VOLUME ANALYSIS: Análisis de tendencias de volumen de datos
        /// - CAPACITY PLANNING: Planificación de infraestructura basada en volúmenes
        /// - PERFORMANCE BENCHMARKING: Benchmarking de performance por volumen
        /// - SLA COMPLIANCE: Verificación de cumplimiento de SLAs de throughput
        /// - ANOMALY DETECTION: Detección de anomalías en volúmenes de datos
        /// - BUSINESS VALIDATION: Validación de volúmenes esperados vs reales
        /// - COST ALLOCATION: Asignación de costos basada en volumen procesado
        /// 
        /// ANÁLISIS EMPRESARIAL POR VOLUMEN:
        /// 
        /// - **MICRO BATCH** (1-1,000 registros)
        ///   * Procesos de alta frecuencia, datos en tiempo real
        ///   * APIs transaccionales, eventos de usuario, alerts
        ///   * SLA: < 1 minuto, alta disponibilidad requerida
        ///   * Impacto: Crítico para operaciones en tiempo real
        /// 
        /// - **SMALL BATCH** (1,001-10,000 registros)
        ///   * Procesos regulares de sincronización de datos
        ///   * Updates incrementales de CRM, catálogos de productos
        ///   * SLA: < 15 minutos, disponibilidad durante horas de negocio
        ///   * Impacto: Importante para operaciones diarias
        /// 
        /// - **MEDIUM BATCH** (10,001-100,000 registros)
        ///   * Procesos de sincronización de sistemas empresariales
        ///   * Datos de ventas diarias, inventario, customer updates
        ///   * SLA: < 1 hora, ventanas de procesamiento planificadas
        ///   * Impacto: Crítico para reportes diarios y análisis
        /// 
        /// - **LARGE BATCH** (100,001-1,000,000 registros)
        ///   * Procesos de consolidación mensual o semanal
        ///   * Históricos de transacciones, reportes consolidados
        ///   * SLA: < 4 horas, ventanas de mantenimiento
        ///   * Impacto: Crítico para reportes ejecutivos y compliance
        /// 
        /// - **BULK PROCESSING** (1,000,000+ registros)
        ///   * Migraciones de datos, procesos de data warehouse
        ///   * Análisis histórico, machine learning datasets
        ///   * SLA: < 24 horas, ventanas de procesamiento extendidas
        ///   * Impacto: Crítico para business intelligence y analytics
        /// 
        /// VALIDACIONES DE NEGOCIO:
        /// - **EXPECTED VOLUME**: Comparación con volúmenes históricos esperados
        /// - **BUSINESS RULES**: Validación contra reglas de negocio específicas
        /// - **SEASONAL PATTERNS**: Consideración de patrones estacionales
        /// - **DATA SOURCE HEALTH**: Indicador de salud de fuentes de datos
        /// - **DOWNSTREAM IMPACT**: Impacto en procesos y reportes dependientes
        /// 
        /// ALERTAS BASADAS EN VOLUMEN:
        /// - **ZERO RECORDS**: Alerta crítica por procesamiento sin datos
        /// - **LOW VOLUME**: Advertencia por volumen menor al 50% del esperado
        /// - **HIGH VOLUME**: Advertencia por volumen mayor al 200% del esperado
        /// - **VOLUME SPIKE**: Alerta por incremento súbito > 500% del promedio
        /// - **VOLUME DROP**: Alerta por caída súbita < 25% del promedio
        /// 
        /// MÉTRICAS DE PERFORMANCE:
        /// - **THROUGHPUT**: Registros por segundo procesados
        /// - **EFFICIENCY**: Ratio de registros exitosos vs procesados
        /// - **SCALABILITY**: Capacidad de manejar incrementos de volumen
        /// - **RESOURCE UTILIZATION**: Uso de recursos por registro procesado
        /// 
        /// REPORTING EMPRESARIAL:
        /// - **DAILY SUMMARY**: Resumen diario de volúmenes por fuente
        /// - **TREND ANALYSIS**: Análisis de tendencias de volumen mensual
        /// - **CAPACITY REPORTS**: Reportes de capacidad y utilización
        /// - **BUSINESS IMPACT**: Impacto de volúmenes en KPIs de negocio
        /// </summary>
        public int RegistrosProcesados { get; set; }

        // ========== INFORMACIÓN CONTEXTUAL ==========
        /// <summary>
        /// NOMBRE DESCRIPTIVO DE LA FUENTE DE DATOS
        /// =======================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Nombre descriptivo y user-friendly de la fuente de datos para
        /// identificación clara en reportes, dashboards, alertas y comunicaciones
        /// empresariales. Facilita la comprensión del contexto de negocio y
        /// la comunicación efectiva con stakeholders no técnicos.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// - USER INTERFACES: Identificación clara en dashboards y reportes
        /// - BUSINESS COMMUNICATION: Comunicación con stakeholders de negocio
        /// - ALERT NOTIFICATIONS: Descripción clara en alertas y notificaciones
        /// - EXECUTIVE REPORTING: Reportes ejecutivos con contexto de negocio
        /// - INCIDENT MANAGEMENT: Comunicación clara durante incidentes
        /// - KNOWLEDGE MANAGEMENT: Documentación user-friendly de fuentes
        /// - TRAINING MATERIALS: Material de entrenamiento para usuarios finales
        /// - STAKEHOLDER UPDATES: Updates claros sobre estado de fuentes críticas
        /// 
        /// EJEMPLOS DE NOMBRES EMPRESARIALES:
        /// 
        /// - **SISTEMAS ERP CORPORATIVOS**
        ///   * "SAP Production - Financial Modules"
        ///   * "Oracle EBS - Human Resources"
        ///   * "Microsoft Dynamics - Supply Chain"
        ///   * "NetSuite - Multi-Company Consolidation"
        /// 
        /// - **PLATAFORMAS CRM EMPRESARIALES**
        ///   * "Salesforce Sales Cloud - Americas"
        ///   * "HubSpot Marketing Hub - Lead Generation"
        ///   * "Microsoft Dynamics CRM - Customer Service"
        ///   * "Pipedrive - Sales Pipeline Management"
        /// 
        /// - **SISTEMAS FINANCIEROS**
        ///   * "QuickBooks Enterprise - Accounting"
        ///   * "Sage Intacct - Financial Reporting"
        ///   * "Oracle Hyperion - Budgeting & Planning"
        ///   * "BlackLine - Account Reconciliation"
        /// 
        /// - **PLATAFORMAS E-COMMERCE**
        ///   * "Shopify Plus - Online Store"
        ///   * "Magento Commerce - B2B Portal"
        ///   * "WooCommerce - Retail Website"
        ///   * "Amazon Seller Central - Marketplace"
        /// 
        /// - **HERRAMIENTAS DE MARKETING**
        ///   * "Google Analytics 4 - Website Traffic"
        ///   * "Adobe Analytics - Customer Journey"
        ///   * "Marketo Engage - Marketing Automation"
        ///   * "Pardot - B2B Lead Nurturing"
        /// 
        /// - **ALMACENAMIENTO CLOUD**
        ///   * "AWS S3 - Data Lake Storage"
        ///   * "Azure Blob Storage - Archive Files"
        ///   * "Google Cloud Storage - Backup Repository"
        ///   * "Snowflake - Data Warehouse"
        /// 
        /// CONVENCIONES DE NAMING EMPRESARIAL:
        /// - **SYSTEM NAME**: Nombre del sistema principal (SAP, Salesforce)
        /// - **MODULE/COMPONENT**: Módulo o componente específico (Finance, Sales)
        /// - **ENVIRONMENT**: Ambiente cuando sea relevante (Production, Staging)
        /// - **GEOGRAPHY**: Región geográfica cuando aplique (Americas, EMEA, APAC)
        /// - **BUSINESS UNIT**: Unidad de negocio cuando sea específica
        /// - **FUNCTION**: Función principal del sistema (Accounting, CRM, Analytics)
        /// 
        /// CLASIFICACIÓN POR CRITICIDAD:
        /// - **TIER 1 - CRITICAL**: Sistemas que impactan operaciones críticas
        /// - **TIER 2 - IMPORTANT**: Sistemas importantes para operaciones diarias
        /// - **TIER 3 - STANDARD**: Sistemas de soporte y análisis
        /// - **TIER 4 - DEVELOPMENT**: Sistemas de desarrollo y testing
        /// 
        /// INTEGRACIÓN CON COMUNICACIONES:
        /// - **ALERT MESSAGES**: "Fallo en procesamiento de [NombreFuente]"
        /// - **STATUS REPORTS**: "Estado de sincronización: [NombreFuente]"
        /// - **EXECUTIVE DASHBOARDS**: Visualización clara por nombre de sistema
        /// - **INCIDENT TICKETS**: Identificación clara en tickets de soporte
        /// 
        /// GOVERNANCE Y COMPLIANCE:
        /// - **DATA CATALOG**: Registro en catálogo de datos empresarial
        /// - **LINEAGE TRACKING**: Seguimiento de linaje de datos por nombre
        /// - **ACCESS CONTROL**: Control de acceso basado en sistema/función
        /// - **AUDIT TRAIL**: Identificación clara en logs de auditoría
        /// 
        /// BUSINESS CONTEXT:
        /// - Facilita comprensión del impacto de negocio de cada fuente
        /// - Permite priorización basada en criticidad empresarial
        /// - Mejora comunicación entre equipos técnicos y de negocio
        /// - Soporta toma de decisiones informada sobre recursos e inversiones
        /// </summary>
        public string NombreFuente { get; set; } = string.Empty;

        // ========== METADATOS EXTENDIDOS FUTUROS ==========
        // TODO: Campos adicionales para funcionalidad empresarial avanzada:

        /// <summary>
        /// FUTURO: Clasificación de criticidad del proceso
        /// Valores: CRITICAL, HIGH, MEDIUM, LOW para priorización de alertas
        /// </summary>
        // public string CriticidadProceso { get; set; } = "MEDIUM";

        /// <summary>
        /// FUTURO: Tiempo de duración en milisegundos para análisis de performance
        /// Calculado automáticamente: FechaFin - FechaInicio
        /// </summary>
        // public long? DuracionMs { get; set; }

        /// <summary>
        /// FUTURO: Throughput de registros por segundo para métricas de eficiencia
        /// Calculado: RegistrosProcesados / (DuracionMs / 1000)
        /// </summary>
        // public decimal? ThroughputRegSeg { get; set; }

        /// <summary>
        /// FUTURO: Identificador de usuario que inició el proceso manualmente
        /// Para procesos manuales, identificación del usuario responsable
        /// </summary>
        // public int? IdUsuarioIniciador { get; set; }

        /// <summary>
        /// FUTURO: Información adicional en formato JSON para extensibilidad
        /// Metadatos específicos por tipo de fuente o proceso
        /// </summary>
        // public string MetadatosAdicionales { get; set; } = "{}";

        /// <summary>
        /// FUTURO: Hash de los datos procesados para verificación de integridad
        /// SHA-256 hash para detección de modificaciones no autorizadas
        /// </summary>
        // public string HashDatos { get; set; } = string.Empty;

        /// <summary>
        /// FUTURO: Código de error estandarizado para categorización automática
        /// Códigos empresariales estándar para integración con ITSM
        /// </summary>
        // public string CodigoError { get; set; } = string.Empty;

        /// <summary>
        /// FUTURO: Prioridad de procesamiento para scheduling inteligente
        /// Valores: URGENT, HIGH, NORMAL, LOW para optimización de recursos
        /// </summary>
        // public string PrioridadProcesamiento { get; set; } = "NORMAL";

        /// <summary>
        /// FUTURO: Ambiente de ejecución para separación de logs por ambiente
        /// Valores: PRODUCTION, STAGING, DEVELOPMENT, TESTING
        /// </summary>
        // public string Ambiente { get; set; } = "PRODUCTION";

        /// <summary>
        /// FUTURO: Versión del pipeline ETL para tracking de cambios
        /// Control de versiones de lógica de procesamiento
        /// </summary>
        // public string VersionPipeline { get; set; } = "1.0.0";
    }
} 