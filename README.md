# RailRush: Urban Legend

**RailRush: Urban Legend** is a high-octane endless runner set in a cyberpunk metropolis. Dash, jump, and slide through the subway lines, dodging trains and drones while building your Crew's reputation.

## 🎮 How to Play
**Objective**: Run as far as you can, collect Coins, and gain Crew Rep.
- **Swipe UP**: Jump
- **Swipe DOWN**: Slide / Roll
- **Swipe LEFT/RIGHT**: Switch Lanes
- **Collect Coins**: Used to unlock new Characters and Boards.
- **Avoid Obstacles**: Trains, Barriers, and Drones will end your run!

## 🚀 Development Setup

### 1. Database (PostgreSQL)
Ensure you have PostgreSQL installed and running. Create a database named `railrush`.
```bash
# Optional: If using docker
docker run --name railrush-db -e POSTGRES_PASSWORD=password -d -p 5432:5432 postgres
```
Run the schema script in your SQL tool or terminal:
`Server/db/schema.sql`

### 2. Backend Server (Node.js)
Navigate to the Server directory:
```bash
cd Server
npm install
# Ensure .env is set up or use defaults (localhost, 5432, postgres/password)
npm start
```
*Server runs on http://localhost:3000*

### 3. Game Client (Unity)
1.  Open **Unity Hub** and add the `Client/` folder as a project.
    -   *Note*: This project is compatible with **Unity 6 (6000.x)** and **2022 LTS**. If prompted to upgrade, click **Confirm**.
2.  **Open Scene**: Go to `Assets/Scenes/Main.unity` (or create one if empty).
3.  **Setup Scene**:
    -   Ensure `GameManager`, `TrackManager`, `APIManager`, `AdManager` are in the scene.
    -   Link `TrackPrefabs` and `CharacterData` in the Inspectors.
4.  **Play**: Press the Play button in the Editor.

## 🛠 Features Implemented
-   **Core**: Endless 3-lane running, Object Pooling (60 FPS target).
-   **Backend**: Auth (Register/Login), Secure Run Validation, Leaderboards.
-   **Social**: Create/Join Crews.
-   **Monetization**: Interstitial/Rewarded Ads (Mock), IAP Verification.

## 📂 Project Structure
-   `/Client`: Unity Project (C#)
-   `/Server`: Node.js Express API + Jest Tests

---
*Version 0.6.0-monetization (Beta Candidate)*

## Core Features (Prototype)

- **Infinite Running**: Procedural track generation.
- **Movement**: 3-lane swipe system, jumping, sliding.
- **Backend**: Health check API and test infrastructure.

## License

Private - All rights reserved.
