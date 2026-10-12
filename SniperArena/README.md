# Sniper Arena (3ª persona, Unity) — base

Juego tipo "Sniper Arena" de Roblox, con personaje en 3ª persona, sniper con skins y bots.

## Instalar
1. Unity 6 → proyecto nuevo **3D (URP)**.
2. Copia la carpeta `Scripts/` a `Assets/SniperArena/Scripts/`.
3. Escena vacía → GameObject vacío → añade el componente **Bootstrap**.
4. Play. (Funciona con Input System nuevo o viejo.)

## Controles
WASD mover · Mouse mirar · Shift correr · Espacio saltar
Clic izq disparar · **Clic der mantener = apuntar (1ª persona + zoom)** · R recargar
**K cambiar skin del rifle** · Esc pausa

## Qué hace
- Sin apuntar: cámara de cadera sobre el hombro (con colisión). Apuntando: 1ª persona con mira de francotirador.
- Sniper de cerrojo: 5 balas, daño x2 a la cabeza, dispersión al disparar de cadera.
- 5 skins de arma (Rosa Neón, Oro, Negro Mate, Azul Hielo, Rojo Sangre).
- Bots que persiguen, se mueven de lado y disparan. Reaparecen al morir.
- HUD: vida, munición, skin, kills/muertes, hitmarker.

## Siguiente paso
Reemplazar cubos por el modelo del personaje (Mixamo, FBX Humanoid) y el modelo real del rifle;
las skins pasan a ser materiales/texturas en vez de colores.
