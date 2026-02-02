# 🎛️ Load-In Legends

**Load-In Legends** is a Unity-based simulation game where players take the role of professional **stagehands** responsible for loading in, building, wiring, and striking concerts and live performances inside real venues.

The goal is to simulate the real-world workflow of live event production — from empty venue to showtime.

---

## 🎯 Concept

Players explore real-world stadiums, arenas, and theaters and complete authentic stagehand tasks such as:

- Rolling cases off trucks
- Building truss and rigging lighting
- Running power and signal
- Hanging PA and video walls
- Placing backline and instruments
- Debugging wiring mistakes
- Working under time pressure before doors open
- Striking the show after load-out

This is not a music game — it’s a **behind-the-scenes live production simulator**.

---

## 🏟️ Venues

Real venues are used as playable spaces (e.g., stadiums like Lincoln Financial Field / Anfield).

Players can freely explore concourses, seating, the stage, backstage, and loading docks while completing objectives.

---

## 🛠️ Built With

- **Unity (URP)**
- **Git + Git LFS** for large 3D assets
- Real-world 3D venue models (Sketchfab / Fab)
- First-person exploration controller

---

## 🗂️ Repository Structure

Only three folders are tracked by Git:

```
Assets/ ← All game content, models, scripts
Packages/ ← Unity dependencies
ProjectSettings/ ← Unity project configuration
```

Unity-generated folders are intentionally ignored:
```
Library/
Temp/
Logs/
UserSettings/
```

---

## 🧱 Large Assets & Git LFS

This project uses **Git LFS** for large 3D models and textures (`.glb`, `.fbx`, images, audio, video).

Before cloning, you **must** install Git LFS:

```bash
git lfs install
git clone https://github.com/russellstrauss/load-in-legends.git

> ⚠️ **Do not download the ZIP from GitHub.**

---

## 🚀 Getting Started

1. Install **Unity Hub**
2. Clone the repo
3. Open the project in Unity Hub
4. Press **Play**

If materials appear pink:

> Change shader to **Universal Render Pipeline / Lit**

---

## 🎮 Planned Gameplay Systems

- Case movement physics
- Cable routing & patching logic
- Rigging / truss placement
- Lighting & audio signal chains
- Time-based objectives
- Show countdown pressure
- Multiplayer co-op load-ins
- Realistic venue navigation

---

## 🎓 Why This Game Exists

Live production is one of the most complex, skilled, and invisible professions in entertainment.  
This simulator aims to make that craft playable, understandable, and fun.

---

## 👷 Contributing

When committing:
```
git add Assets Packages ProjectSettings
git commit -m "your change"
git push
```
> ❗ **Never commit Unity cache folders.**

---

## 📦 Asset Licensing

Venue and model assets come from third-party marketplaces (Sketchfab / Fab).  
They are used under their respective licenses and may not be redistributed outside this project.

---

## 🧠 Vision

A realistic, immersive, and sometimes chaotic simulation of what it actually takes to build a live show — from truck to encore.
