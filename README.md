# Sistema Web de Gestión Financiera

Aplicación financiera empresarial desarrollada con **C#** 
y **ASP.NET** implementando una arquitectura moderna en 
**4 capas**.

## 🏗️ Arquitectura

VISTA (ASP.NET)
↓
NEGOCIO (Lógica de negocio)
↓
DATOS (Acceso a BD)
↓
MODELO (Entidades)
↓
SQL Server
VISTA (ASP.NET)
↓
NEGOCIO (Lógica de negocio)
↓
DATOS (Acceso a BD)
↓
MODELO (Entidades)
↓
SQL Server

### Capas del Proyecto

| Capa | Responsabilidad |
|------|-----------------|
| **Modelo** | Clases de entidad (Empleado, Usuario, Región, etc.) |
| **Datos** | Acceso a SQL Server, ejecución de Store Procedures |
| **Negocio** | Lógica de negocio y validaciones |
| **Vista** | Interfaz web ASP.NET |

## ✨ Funcionalidades

### Autenticación
- Login seguro con validación de credenciales
- Gestión de sesiones

### Sistema de Roles y Permisos
- Control de acceso basado en roles (RBAC)
- Asignación de roles a usuarios
- Validación de permisos por funcionalidad
- Auditoría de acciones

### Gestión de Empleados
- CRUD completo
- Validación de datos
- Búsqueda y filtrado
- Asignación a regiones

### Administración de Usuarios
- Gestión de usuarios del sistema
- Asignación de roles
- Control de permisos de acceso
## 🔧 Tecnologías Utilizadas

**Backend:**
- C# (.NET Framework/Core)
- ASP.NET
- Arquitectura en 4 capas

**Base de Datos:**
- SQL Server
- Store Procedures

**Control de Versiones:**
- Git
- GitHub

## 🎯 Desafíos Técnicos Resueltos

1. **Migración de Windows Forms a Web**
   - Adaptación de lógica de negocio a ASP.NET
   - Implementación de capas usando carpetas del proyecto

2. **Sistema de Roles y Permisos**
   - Diseño e implementación de RBAC
   - Validación de acceso en múltiples capas

3. **Arquitectura Limpia de 4 Capas**
   - Separación clara de responsabilidades
   - Reutilización de código
   - Fácil mantenimiento y escalabilidad

4. **Store Procedures Optimizados**
   - Operaciones transaccionales seguras
   - Integridad de datos financieros

- ✅ Autenticación
- ✅ Sistema de Roles
- ✅ Gestión de Empleados
- ✅ Administración de Usuarios
- ✅ Gestión de Regiones
