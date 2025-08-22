# 🌊 Simulador de Dinámica de Fluidos en Unity

Este proyecto es un **simulador interactivo de drenaje y dinámica de fluidos en 3D**, desarrollado en **Unity** con el plugin **Obi Fluid**.  
El objetivo principal es proporcionar una herramienta que permita **editar recipientes, puntos de drenaje y parámetros del fluido en tiempo real**, para estudiar y visualizar el comportamiento hidrodinámico.

---

## 🚀 Características principales
- Visualización en tiempo real de fluidos con **Obi Fluid**.
- Control de parámetros físicos (densidad, viscosidad, tensión superficial, etc.) desde la **UI**.
- Edición visual de **recipientes** y **puntos de drenaje**.
- Medición automática de:
  - Tiempo de vaciado.
  - Volumen restante.
- Reinicio de simulación desde la UI.
- Guardado y carga de configuraciones.
- Soporte para **múltiples escenarios de simulación**.
- Posibilidad de exportar resultados para comparaciones.

---

## 🛠️ Tecnologías y software utilizado
- **Unity 2022.3.62f1 (LTS)**  
- **Obi Fluid 6.5.4** (motor de simulación de fluidos)  
- **C#** para la programación de scripts  
- **Git + GitHub** para control de versiones  

---

## 📦 Instalación y ejecución
### 1. Clonar el repositorio
```bash
git clone https://github.com/ElleLevi/hydrodynamics_simulation.git
```
### 2. Abrir el proyecto en Unity
- Abrir Unity Hub.
- Seleccionar Add Project → ubicar la carpeta del repositorio clonado.
- Asegurarse de usar la versión Unity 2022.3.62f1 (o compatible).

### 3. Instalar dependencias
Importar el paquete de Obi Fluid 6.5.4 desde la Asset Store (requiere licencia).

---

## 📂 Estructura del proyecto
```bash
Assets/
 ├── Materials/          # Materiales utilizados en los recipientes y fluidos
 ├── Prefabs/            # Prefabricados de recipientes y drenajes
 ├── Scenes/             # Escenas principales de la simulación
 ├── Scripts/            # Lógica del simulador (C#)
 └── UI/                 # Interfaz de usuario
Packages/
ProjectSettings/
```

---

## 🤝 Contribuciones
Si deseas contribuir:
1. Haz un fork del proyecto.
2. Crea una rama (git checkout -b feature/nueva-funcionalidad).
3. Haz tus cambios y commits.
4. Envía un Pull Request.
