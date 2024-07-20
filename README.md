# Despliegue del Juego en Unity con Git y Git LFS

Este documento describe los pasos necesarios para configurar y desplegar el juego utilizando Unity, Git y Git LFS. Proyecto desarrollado en el curso de Interacción Humano Computador 2024-1, por Kalos Lazo

## Pre-requisitos

Software necesario:
- [Git](https://git-scm.com/downloads)
- [Git Large File Storage (LFS)](https://git-lfs.github.com/)
- [Unity Hub](https://unity.com/download)

## Git LFS

Git LFS se utiliza para manejar archivos grandes eficientemente en este proyecto. Para configurarlo ten en cuenta las siguientes instrucciones:

**1. Instala Git LFS**:
Abre una terminal y ejecuta el siguiente comando para instalar Git LFS en tu sistema:
```bash
git lfs install
```

**2. Configura tipo Git LFS**:
Debes configurar Git LFS para que maneje automáticamente los archivos grandes específicos de este proyecto, como archivos de texturas, modelos 3D y audios. Ejecuta los siguientes comandos en la raíz de tu repositorio:

```bash
git lfs track "*.psd"
git lfs track "*.png"
git lfs track "*.mp3"
git lfs track "*.wav"
git lfs track "*.blend"
git lfs track "*.fbx"
git lfs track "*.tif"
```

**3. Clonar repositorio**
Una vez configurado Git LFS, puedes clonar el repositorio de tu proyecto:

```bash
git clone https://github.com/kaloslazo/cross-asia-work
```

## Abrir el Proyecto en Unity

1. **Abrir Unity Hub**:
   Inicia Unity Hub en tu computadora.

2. **Agregar el proyecto**:
   En Unity Hub, ve a la pestaña 'Projects', haz clic en 'Add' y selecciona la carpeta donde has clonado tu proyecto.

3. **Abrir el proyecto**:
   Haz clic en el proyecto que acabas de agregar para abrirlo con la versión de Unity configurada para ese proyecto.

## Despliegue y Pruebas

Una vez abierto el proyecto en Unity, puedes proceder a compilar y ejecutar el juego para probarlo:

1. **Compilar el juego**:
   Ve a `File > Build Settings`, selecciona la plataforma de destino y haz clic en `Build`.

2. **Ejecutar el juego**:
   Una vez compilado, puedes ejecutar el archivo ejecutable generado para probar el juego.

## Subir Cambios al Repositorio

Después de hacer cambios en tu proyecto, no olvides subir los cambios al repositorio:

```bash
git add .
git commit -m ""
git push origin main
```