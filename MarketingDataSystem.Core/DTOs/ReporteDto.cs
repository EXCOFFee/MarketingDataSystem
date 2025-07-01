// ==================== DTO DE REPORTE EMPRESARIAL - BUSINESS INTELLIGENCE ====================
// PROPÃ“SITO: Representa metadatos y contexto de reportes empresariales crÃ­ticos
// CRITICIDAD: ALTA - Contiene informaciÃ³n de reportes que pueden incluir datos financieros
// COMPLIANCE: Sujeto a regulaciones de auditorÃ­a, retenciÃ³n de datos y trazabilidad
// SEGURIDAD: Información sensible que requiere control de acceso y audit trail
// ESCALABILIDAD: DiseÃ±ado para manejar miles de reportes diarios en ambiente empresarial

namespace MarketingDataSystem.Core.DTOs
{
    /// <summary>
    /// DTO DE REPORTE EMPRESARIAL PARA BUSINESS INTELLIGENCE
    /// ===================================================
    /// 
    /// PROPÃ“SITO EMPRESARIAL:
    /// Representa metadatos completos de reportes empresariales generados por el sistema,
    /// incluyendo informaciÃ³n crÃ­tica para auditorÃ­a, compliance, seguridad y gestiÃ³n
    /// de activos de informaciÃ³n. Es fundamental para trazabilidad de acceso a datos.
    /// 
    /// CASOS DE USO EMPRESARIALES CRÃTICOS:
    /// 
    /// 1. **REPORTES FINANCIEROS EJECUTIVOS**
    ///    - Reportes de ventas para análisis ejecutivo y toma de decisiones estratÃ©gicas
    ///    - Estados financieros y mÃ©tricas de performance para board de directores
    ///    - análisis de rentabilidad por producto/cliente para planificaciÃ³n comercial
    ///    - Reportes de presupuesto vs real para control financiero corporativo
    /// 
    /// 2. **AUDITORÃA Y COMPLIANCE REGULATORIO**
    ///    - Reportes de auditorÃ­a para cumplimiento SOX (Sarbanes-Oxley) en empresas públicas
    ///    - DocumentaciÃ³n de procesos para auditorÃ­as internas y externas
    ///    - Reportes de compliance para reguladores financieros (SEC, FINRA)
    ///    - Trazabilidad de acceso a informaciÃ³n sensible para marcos de seguridad
    /// 
    /// 3. **ANÃLISIS COMERCIAL Y MARKETING**
    ///    - Reportes de performance de campaÃ±as de marketing para ROI measurement
    ///    - análisis de segmentaciÃ³n de clientes para estrategias comerciales
    ///    - Reportes de efectividad de canales de venta para optimizaciÃ³n
    ///    - MÃ©tricas de customer lifetime value para estrategias de retenciÃ³n
    /// 
    /// 4. **OPERACIONES Y SUPPLY CHAIN**
    ///    - Reportes de inventario para gestiÃ³n de supply chain y logÃ­stica
    ///    - análisis de rotaciÃ³n de productos para optimizaciÃ³n de stock
    ///    - Reportes de performance de proveedores para gestiÃ³n de relaciones
    ///    - análisis de costos operativos para eficiencia empresarial
    /// 
    /// 5. **RECURSOS HUMANOS Y GESTIÃ“N**
    ///    - Reportes de performance de equipos para evaluaciones y bonificaciones
    ///    - análisis de productividad para optimizaciÃ³n de recursos humanos
    ///    - Reportes de compliance laboral para cumplimiento de regulaciones
    ///    - MÃ©tricas de satisfacciÃ³n y engagement para retenciÃ³n de talento
    /// 
    /// 6. **INTELIGENCIA DE NEGOCIO (BI)**
    ///    - Dashboards ejecutivos para monitoreo en tiempo real de KPIs crÃ­ticos
    ///    - Reportes de tendencias para forecasting y planificaciÃ³n estratÃ©gica
    ///    - análisis predictivo para identificaciÃ³n de oportunidades de negocio
    ///    - Reportes de benchmarking contra competidores e industria
    /// 
    /// 7. **GESTIÃ“N DE RIESGOS EMPRESARIALES**
    ///    - Reportes de riesgo crediticio para decisiones de financiamiento
    ///    - análisis de riesgo operacional para continuidad de negocio
    ///    - Reportes de riesgo de mercado para estrategias de pricing
    ///    - Monitoreo de riesgos regulatorios para compliance proactivo
    /// 
    /// 8. **CUSTOMER RELATIONSHIP MANAGEMENT**
    ///    - Reportes de satisfacciÃ³n de clientes para mejora de servicios
    ///    - análisis de churn prediction para estrategias de retenciÃ³n
    ///    - Reportes de cross-selling y up-selling para crecimiento de revenue
    ///    - MÃ©tricas de Net Promoter Score para brand management
    /// 
    /// TIPOS DE REPORTES SOPORTADOS:
    /// - **Excel (.xlsx)**: Reportes tabulares para análisis detallado y manipulaciÃ³n
    /// - **PDF**: Reportes formales para distribuciÃ³n ejecutiva y compliance
    /// - **CSV**: ExportaciÃ³n de datos para análisis en herramientas externas
    /// - **Power BI**: IntegraciÃ³n con herramientas de business intelligence
    /// - **Tableau**: Conectores para visualizaciÃ³n avanzada de datos
    /// - **JSON**: APIs para integraciÃ³n con sistemas externos y dashboards
    /// 
    /// CLASIFICACIÃ“N DE SEGURIDAD:
    /// - **PÚBLICO**: MÃ©tricas generales sin informaciÃ³n sensible
    /// - **INTERNO**: InformaciÃ³n operacional para empleados autorizados
    /// - **CONFIDENCIAL**: Datos financieros y estratÃ©gicos de la empresa
    /// - **RESTRINGIDO**: InformaciÃ³n altamente sensible con acceso limitado
    /// - **REGULADO**: Datos sujetos a regulaciones especÃ­ficas (SOX, GDPR)
    /// 
    /// COMPLIANCE Y REGULACIONES:
    /// - **SOX (Sarbanes-Oxley)**: Control de acceso a reportes financieros crÃ­ticos
    /// - **GDPR**: ProtecciÃ³n de datos personales en reportes de clientes
    /// - **CCPA**: Compliance con California Consumer Privacy Act
    /// - **HIPAA**: Si maneja datos de salud, protecciÃ³n especÃ­fica requerida
    /// - **PCI DSS**: Si incluye datos de tarjetas, cumplimiento de estándares
    /// - **ISO 27001**: GestiÃ³n de seguridad de informaciÃ³n empresarial
    /// 
    /// RETENCIÃ“N Y ARCHIVADO:
    /// - **Reportes Financieros**: 7 aÃ±os según regulaciones fiscales
    /// - **Reportes de AuditorÃ­a**: 10 aÃ±os para compliance regulatorio
    /// - **Reportes Operacionales**: 3 aÃ±os para análisis histÃ³rico
    /// - **Reportes de Marketing**: 2 aÃ±os para análisis de tendencias
    /// - **Reportes de RRHH**: según polÃ­ticas laborales locales
    /// 
    /// PERFORMANCE Y ESCALABILIDAD:
    /// - **GeneraciÃ³n AsÃ­ncrona**: Reportes grandes procesados en background
    /// - **Caching Inteligente**: Cache de reportes frecuentemente accedidos
    /// - **CompresiÃ³n**: Almacenamiento optimizado para reportes de gran tamaÃ±o
    /// - **CDN Distribution**: DistribuciÃ³n global para acceso rápido
    /// - **Parallel Processing**: GeneraciÃ³n paralela de reportes complejos
    /// 
    /// SEGURIDAD EMPRESARIAL:
    /// - **Access Control**: AutorizaciÃ³n granular por usuario y rol
    /// - **Encryption**: EncriptaciÃ³n en reposo y trÃ¡nsito para datos sensibles
    /// - **Audit Trail**: Registro completo de acceso y descarga de reportes
    /// - **Digital Signatures**: Firma digital para integridad de reportes crÃ­ticos
    /// - **Watermarking**: Marcas de agua para trazabilidad de documentos
    /// - **DLP (Data Loss Prevention)**: PrevenciÃ³n de fuga de informaciÃ³n
    /// 
    /// INTEGRACIÃ“N EMPRESARIAL:
    /// - **ERP Systems**: IntegraciÃ³n con SAP, Oracle, Microsoft Dynamics
    /// - **CRM Platforms**: ConexiÃ³n con Salesforce, HubSpot, Microsoft CRM
    /// - **BI Tools**: IntegraciÃ³n nativa con Power BI, Tableau, Qlik
    /// - **Cloud Storage**: Almacenamiento en AWS S3, Azure Blob, Google Cloud
    /// - **SharePoint**: DistribuciÃ³n atravÃ©s de portales corporativos
    /// - **Email Distribution**: EnvÃ­o automÃ¡tico de reportes programados
    /// 
    /// MONITOREO Y ALERTAS:
    /// - **Generation Monitoring**: Monitoreo de Ã©xito/fallo en generaciÃ³n
    /// - **Access Monitoring**: Alertas por acceso anÃ³malo a reportes sensibles
    /// - **Size Alerts**: Alertas por reportes inusualmente grandes o pequeÃ±os
    /// - **Performance Tracking**: MÃ©tricas de tiempo de generaciÃ³n y descarga
    /// - **Usage Analytics**: análisis de patrones de uso para optimizaciÃ³n
    /// 
    /// DISASTER RECOVERY:
    /// - **Backup Strategy**: Respaldo automÃ¡tico de reportes crÃ­ticos
    /// - **Replication**: ReplicaciÃ³n geogrÃ¡fica para continuidad de negocio
    /// - **Recovery Procedures**: Procedimientos de recuperaciÃ³n de reportes
    /// - **Archive Management**: GestiÃ³n de archivos histÃ³ricos para compliance
    /// </summary>
    public class ReporteDto
    {
        // ========== IDENTIFICADOR ÃšNICO EMPRESARIAL ==========
        /// <summary>
        /// IDENTIFICADOR ÃšNICO DEL REPORTE
        /// ==============================
        /// 
        /// PROPÃ“SITO EMPRESARIAL:
        /// Identificador Ãºnico e inmutable que representa este reporte especÃ­fico en todo
        /// el ecosistema de business intelligence empresarial. Es la clave principal para
        /// trazabilidad, auditorÃ­a, acceso controlado y gestiÃ³n del ciclo de vida.
        /// 
        /// CASOS DE USO ESPECÃFICOS:
        /// - AUDIT TRAIL: IdentificaciÃ³n Ãºnica en logs de auditorÃ­a para compliance
        /// - ACCESS CONTROL: Control granular de permisos por reporte especÃ­fico
        /// - CACHE MANAGEMENT: Clave para cache inteligente de reportes frecuentes
        /// - API INTEGRATION: Identificador para APIs de distribuciÃ³n de reportes
        /// - WORKFLOW AUTOMATION: Referencia en flujos de trabajo automatizados
        /// - COMPLIANCE REPORTING: IdentificaciÃ³n en reportes regulatorios de acceso
        /// - DISASTER RECOVERY: Referencia para procedimientos de backup y restore
        /// - ANALYTICS: MÃ©tricas de uso y performance por reporte especÃ­fico
        /// 
        /// CONSIDERACIONES TÃ‰CNICAS:
        /// - IMMUTABLE: Una vez asignado, permanece constante durante todo el ciclo
        /// - UNIQUE: Garantiza unicidad absoluta en el sistema empresarial
        /// - INDEXED: Ãndice primario para mÃ¡ximo performance en consultas
        /// - SEQUENTIAL: Facilita identificaciÃ³n de orden temporal de generaciÃ³n
        /// 
        /// COMPLIANCE:
        /// - SOX: Usado para trazabilidad en auditorÃ­as de control interno
        /// - AUDIT TRAILS: Referencia permanente en logs de compliance regulatorio  
        /// - FORENSICS: IdentificaciÃ³n Ãºnica durante investigaciones de seguridad
        /// </summary>
        public int IdReporte { get; set; }
        /// <summary>
        /// Alias para IdReporte - Compatibilidad con tests
        /// </summary>
        public int Id 
        { 
            get => IdReporte; 
            set => IdReporte = value; 
        }

        // ========== METADATOS DE IDENTIFICACIÃ“N ==========
        /// <summary>
        /// NOMBRE EMPRESARIAL DEL REPORTE
        /// ==============================
        /// 
        /// PROPÃ“SITO EMPRESARIAL:
        /// Nombre descriptivo y profesional del reporte para identificaciÃ³n clara
        /// en interfaces empresariales, catÃ¡logos de reportes, distribuciÃ³n ejecutiva
        /// y documentaciÃ³n de compliance. Debe seguir convenciones corporativas.
        /// 
        /// CASOS DE USO ESPECÃFICOS:
        /// - EXECUTIVE DASHBOARDS: IdentificaciÃ³n clara en dashboards ejecutivos
        /// - REPORT CATALOG: BÃºsqueda y navegaciÃ³n en catÃ¡logo empresarial
        /// - EMAIL DISTRIBUTION: Subject line en distribuciÃ³n automÃ¡tica de reportes
        /// - COMPLIANCE DOCS: Referencia en documentos de auditorÃ­a y compliance
        /// - USER INTERFACES: IdentificaciÃ³n user-friendly en interfaces del sistema
        /// - API RESPONSES: Nombre descriptivo en respuestas de APIs corporativas
        /// - AUDIT LOGS: IdentificaciÃ³n humana en registros de auditorÃ­a
        /// - ARCHIVE MANAGEMENT: OrganizaciÃ³n de reportes histÃ³ricos por nombre
        /// 
        /// CONVENCIONES EMPRESARIALES:
        /// - DESCRIPTIVE: Debe describir claramente el contenido y propÃ³sito
        /// - PROFESSIONAL: Usar terminologÃ­a profesional y corporativa apropiada
        /// - CONSISTENT: Seguir patrones de naming establecidos por la empresa
        /// - SEARCHABLE: Incluir palabras clave relevantes para bÃºsqueda
        /// - HIERARCHICAL: Prefijos para organizaciÃ³n (Finance_, Sales_, HR_)
        /// - VERSION-AWARE: Incluir indicadores de versiÃ³n cuando sea relevante
        /// 
        /// EJEMPLOS DE NOMBRES EMPRESARIALES:
        /// - "Finance_Monthly_P&L_Executive_Summary"
        /// - "Sales_Weekly_Performance_Dashboard"  
        /// - "HR_Quarterly_Headcount_Analysis"
        /// - "Marketing_Campaign_ROI_Analysis"
        /// - "Operations_Daily_KPI_Scorecard"
        /// - "Compliance_SOX_Control_Testing"
        /// 
        /// VALIDACIONES CRÃTICAS:
        /// - OBLIGATORIO: Requerido para identificaciÃ³n y organizaciÃ³n
        /// - LONGITUD: Limitado para compatibilidad con sistemas legacy
        /// - CARACTERES: Evitar caracteres especiales que causen problemas de filesystem
        /// - UNIQUENESS: Considerar unicidad dentro de categorÃ­as para organizaciÃ³n
        /// - LOCALIZATION: Soporte para mÃºltiples idiomas en empresas globales
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// DESCRIPCIÃ“N DETALLADA DEL REPORTE
        /// =================================
        /// 
        /// PROPÃ“SITO EMPRESARIAL:
        /// DescripciÃ³n completa y profesional del reporte que explica su propÃ³sito,
        /// contenido, audiencia objetivo y valor para el negocio. Es crÃ­tica para
        /// comprensiÃ³n ejecutiva, compliance y gestiÃ³n de conocimiento empresarial.
        /// 
        /// CASOS DE USO ESPECÃFICOS:
        /// - EXECUTIVE BRIEFING: ExplicaciÃ³n para ejecutivos sobre valor del reporte
        /// - USER TRAINING: DocumentaciÃ³n para entrenar usuarios en uso de reportes
        /// - COMPLIANCE DOCUMENTATION: DescripciÃ³n para auditorÃ­as y reguladores
        /// - CATALOG MANAGEMENT: InformaciÃ³n para gestiÃ³n de catÃ¡logo empresarial
        /// - API DOCUMENTATION: DescripciÃ³n para integraciones con sistemas externos
        /// - KNOWLEDGE MANAGEMENT: Base de conocimiento para equipos de BI
        /// - ONBOARDING: InformaciÃ³n para nuevos usuarios del sistema
        /// - GOVERNANCE: DocumentaciÃ³n para comitÃ©s de data governance
        /// 
        /// INFORMACIÃ“N RECOMENDADA:
        /// - **PropÃ³sito del Negocio**: Por quÃ© existe este reporte y quÃ© problema resuelve
        /// - **Audiencia Objetivo**: QuiÃ©n deberÃ­a usar este reporte (ejecutivos, managers, analistas)
        /// - **Contenido Principal**: QuÃ© datos y mÃ©tricas incluye el reporte
        /// - **Frecuencia de Uso**: CuÃ¡ndo y con quÃ© frecuencia debe revisarse
        /// - **Decisiones Soportadas**: QuÃ© decisiones empresariales facilita
        /// - **Fuentes de Datos**: De dÃ³nde provienen los datos del reporte
        /// - **Consideraciones**: Limitaciones, supuestos o interpretaciÃ³n especial
        /// - **AcciÃ³n Requerida**: QuÃ© acciones se esperan tras revisar el reporte
        /// 
        /// EJEMPLO DE DESCRIPCIÃ“N EMPRESARIAL:
        /// "Reporte ejecutivo mensual que analiza performance financiera vs presupuesto.
        /// Dirigido a CFO y directores financieros para toma de decisiones estratÃ©gicas.
        /// Incluye análisis de variaciones, tendencias y proyecciones actualizadas.
        /// Soporta decisiones de inversiÃ³n, control de costos y comunicaciÃ³n a board."
        /// 
        /// COMPLIANCE Y GOVERNANCE:
        /// - DATA LINEAGE: DescripciÃ³n de origen y transformaciÃ³n de datos
        /// - QUALITY METRICS: InformaciÃ³n sobre calidad y confiabilidad de datos
        /// - REFRESH SCHEDULE: Frecuencia de actualizaciÃ³n y horarios
        /// - BUSINESS RULES: Reglas de negocio aplicadas en el reporte
        /// - APPROVAL PROCESS: Proceso de aprobación y validación del reporte
        /// </summary>
        public string Descripcion { get; set; } = string.Empty;

        // ========== GESTIÃ“N DE ARCHIVOS EMPRESARIAL ==========
        /// <summary>
        /// NOMBRE DE ARCHIVO EMPRESARIAL
        /// ============================
        /// 
        /// PROPÃ“SITO EMPRESARIAL:
        /// Nombre tÃ©cnico del archivo fÃ­sico generado, siguiendo convenciones
        /// empresariales para organizaciÃ³n, versionado, compatibilidad y governance.
        /// Es crÃ­tico para sistemas de archivo, distribuciÃ³n y archivado corporativo.
        /// 
        /// CASOS DE USO ESPECÃFICOS:
        /// - FILE SYSTEM STORAGE: Almacenamiento organizado en sistemas corporativos
        /// - EMAIL DISTRIBUTION: Nombre de archivo en distribuciÃ³n automÃ¡tica
        /// - ARCHIVE MANAGEMENT: OrganizaciÃ³n de archivos histÃ³ricos por convenciones
        /// - API DOWNLOADS: Nombre de archivo en descargas via API empresarial
        /// - CLOUD STORAGE: Almacenamiento estructurado en AWS S3, Azure, Google Cloud
        /// - SHAREPOINT: OrganizaciÃ³n en bibliotecas de documentos corporativos
        /// - BACKUP SYSTEMS: IdentificaciÃ³n en sistemas de backup empresarial
        /// - COMPLIANCE: Naming para cumplimiento de polÃ­ticas de retenciÃ³n
        /// 
        /// CONVENCIONES EMPRESARIALES DE NAMING:
        /// - **Timestamp**: Incluir fecha/hora para versionado temporal
        /// - **Department**: Prefijo del departamento propietario (FIN_, SAL_, MKT_)
        /// - **Classification**: Indicador de clasificaciÃ³n de seguridad
        /// - **Format**: ExtensiÃ³n clara (.xlsx, .pdf, .csv)
        /// - **Version**: NÃºmero de versiÃ³n cuando sea aplicable
        /// - **Environment**: Indicador de ambiente (PROD_, TEST_, DEV_)
        /// 
        /// EJEMPLOS DE NAMING EMPRESARIAL:
        /// - "FIN_Monthly_P&L_2024-01-31_v1.2_CONFIDENTIAL.xlsx"
        /// - "SAL_Weekly_Dashboard_2024-W04_INTERNAL.pdf"
        /// - "MKT_Campaign_Analysis_2024-Q1_RESTRICTED.csv"
        /// - "OPS_Daily_KPI_20240131_143022_PUBLIC.json"
        /// 
        /// CONSIDERACIONES TÃ‰CNICAS:
        /// - FILE SYSTEM COMPATIBILITY: Evitar caracteres problemÃ¡ticos (<>:"/\\|?*)
        /// - LENGTH LIMITS: Respetar lÃ­mites de longitud de sistemas legacy
        /// - CASE SENSITIVITY: Considerar sistemas case-sensitive vs case-insensitive
        /// - UNICODE SUPPORT: Soporte para caracteres especiales según necesidades globales
        /// - AUTOMATION FRIENDLY: Nombres que faciliten procesamiento automatizado
        /// 
        /// SEGURIDAD Y COMPLIANCE:
        /// - CLASSIFICATION TAGS: Incluir indicadores de clasificaciÃ³n de seguridad
        /// - RETENTION CODES: CÃ³digos para polÃ­ticas de retenciÃ³n automÃ¡tica  
        /// - AUDIT TRAIL: Nombres que faciliten auditorÃ­a y trazabilidad
        /// - DLP RULES: Compatibilidad con reglas de Data Loss Prevention
        /// </summary>
        public string NombreArchivo { get; set; } = string.Empty;

        // ========== METADATOS TEMPORALES ==========
        /// <summary>
        /// TIMESTAMP DE GENERACIÃ“N EMPRESARIAL
        /// ==================================
        /// 
        /// PROPÃ“SITO EMPRESARIAL:
        /// Fecha y hora exacta de generaciÃ³n del reporte en UTC para consistencia
        /// global. Es fundamental para auditorÃ­a, compliance, versionado, trazabilidad
        /// y gestiÃ³n del ciclo de vida de informaciÃ³n empresarial.
        /// 
        /// CASOS DE USO ESPECÃFICOS:
        /// - AUDIT COMPLIANCE: Timestamp exacto para auditorÃ­as regulatorias
        /// - VERSION CONTROL: Control de versiones temporal de reportes
        /// - DATA FRESHNESS: Indicador de quÃ© tan actuales son los datos
        /// - SLA MONITORING: Monitoreo de SLAs de generaciÃ³n de reportes
        /// - CACHE INVALIDATION: DeterminaciÃ³n de validez de cache de reportes
        /// - RETENTION POLICIES: AplicaciÃ³n automÃ¡tica de polÃ­ticas de retenciÃ³n
        /// - INCIDENT INVESTIGATION: Timeline para investigaciÃ³n de incidentes
        /// - PERFORMANCE ANALYTICS: análisis de patrones temporales de generaciÃ³n
        /// 
        /// PRECISIÃ“N EMPRESARIAL:
        /// - UTC TIMEZONE: Almacenado en UTC para consistencia global
        /// - MILLISECOND PRECISION: PrecisiÃ³n de milisegundos para ordenamiento exacto
        /// - ISO 8601 FORMAT: Formato estÃ¡ndar internacional para interoperabilidad
        /// - BUSINESS HOURS AWARE: ConsideraciÃ³n de horarios de negocio para contexto
        /// 
        /// COMPLIANCE Y AUDITORÃA:
        /// - SOX REQUIREMENTS: Timestamp preciso para control interno de reportes financieros
        /// - REGULATORY REPORTING: Fechas exactas para reportes regulatorios
        /// - LEGAL DISCOVERY: Evidencia temporal para procesos legales
        /// - FORENSIC ANALYSIS: Timeline preciso para análisis forense
        /// - CHANGE TRACKING: Seguimiento de cambios temporales en reportes
        /// 
        /// OPERACIONES EMPRESARIALES:
        /// - SCHEDULED REPORTS: VerificaciÃ³n de ejecuciÃ³n de reportes programados
        /// - BUSINESS DEADLINES: Cumplimiento de deadlines empresariales crÃ­ticos
        /// - MONTH-END CLOSE: Timing crÃ­tico para procesos de cierre contable
        /// - QUARTERLY REPORTING: CoordinaciÃ³n de reportes trimestrales
        /// - REAL-TIME DASHBOARDS: Timestamp para dashboards en tiempo real
        /// 
        /// ALERTAS Y MONITOREO:
        /// - LATE REPORTS: Alertas por reportes que no se generan a tiempo
        /// - STALE DATA: DetecciÃ³n de datos obsoletos en reportes crÃ­ticos
        /// - GENERATION FAILURES: IdentificaciÃ³n de fallos en generaciÃ³n programada
        /// - PERFORMANCE DEGRADATION: DetecciÃ³n de degradaciÃ³n en tiempos de generaciÃ³n
        /// </summary>
        public DateTime FechaGeneracion { get; set; }

        // ========== UBICACIÃ“N Y ALMACENAMIENTO ==========
        /// <summary>
        /// RUTA DE ALMACENAMIENTO EMPRESARIAL
        /// =================================
        /// 
        /// PROPÃ“SITO EMPRESARIAL:
        /// UbicaciÃ³n fÃ­sica o lÃ³gica completa del archivo de reporte en la infraestructura
        /// empresarial. Es crÃ­tica para acceso, distribuciÃ³n, backup, archivado y
        /// compliance con polÃ­ticas de gestiÃ³n de informaciÃ³n corporativa.
        /// 
        /// CASOS DE USO ESPECÃFICOS:
        /// - FILE ACCESS: Acceso directo al archivo desde aplicaciones empresariales
        /// - BACKUP SYSTEMS: InclusiÃ³n en polÃ­ticas de backup corporativo
        /// - ARCHIVE MANAGEMENT: MigraciÃ³n automÃ¡tica a sistemas de archivo histÃ³rico
        /// - DISASTER RECOVERY: ReplicaciÃ³n y recovery de archivos crÃ­ticos
        /// - COMPLIANCE AUDITS: UbicaciÃ³n de archivos para auditorÃ­as regulatorias
        /// - CLOUD MIGRATION: MigraciÃ³n de archivos entre infraestructuras cloud
        /// - SECURITY SCANNING: Escaneo de seguridad en ubicaciones de archivos
        /// - CAPACITY PLANNING: Monitoreo de uso de almacenamiento por ubicaciÃ³n
        /// 
        /// TIPOS DE UBICACIONES EMPRESARIALES:
        /// - **Local Storage**: "/reports/finance/monthly/2024/01/"
        /// - **Network Shares**: "\\corporate-nas\reports\executive\"
        /// - **Cloud Storage**: "https://s3.amazonaws.com/corp-reports/sensitive/"
        /// - **SharePoint**: "https://company.sharepoint.com/sites/reports/finance/"
        /// - **Document Management**: DMS://corporate/reports/category/subcategory/
        /// - **Archive Systems**: "archive://longterm/reports/2024/finance/"
        /// 
        /// ORGANIZACIÃ“N JERÃRQUICA:
        /// - **Department Level**: OrganizaciÃ³n por departamento propietario
        /// - **Security Level**: SeparaciÃ³n por clasificaciÃ³n de seguridad
        /// - **Temporal**: OrganizaciÃ³n por aÃ±o/mes/dÃ­a para facilitar archivado
        /// - **Report Type**: AgrupaciÃ³n por tipo de reporte (financial, operational)
        /// - **Audience**: SeparaciÃ³n por audiencia (executive, management, operational)
        /// 
        /// CONSIDERACIONES DE SEGURIDAD:
        /// - ACCESS CONTROL: Control de acceso basado en ubicaciÃ³n del archivo
        /// - ENCRYPTION: EncriptaciÃ³n especÃ­fica según ubicaciÃ³n y clasificaciÃ³n
        /// - AUDIT TRAIL: Registro de accesos por ubicaciÃ³n de archivo
        /// - DLP POLICIES: AplicaciÃ³n de polÃ­ticas de prevenciÃ³n de pÃ©rdida de datos
        /// - GEOGRAPHIC RESTRICTIONS: Restricciones geogrÃ¡ficas según regulaciones
        /// 
        /// GESTIÃ“N DE INFRAESTRUCTURA:
        /// - LOAD BALANCING: DistribuciÃ³n de carga entre mÃºltiples ubicaciones
        /// - REDUNDANCY: ReplicaciÃ³n de archivos crÃ­ticos en mÃºltiples ubicaciones
        /// - PERFORMANCE: OptimizaciÃ³n de ubicaciones para acceso rápido
        /// - COST OPTIMIZATION: Ubicaciones cost-effective según frecuencia de acceso
        /// - COMPLIANCE: Ubicaciones que cumplen con regulaciones especÃ­ficas
        /// 
        /// AUTOMATIZACIÃ“N EMPRESARIAL:
        /// - LIFECYCLE MANAGEMENT: MigraciÃ³n automÃ¡tica entre tiers de almacenamiento
        /// - RETENTION POLICIES: AplicaciÃ³n automÃ¡tica de polÃ­ticas de retenciÃ³n
        /// - ARCHIVE TRIGGERS: Archivado automÃ¡tico basado en edad y acceso
        /// - CLEANUP PROCESSES: Limpieza automÃ¡tica de archivos expirados
        /// - MONITORING ALERTS: Alertas por problemas de acceso o almacenamiento
        /// </summary>
        public string RutaArchivo { get; set; } = string.Empty;

        // ========== METADATOS EXTENDIDOS FUTUROS ==========
        // TODO: Campos adicionales para funcionalidad empresarial avanzada:

        /// <summary>
        /// FUTURO: ClasificaciÃ³n de seguridad del reporte
        /// Valores: PUBLIC, INTERNAL, CONFIDENTIAL, RESTRICTED, TOP_SECRET
        /// </summary>
        // public string ClasificacionSeguridad { get; set; } = "INTERNAL";

        /// <summary>
        /// FUTURO: TamaÃ±o del archivo en bytes para gestiÃ³n de almacenamiento
        /// Usado para alertas, quotas y optimizaciÃ³n de infraestructura
        /// </summary>
        // public long TamaÃ±oArchivo { get; set; }

        /// <summary>
        /// FUTURO: Hash SHA-256 del archivo para verificaciÃ³n de integridad
        /// CrÃ­tico para compliance y detecciÃ³n de modificaciones no autorizadas
        /// </summary>
        // public string HashIntegridad { get; set; } = string.Empty;

        /// <summary>
        /// FUTURO: Usuario que generÃ³ el reporte para audit trail
        /// Referencia al ID del usuario para trazabilidad completa
        /// </summary>
        // public int IdUsuarioGenerador { get; set; }

        /// <summary>
        /// FUTURO: Fecha de expiraciÃ³n para aplicaciÃ³n automÃ¡tica de retenciÃ³n
        /// Facilita compliance con polÃ­ticas de retenciÃ³n empresarial
        /// </summary>
        // public DateTime? FechaExpiracion { get; set; }

        /// <summary>
        /// FUTURO: Contador de descargas para mÃ©tricas de uso
        /// Analytics para optimizaciÃ³n de catÃ¡logo de reportes
        /// </summary>
        // public int NumeroDescargas { get; set; }

        /// <summary>
        /// FUTURO: Tiempo de generaciÃ³n en milisegundos para performance monitoring
        /// SLA tracking y optimizaciÃ³n de procesos de generaciÃ³n
        /// </summary>
        // public int TiempoGeneracionMs { get; set; }

        /// <summary>
        /// FUTURO: Formato del archivo para procesamiento automÃ¡tico
        /// Valores: EXCEL, PDF, CSV, JSON, XML, POWERBI, TABLEAU
        /// </summary>
        // public string FormatoArchivo { get; set; } = "EXCEL";

        /// <summary>
        /// FUTURO: Firma digital para reportes crÃ­ticos
        /// Garantiza autenticidad e integridad de reportes financieros
        /// </summary>
        // public string FirmaDigital { get; set; } = string.Empty;

        /// <summary>
        /// FUTURO: Metadatos JSON adicionales para extensibilidad
        /// Permite agregar informaciÃ³n especÃ­fica sin cambios de schema
        /// </summary>
        // public string MetadatosAdicionales { get; set; } = "{}";
    }
} 



