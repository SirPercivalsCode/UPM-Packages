# Unity Modular UPM Packages

A collection of **modular Unity Package Manager (UPM) packages** used to quickly bootstrap new Unity projects with a clean structure, reusable runtime code, editor tools, and script templates.

This setup is designed to:

- reduce project setup time
- avoid copy-pasting between projects
- keep `Assets/` clean
- scale cleanly across many projects

---

## 📦 Package Overview

Each folder in this repository is a **standalone UPM package**.

unity-packages
├── com.sirpercival.all
├── com.sirpercival.core
├── com.sirpercival.misc
├── com.sirpercival.audio
└── com.sirpercival.ui

- `Runtime/`
  Included in builds
- `Editor/`
  Editor-only (menu items, inspectors, templates)
- `Editor/Templates/`
  Automatically detected by Unity as script templates

---

## 🧠 Package Details

### `com.sirpercival.core`

**Foundation layer**

Contains:

- base patterns (e.g. Singleton)
- math helpers
- extensions
- general utilities

Rules:

- ❌ No `UnityEditor` usage
- ✅ Safe dependency for all other packages

---

### `com.sirpercival.misc`

**Always-import utilities**

Contains:

- generic helpers
- script templates (`Singleton`, `Enum`, etc.)
- editor tools
- project setup utilities (folder creation, tooling)

This is usually the **first package** added to a new project.

---

### `com.sirpercival.ui`

**Reusable UI systems**

Contains:

- `UIManagerSingleton`
- Toast notifications
- Confirmation popups
- Shared UI logic

Designed to be:

- runtime-safe
- project-agnostic
- easily extensible

---

## 📥 Installing a Package

### Via Git URL (recommended)

In Unity:

**Window → Package Manager → + → Add package from git URL**
