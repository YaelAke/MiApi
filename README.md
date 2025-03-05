# API en C# con Patrón Fachada y Conexion a Base de Datos en MongoDB

Este proyecto implementa una **API** en **C# con .NET 7**, siguiendo el **Patrón de Diseño Fachada**. La API maneja la gestión de alumnos en una base de datos **MongoDB**, proporcionando endpoints para crear, actualizar, obtener y eliminar alumnos.

## Características Principales
- **Patrón Fachada:** Se centraliza la lógica de negocio en una única capa de servicios, simplificando la interacción con la base de datos.
- **MongoDB como Base de Datos:** Almacena los datos de los alumnos en una colección optimizada.
- **Arquitectura en Capas:** Separación clara entre **Controllers**, **Services** y **Models**.
- **Swagger UI:** Documentación automática y pruebas de API integradas.
- **Configuración de CORS:** Permite el acceso desde clientes externos y no solo en local

## Tecnologías Utilizadas
- **C# y .NET 7**  
- **MongoDB**  
- **Swagger (para la documentación de la API)**  
- **CORS habilitado** para permitir peticiones externas  
- **Patrón Fachada** para simplificar el acceso a la base de datos  

---

## **¿Qué es el Patrón de Diseño Fachada?**
El Patrón Fachada proporciona una interfaz única para acceder a un subsistema complejo, reduciendo la dependencia de múltiples clases y simplificando el uso de servicios.

### **Características del Patrón Fachada**
- **Abstracción:** La API oculta la complejidad de la base de datos y solo expone operaciones clave.  
- **Simplicidad:** Proporciona un único punto de acceso (`AlumnoService`) en lugar de múltiples clases.  
- **Desacoplamiento:** Los controladores (`AlumnosController`) no interactúan directamente con MongoDB, sino a través de `AlumnoService`.  
- **Mantenimiento y Escalabilidad:** Se pueden agregar nuevas funciones sin modificar los controladores.  
- **Mejor organización del código:** Se mantiene la separación entre lógica de negocio y acceso a datos.  

### **Diagrama UML del Patrón Fachada**
![Diagrama UML]()


