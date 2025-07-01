// ==================== METADATA DTO EMPRESARIAL - DATA GOVERNANCE CORE ====================
// PROPÓSITO: Contenedor fundamental de metadatos para governance y audit trails empresariales
// CRITICIDAD: ALTA - Backbone de trazabilidad y compliance requerido por regulaciones
// COMPLIANCE: SOX, GDPR, ISO 27001 - Essential para audit trails y data lineage
// ARQUITECTURA: Core DTO que soporta data governance y enterprise metadata management
// VALOR: Habilita observabilidad completa, forensics y regulatory compliance

using System;
using System.ComponentModel.DataAnnotations;

namespace MarketingDataSystem.Core.DTOs
{
    /// <summary>
    /// METADATA DTO EMPRESARIAL - DATA GOVERNANCE Y AUDIT TRAIL FOUNDATION
    /// ==================================================================
    /// 
    /// PROPÓSITO EMPRESARIAL:
    /// Contenedor fundamental de metadatos que implementa la base de data governance
    /// empresarial, audit trails y compliance regulatory. Este DTO crítico captura
    /// información esencial sobre transformaciones, procesos y decisiones que permiten
    /// trazabilidad completa para auditorías financieras, compliance regulations
    /// y forensic analysis en entornos empresariales críticos.
    /// 
    /// CASOS DE USO EMPRESARIALES CRÍTICOS:
    /// 
    /// 1. **SOX COMPLIANCE Y AUDIT TRAILS FINANCIEROS**
    ///    - Trazabilidad completa de transformaciones de datos financieros críticos
    ///    - Audit trail de cambios en reportes financieros para external auditors
    ///    - Documentation de procesos ETL que alimentan financial statements
    ///    - Chain of custody para datos utilizados en quarterly earnings reports
    ///    - Compliance con Section 404 de SOX para internal controls documentation
    /// 
    /// 2. **GDPR Y DATA PRIVACY COMPLIANCE**
    ///    - Metadata tracking para Right to be Forgotten implementation
    ///    - Data lineage documentation para privacy impact assessments
    ///    - Processing activity records requeridos por GDPR Article 30
    ///    - Consent metadata tracking para lawful basis documentation
    ///    - Cross-border transfer metadata para adequacy decision compliance
    /// 
    /// 3. **DATA LINEAGE Y IMPACT ANALYSIS**
    ///    - Complete data lineage desde source systems hasta business reports
    ///    - Impact analysis para system changes y data model modifications
    ///    - Upstream/downstream dependency mapping para change management
    ///    - Data flow documentation para system integration y architecture
    ///    - Business process documentation mediante metadata relationships
    /// 
    /// 4. **OPERATIONAL INTELLIGENCE Y TROUBLESHOOTING**
    ///    - Root cause analysis para data quality issues y system failures
    ///    - Performance monitoring metadata para ETL process optimization
    ///    - Error tracking y resolution documentation para operational excellence
    ///    - Process timing metadata para SLA monitoring y capacity planning
    ///    - System health monitoring mediante metadata trend analysis
    /// 
    /// 5. **REGULATORY REPORTING Y COMPLIANCE**
    ///    - Metadata documentation para regulatory submissions (SEC, banking)
    ///    - Audit preparation materials con complete process documentation
    ///    - Control testing evidence para internal audit y risk management
    ///    - Data quality metrics documentation para regulatory compliance
    ///    - Change management documentation para regulatory change control
    /// 
    /// 6. **BUSINESS INTELLIGENCE Y DATA GOVERNANCE**
    ///    - Data quality metrics tracking para business intelligence accuracy
    ///    - Business glossary implementation mediante standardized metadata
    ///    - Data stewardship workflows con metadata-driven processes
    ///    - Master data management mediante authoritative metadata sources
    ///    - Data catalog population para self-service analytics enablement
    /// 
    /// 7. **SECURITY Y FORENSIC ANALYSIS**
    ///    - Security incident investigation mediante detailed audit trails
    ///    - Data breach impact assessment using comprehensive metadata
    ///    - Access pattern analysis para anomaly detection y threat hunting
    ///    - Compliance monitoring mediante automated metadata analysis
    ///    - Insider threat detection using metadata pattern recognition
    /// 
    /// 8. **ENTERPRISE ARCHITECTURE Y SYSTEM INTEGRATION**
    ///    - System integration documentation mediante metadata mapping
    ///    - API versioning y compatibility tracking through metadata
    ///    - Service dependency mapping para microservices architecture
    ///    - Data contract enforcement mediante metadata validation
    ///    - System retirement planning usando metadata dependency analysis
    /// 
    /// ARQUITECTURA DE METADATA EMPRESARIAL:
    /// 
    /// **METADATA CATEGORIES:**
    /// - **Technical Metadata**: System details, formats, schemas, processing info
    /// - **Business Metadata**: Business definitions, rules, ownership, usage
    /// - **Operational Metadata**: Processing times, volumes, errors, performance
    /// - **Governance Metadata**: Policies, compliance status, approval workflows
    /// - **Security Metadata**: Access controls, classification, encryption status
    /// 
    /// **METADATA LIFECYCLE:**
    /// ```
    /// CREATION → VALIDATION → ENRICHMENT → STORAGE → RETRIEVAL → ANALYSIS → ARCHIVAL
    /// ```
    /// 
    /// **GOVERNANCE INTEGRATION:**
    /// - **Data Stewardship**: Metadata ownership y quality management
    /// - **Policy Enforcement**: Automated compliance checking mediante metadata
    /// - **Access Control**: Role-based metadata visibility y data access
    /// - **Audit Compliance**: Automated audit trail generation y reporting
    /// - **Risk Management**: Metadata-driven risk assessment y monitoring
    /// 
    /// COMPLIANCE REQUIREMENTS POR REGULATION:
    /// 
    /// **SOX (Sarbanes-Oxley) REQUIREMENTS:**
    /// - Section 302: Executive certification de accuracy requires metadata trails
    /// - Section 404: Internal controls documentation mediante comprehensive metadata
    /// - Section 409: Real-time disclosure supported by metadata-driven reporting
    /// - Audit trail requirements para financial data transformations
    /// 
    /// **GDPR (General Data Protection Regulation) REQUIREMENTS:**
    /// - Article 5: Data minimization principle enforcement mediante metadata
    /// - Article 25: Privacy by design implementation through metadata controls
    /// - Article 30: Records of processing activities usando structured metadata
    /// - Article 35: Data protection impact assessments supported by metadata
    /// 
    /// **ISO 27001 INFORMATION SECURITY:**
    /// - Asset management mediante comprehensive metadata inventory
    /// - Risk assessment supported by metadata-driven analysis
    /// - Incident response facilitated by detailed metadata trails
    /// - Continuous monitoring through automated metadata collection
    /// 
    /// PERFORMANCE Y SCALABILITY ENTERPRISE:
    /// - **High-Volume Metadata**: Millions de metadata records per day
    /// - **Real-time Processing**: Sub-second metadata capture y retrieval
    /// - **Search Performance**: Indexed metadata para fast discovery y lineage
    /// - **Storage Optimization**: Efficient metadata storage y archival strategies
    /// - **Global Distribution**: Multi-region metadata replication y synchronization
    /// 
    /// INTEGRATION CON ENTERPRISE TOOLS:
    /// - **Data Catalogs**: Collibra, Apache Atlas, AWS Glue Data Catalog
    /// - **Governance Platforms**: Informatica, IBM InfoSphere, Talend
    /// - **Monitoring Tools**: Splunk, ELK Stack, Datadog, New Relic
    /// - **SIEM Systems**: QRadar, Splunk Security, ArcSight
    /// - **Audit Tools**: Audit Analytics, ACL, IDEA Data Analysis
    /// 
    /// SECURITY Y ACCESS CONTROL:
    /// - **Classification Levels**: Public, Internal, Confidential, Restricted
    /// - **Role-Based Access**: Metadata visibility based on user roles
    /// - **Encryption**: Sensitive metadata encrypted at rest y in transit
    /// - **Audit Logging**: All metadata access y modifications logged
    /// - **Data Loss Prevention**: Metadata-driven DLP policy enforcement
    /// 
    /// DISASTER RECOVERY Y BUSINESS CONTINUITY:
    /// - **Metadata Backup**: Automated backup de critical metadata
    /// - **Recovery Procedures**: Metadata restoration para business continuity
    /// - **Replication**: Real-time metadata replication across regions
    /// - **Integrity Verification**: Automated metadata consistency checking
    /// - **Recovery Time Objectives**: <1 hour para critical metadata recovery
    /// </summary>
    public class MetadataDto
    {
        /// <summary>
        /// IDENTIFICADOR ÚNICO DEL METADATA RECORD
        /// ====================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Unique identifier que permite tracking, referencing y linking de metadata
        /// records across enterprise systems. Essential para maintaining referential
        /// integrity en complex data lineage chains y audit trail correlation.
        /// 
        /// CASOS DE USO ESPECÍFICOS:
        /// 
        /// **AUDIT TRAIL CORRELATION:**
        /// ```csharp
        /// // ESCENARIO: SOX audit trail linking
        /// var auditTrail = metadataService.GetAuditChain(metadata.Id);
        /// var relatedTransformations = lineageService.TraceById(metadata.Id);
        /// var complianceReport = auditService.GenerateSOXReport(metadata.Id);
        /// ```
        /// 
        /// **DATA LINEAGE NAVIGATION:**
        /// ```csharp
        /// // ESCENARIO: Impact analysis para system changes
        /// var upstreamMetadata = lineageService.GetUpstreamById(metadata.Id);
        /// var downstreamImpact = impactService.AnalyzeDownstream(metadata.Id);
        /// var changeAssessment = riskService.AssessChange(metadata.Id);
        /// ```
        /// 
        /// **CROSS-SYSTEM CORRELATION:**
        /// ```csharp
        /// // ESCENARIO: Multi-system metadata correlation
        /// var crmMetadata = externalService.GetCRMMetadata(metadata.Id);
        /// var erpMetadata = externalService.GetERPMetadata(metadata.Id);
        /// var consolidatedView = metadataService.ConsolidateMetadata(metadata.Id);
        /// ```
        /// 
        /// TECHNICAL REQUIREMENTS:
        /// - **Uniqueness**: Globally unique across all enterprise systems
        /// - **Immutability**: Never changes once assigned para referential integrity
        /// - **Performance**: Indexed para fast lookup y correlation operations
        /// - **Scalability**: Supports millions de concurrent metadata records
        /// 
        /// COMPLIANCE CONSIDERATIONS:
        /// - **SOX**: Required para audit trail correlation y financial data tracking
        /// - **GDPR**: Essential para Right to be Forgotten implementation
        /// - **ISO 27001**: Critical para asset tracking y security management
        /// - **Audit Standards**: Supports external auditor requirements
        /// </summary>
        [Required(ErrorMessage = "Metadata ID es requerido para trazabilidad empresarial")]
        [Range(1, int.MaxValue, ErrorMessage = "Metadata ID debe ser un valor positivo válido")]
        public int Id { get; set; }
        
        /// <summary>
        /// TIPO DE METADATA PARA CLASIFICACIÓN EMPRESARIAL
        /// ============================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Classification system que categoriza metadata records para governance,
        /// compliance y operational management. Enables automated processing,
        /// routing y policy enforcement based on metadata type.
        /// 
        /// TIPOS EMPRESARIALES ESTÁNDAR:
        /// 
        /// **TECHNICAL METADATA TYPES:**
        /// - "ETL_TRANSFORMATION": Transformaciones de datos en pipeline ETL
        /// - "DATA_VALIDATION": Validaciones aplicadas durante processing
        /// - "FORMAT_CONVERSION": Conversiones de formato (JSON, CSV, XML)
        /// - "SCHEMA_EVOLUTION": Cambios en estructura de datos
        /// - "SYSTEM_INTEGRATION": Integraciones entre sistemas
        /// 
        /// **BUSINESS METADATA TYPES:**
        /// - "BUSINESS_RULE": Reglas de negocio aplicadas
        /// - "DATA_QUALITY": Métricas de calidad de datos
        /// - "MASTER_DATA": Información de master data management
        /// - "BUSINESS_GLOSSARY": Definiciones de términos de negocio
        /// - "OWNERSHIP": Información de data stewardship
        /// 
        /// **OPERATIONAL METADATA TYPES:**
        /// - "PERFORMANCE_METRIC": Métricas de performance del sistema
        /// - "ERROR_LOG": Registros de errores y exceptions
        /// - "CAPACITY_METRIC": Métricas de capacidad y utilización
        /// - "SLA_TRACKING": Tracking de service level agreements
        /// - "MONITORING_ALERT": Alertas del sistema de monitoreo
        /// 
        /// **GOVERNANCE METADATA TYPES:**
        /// - "COMPLIANCE_CHECK": Verificaciones de compliance
        /// - "POLICY_ENFORCEMENT": Aplicación de políticas de governance
        /// - "AUDIT_TRAIL": Registros para audit trails
        /// - "RISK_ASSESSMENT": Evaluaciones de riesgo
        /// - "APPROVAL_WORKFLOW": Workflows de aprobación
        /// 
        /// **SECURITY METADATA TYPES:**
        /// - "ACCESS_CONTROL": Information de control de acceso
        /// - "ENCRYPTION_STATUS": Estado de encriptación de datos
        /// - "CLASSIFICATION": Clasificación de seguridad de datos
        /// - "PRIVACY_IMPACT": Impact assessments de privacidad
        /// - "SECURITY_INCIDENT": Incidentes de seguridad
        /// 
        /// CASOS DE USO POR TIPO:
        /// 
        /// **ETL_TRANSFORMATION METADATA:**
        /// ```csharp
        /// var etlMetadata = new MetadataDto 
        /// {
        ///     Tipo = "ETL_TRANSFORMATION",
        ///     Descripcion = "Customer data normalization from CRM to warehouse",
        ///     Fecha = DateTime.UtcNow
        /// };
        /// // Used for: Data lineage, impact analysis, troubleshooting
        /// ```
        /// 
        /// **COMPLIANCE_CHECK METADATA:**
        /// ```csharp
        /// var complianceMetadata = new MetadataDto 
        /// {
        ///     Tipo = "COMPLIANCE_CHECK",
        ///     Descripcion = "GDPR consent verification for marketing data",
        ///     Fecha = DateTime.UtcNow
        /// };
        /// // Used for: Regulatory reporting, audit preparation
        /// ```
        /// 
        /// **BUSINESS_RULE METADATA:**
        /// ```csharp
        /// var businessRuleMetadata = new MetadataDto 
        /// {
        ///     Tipo = "BUSINESS_RULE",
        ///     Descripcion = "Revenue recognition rule applied to transaction",
        ///     Fecha = DateTime.UtcNow
        /// };
        /// // Used for: Business intelligence, financial reporting
        /// ```
        /// 
        /// AUTOMATION BENEFITS:
        /// - **Policy Routing**: Automated routing based on metadata type
        /// - **Compliance Monitoring**: Automated compliance checking
        /// - **Alert Generation**: Type-specific alerting y notifications
        /// - **Reporting**: Automated categorization para reports
        /// - **Governance**: Type-based governance policy enforcement
        /// </summary>
        [Required(ErrorMessage = "Tipo de metadata es requerido para clasificación empresarial")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tipo debe tener entre 3 y 100 caracteres")]
        public string Tipo { get; set; } = string.Empty;
        
        /// <summary>
        /// TIMESTAMP EMPRESARIAL PARA AUDIT TRAILS Y COMPLIANCE
        /// ================================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Precise timestamp que establece when metadata was created, providing
        /// temporal context essential para audit trails, compliance reporting
        /// y forensic analysis. Critical para maintaining chronological order
        /// en complex business processes y regulatory requirements.
        /// 
        /// CASOS DE USO EMPRESARIALES:
        /// 
        /// **SOX COMPLIANCE TIMING:**
        /// ```csharp
        /// // ESCENARIO: Financial period-end process tracking
        /// var quarterEndCutoff = new DateTime(2024, 3, 31, 23, 59, 59);
        /// if (metadata.Fecha <= quarterEndCutoff) 
        /// {
        ///     // Include in Q1 financial reporting
        ///     financialReportService.IncludeInQuarterlyReport(metadata);
        /// }
        /// ```
        /// 
        /// **GDPR DATA RETENTION:**
        /// ```csharp
        /// // ESCENARIO: Data retention policy enforcement
        /// var retentionPeriod = TimeSpan.FromDays(2555); // 7 years
        /// if (DateTime.UtcNow - metadata.Fecha > retentionPeriod) 
        /// {
        ///     // Data eligible for deletion under retention policy
        ///     retentionService.ScheduleForDeletion(metadata);
        /// }
        /// ```
        /// 
        /// **INCIDENT TIMELINE RECONSTRUCTION:**
        /// ```csharp
        /// // ESCENARIO: Security incident forensics
        /// var incidentTime = new DateTime(2024, 6, 15, 14, 30, 0);
        /// var relevantMetadata = metadataService.GetMetadataAroundTime(
        ///     incidentTime.AddHours(-2), 
        ///     incidentTime.AddHours(2)
        /// );
        /// var timeline = forensicsService.ReconstructTimeline(relevantMetadata);
        /// ```
        /// 
        /// **SLA COMPLIANCE MONITORING:**
        /// ```csharp
        /// // ESCENARIO: Process timing validation
        /// var slaDeadline = metadata.Fecha.AddHours(4); // 4-hour SLA
        /// if (DateTime.UtcNow > slaDeadline) 
        /// {
        ///     // SLA breach - trigger escalation
        ///     slaService.TriggerSLABreach(metadata);
        /// }
        /// ```
        /// 
        /// TEMPORAL ANALYTICS:
        /// - **Pattern Detection**: Identify timing patterns en business processes
        /// - **Performance Analysis**: Measure process duration y optimization opportunities
        /// - **Seasonality Analysis**: Detect seasonal patterns para capacity planning
        /// - **Trend Analysis**: Track metadata creation trends para business insights
        /// 
        /// COMPLIANCE REQUIREMENTS:
        /// - **SOX**: Accurate timing para financial process documentation
        /// - **GDPR**: Timestamp precision para data processing records
        /// - **ISO 27001**: Timing information para security incident response
        /// - **Audit Standards**: Chronological evidence para external auditors
        /// 
        /// TECHNICAL SPECIFICATIONS:
        /// - **Precision**: Millisecond precision para detailed audit trails
        /// - **Time Zone**: UTC standardization para global consistency
        /// - **Immutability**: Timestamp never changes after initial creation
        /// - **Indexing**: Optimized para time-based queries y reporting
        /// </summary>
        [Required(ErrorMessage = "Fecha de metadata es requerida para audit trails")]
        public DateTime Fecha { get; set; }
        
        /// <summary>
        /// DESCRIPCIÓN EMPRESARIAL PARA BUSINESS CONTEXT Y AUDIT CLARITY
        /// ==========================================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Human-readable description que provides business context y detailed
        /// information about the metadata record. Essential para audit trail
        /// clarity, compliance documentation y business user understanding
        /// of complex technical processes.
        /// 
        /// COMPONENTES DE DESCRIPCIÓN EMPRESARIAL:
        /// 
        /// **BUSINESS CONTEXT:**
        /// - What business process or operation the metadata represents
        /// - Why the metadata was created (business justification)
        /// - Which business stakeholder or department is responsible
        /// - How the metadata relates to business objectives
        /// 
        /// **TECHNICAL DETAILS:**
        /// - Technical process or transformation applied
        /// - Source y target systems involved
        /// - Data volumes or performance metrics
        /// - Error conditions or exceptional circumstances
        /// 
        /// **COMPLIANCE INFORMATION:**
        /// - Regulatory requirements that drive the metadata
        /// - Control objectives being satisfied
        /// - Risk mitigation measures implemented
        /// - Audit trail connections to other processes
        /// 
        /// EJEMPLOS EMPRESARIALES POR DOMAIN:
        /// 
        /// **FINANCIAL PROCESSING:**
        /// ```csharp
        /// var financialMetadata = new MetadataDto 
        /// {
        ///     Descripcion = "Revenue recognition transformation applied to Q1 sales data " +
        ///                  "per ASC 606 standard. Converted 15,234 transactions from " +
        ///                  "Salesforce CRM to financial reporting format. " +
        ///                  "Control: SOX-FIN-001. Approved by: CFO Office."
        /// };
        /// ```
        /// 
        /// **CUSTOMER DATA PROCESSING:**
        /// ```csharp
        /// var customerMetadata = new MetadataDto 
        /// {
        ///     Descripcion = "Customer PII anonymization applied for analytics dataset " +
        ///                  "per GDPR Article 6(1)(f) legitimate interest. " +
        ///                  "Masked 45,678 customer records while preserving " +
        ///                  "analytical value. DPO approval: DPO-2024-156."
        /// };
        /// ```
        /// 
        /// **OPERATIONAL MONITORING:**
        /// ```csharp
        /// var operationalMetadata = new MetadataDto 
        /// {
        ///     Descripcion = "ETL pipeline performance degradation detected. " +
        ///                  "Processing time increased 300% above baseline. " +
        ///                  "Root cause: Database index fragmentation. " +
        ///                  "Resolution: Index rebuild scheduled. SLA impact: None."
        /// };
        /// ```
        /// 
        /// **SECURITY INCIDENT:**
        /// ```csharp
        /// var securityMetadata = new MetadataDto 
        /// {
        ///     Descripcion = "Anomalous data access pattern detected for user ID 'jsmith'. " +
        ///                  "Accessed 500+ customer records outside normal hours. " +
        ///                  "SIEM alert triggered. Security team notified. " +
        ///                  "Investigation status: In progress. Incident: INC-2024-789."
        /// };
        /// ```
        /// 
        /// BEST PRACTICES PARA DESCRIPCIONES:
        /// 
        /// **CLARITY Y PRECISION:**
        /// - Use clear, non-technical language when possible
        /// - Include specific numbers, dates, y identifiers
        /// - Explain business impact y significance
        /// - Reference relevant policies, procedures, o controls
        /// 
        /// **AUDIT TRAIL LINKAGE:**
        /// - Reference related processes, systems, o documents
        /// - Include approval references y authorization details
        /// - Mention compliance requirements o standards
        /// - Link to incident numbers, change requests, o tickets
        /// 
        /// **SEARCHABILITY:**
        /// - Include relevant keywords para discovery
        /// - Use consistent terminology across the organization
        /// - Include system names, process names, y business terms
        /// - Structure information para automated parsing if needed
        /// 
        /// COMPLIANCE VALUE:
        /// - **Audit Evidence**: Provides clear explanation para auditors
        /// - **Regulatory Reporting**: Supports regulatory submission narratives
        /// - **Incident Investigation**: Facilitates root cause analysis
        /// - **Business Understanding**: Bridges technical y business contexts
        /// </summary>
        [Required(ErrorMessage = "Descripción es requerida para business context y audit clarity")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Descripción debe tener entre 10 y 2000 caracteres para adequate detail")]
        public string Descripcion { get; set; } = string.Empty;
        
        /// <summary>
        /// REFERENCIA AL DATO NORMALIZADO - DATA LINEAGE FOUNDATION
        /// =====================================================
        /// 
        /// PROPÓSITO EMPRESARIAL:
        /// Foreign key relationship que establece direct linkage between metadata
        /// y the normalized data record it describes. Foundation para data lineage,
        /// impact analysis y comprehensive audit trails que span across
        /// enterprise data processing workflows.
        /// 
        /// CASOS DE USO DE DATA LINEAGE:
        /// 
        /// **UPSTREAM IMPACT ANALYSIS:**
        /// ```csharp
        /// // ESCENARIO: Source system change impact assessment
        /// var affectedMetadata = metadataService.GetByDataId(normalizedDataId);
        /// var upstreamSystems = lineageService.TraceUpstream(normalizedDataId);
        /// var impactReport = impactService.GenerateChangeImpact(affectedMetadata);
        /// ```
        /// 
        /// **DOWNSTREAM DEPENDENCY MAPPING:**
        /// ```csharp
        /// // ESCENARIO: Data deletion impact assessment
        /// var dependentProcesses = lineageService.GetDownstreamProcesses(normalizedDataId);
        /// var affectedReports = reportingService.GetAffectedReports(normalizedDataId);
        /// var businessImpact = impactService.AssessBusinessImpact(dependentProcesses);
        /// ```
        /// 
        /// **AUDIT TRAIL RECONSTRUCTION:**
        /// ```csharp
        /// // ESCENARIO: Financial audit trail construction
        /// var dataLineage = lineageService.ReconstructLineage(normalizedDataId);
        /// var transformationHistory = metadataService.GetTransformationChain(normalizedDataId);
        /// var auditReport = auditService.GenerateLineageReport(dataLineage);
        /// ```
        /// 
        /// **DATA QUALITY ROOT CAUSE:**
        /// ```csharp
        /// // ESCENARIO: Data quality issue investigation
        /// var qualityMetadata = metadataService.GetQualityHistory(normalizedDataId);
        /// var transformationChain = lineageService.GetTransformationPath(normalizedDataId);
        /// var rootCause = qualityService.IdentifyQualityIssueSource(transformationChain);
        /// ```
        /// 
        /// ENTERPRISE BENEFITS:
        /// 
        /// **REGULATORY COMPLIANCE:**
        /// - **SOX Compliance**: Complete audit trail para financial data
        /// - **GDPR Compliance**: Data processing record linkage
        /// - **FDA Validation**: Pharmaceutical data integrity requirements
        /// - **Basel III**: Banking data lineage requirements
        /// 
        /// **OPERATIONAL EXCELLENCE:**
        /// - **Change Management**: Impact assessment para system changes
        /// - **Incident Response**: Rapid identification de affected systems
        /// - **Performance Optimization**: Bottleneck identification
        /// - **Capacity Planning**: Resource utilization analysis
        /// 
        /// **BUSINESS INTELLIGENCE:**
        /// - **Data Trust**: Confidence en analytical results
        /// - **Decision Support**: Understanding data provenance
        /// - **Risk Management**: Data dependency risk assessment
        /// - **Business Continuity**: Critical data path identification
        /// 
        /// TECHNICAL IMPLEMENTATION:
        /// - **Referential Integrity**: Enforced at database level
        /// - **Indexing**: Optimized para lineage traversal queries
        /// - **Cascade Operations**: Managed deletion y archival
        /// - **Performance**: Efficient join operations para large datasets
        /// 
        /// DATA LINEAGE INTEGRATION:
        /// - **Graph Databases**: Neo4j, Amazon Neptune para complex lineage
        /// - **Metadata Catalogs**: Apache Atlas, Collibra integration
        /// - **ETL Tools**: Built-in lineage capture en Informatica, Talend
        /// - **Cloud Services**: AWS Glue, Azure Data Factory lineage
        /// </summary>
        [Required(ErrorMessage = "IdDatoNormalizado es requerido para data lineage")]
        [Range(1, int.MaxValue, ErrorMessage = "IdDatoNormalizado debe ser un ID válido positivo")]
        public int IdDatoNormalizado { get; set; }
        
        // ========== FUTURE ENTERPRISE METADATA PROPERTIES ==========
        // TODO: Additional properties para comprehensive enterprise metadata
        
        /// <summary>
        /// FUTURO: Data classification level (Public, Internal, Confidential, Restricted)
        /// </summary>
        // public string ClassificationLevel { get; set; } = "Internal";
        
        /// <summary>
        /// FUTURO: Business owner responsible para this metadata
        /// </summary>
        // public string BusinessOwner { get; set; } = string.Empty;
        
        /// <summary>
        /// FUTURO: Technical contact responsible para this metadata
        /// </summary>
        // public string TechnicalContact { get; set; } = string.Empty;
        
        /// <summary>
        /// FUTURO: Quality score (0-100) basado en completeness y accuracy
        /// </summary>
        // public int QualityScore { get; set; } = 0;
        
        /// <summary>
        /// FUTURO: Business impact level (Low, Medium, High, Critical)
        /// </summary>
        // public string BusinessImpact { get; set; } = "Medium";
        
        /// <summary>
        /// FUTURO: Retention period para automatic archival
        /// </summary>
        // public TimeSpan RetentionPeriod { get; set; } = TimeSpan.FromDays(2555); // 7 years default
        
        /// <summary>
        /// FUTURO: JSON object para additional custom properties
        /// </summary>
        // public string AdditionalProperties { get; set; } = "{}";
        
        /// <summary>
        /// FUTURO: Version number para metadata evolution tracking
        /// </summary>
        // public int Version { get; set; } = 1;
        
        /// <summary>
        /// FUTURO: Status (Active, Archived, Deprecated, Deleted)
        /// </summary>
        // public string Status { get; set; } = "Active";
    }
} 