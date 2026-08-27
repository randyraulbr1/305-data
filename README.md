# Mis Horas — cómo publicarla en GitHub Pages (Android)

1. Abre github.com en Chrome (o la app de GitHub) y entra a tu cuenta (randyraulbr1).
2. Toca **+** → **New repository**. Nombre: `mis-horas` (puede ser público). Create repository.
3. Dentro del repo vacío, toca **"uploading an existing file"** (o Add file → Upload files).
4. Selecciona estos 5 archivos/carpeta desde el zip descomprimido:
   - index.html
   - manifest.json
   - service-worker.js
   - icons/icon-192.png
   - icons/icon-512.png
   (Sube la carpeta `icons` completa para que las rutas coincidan.)
5. Commit changes.
6. Ve a **Settings** → **Pages** (menú lateral).
7. En "Branch" elige `main` y carpeta `/ (root)` → Save.
8. Espera 1-2 minutos. Tu app queda en:
   `https://randyraulbr1.github.io/mis-horas/`
9. Abre ese link en Chrome de tu teléfono → menú ⋮ → **"Instalar aplicación"**.

## Para actualizar la app después
Cada vez que quieras una versión nueva (yo te la genero), solo reemplaza los archivos
en el repo (Add file → Upload files, sube el `index.html` nuevo y confirma que
sobrescriba). La app detecta la versión nueva sola la próxima vez que la abras
y se refresca automáticamente — no hace falta reinstalarla ni borrar nada.

Si en algún momento me pasas acceso (o conectas un integrador de GitHub aquí en el
chat), puedo subir los cambios directamente por ti en vez de que lo hagas manual.
