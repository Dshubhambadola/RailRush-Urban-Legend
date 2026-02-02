# RailRush: Urban Legend

> "Outrun the past, chase the future"

RailRush is a high-octane endless runner set in a cyberpunk-meets-street-art urban railway network. Players control graffiti artists fleeing from AI security drones through neon-lit metros and rooftops.

## Project Structure

- **Client/**: Unity project containing game logic and assets.
- **Server/**: Node.js backend API for progression, leaderboards, and auth.

## Getting Started

### Prerequisites

- Unity 2022 LTS or newer
- Node.js v18+
- npm

### Server Setup

1. Navigate to the server directory:
   ```bash
   cd Server
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Run tests:
   ```bash
   npm test
   ```
4. Start the server:
   ```bash
   npm start
   ```

### Client Setup

1. Open Unity Hub.
2. Add the `Client` folder as a new project.
3. Open the project.
4. Open the `Assets/Scenes/Main` scene (create if not exists).

## Core Features (Prototype)

- **Infinite Running**: Procedural track generation.
- **Movement**: 3-lane swipe system, jumping, sliding.
- **Backend**: Health check API and test infrastructure.

## License

Private - All rights reserved.
