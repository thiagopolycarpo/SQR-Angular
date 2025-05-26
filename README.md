# SQR Production

A production tracking application with an Angular frontend and .NET backend, built for the SQR  frontend test.

## Structure
- \`SQRFrontend/\`: Angular 14 frontend with production entry form and records display.
- \`SQRBackend/\`: .NET backend with SQLite database (\`sqr.db\`) and API endpoints.
- \`SQRFrontend-React/\`: React frontend (TypeScript, Vite) for the SQR system.

## Setup
### Prerequisites
- Node.js 16
- Angular CLI 14
- .NET SDK 8
- SQLite

### Backend
\`\`\`bash
cd SQRBackend
dotnet restore
dotnet ef database update
dotnet run
\`\`\`

### Frontend
\`\`\`bash
cd SQRFrontend
npm install
ng serve
\`\`\`

### Docker
- Backend: \`docker build -t sqrbackend .\`
- Frontend: \`docker build -t sqrfrontend .\`

## Usage
- Access frontend at \`http://localhost:4200\`.
- Submit production records and view them at \`/productions\`.

## Notes
- Database is seeded with orders \`111\` (cycle time 30.3s) and \`222\` (cycle time 45.5s).
- Use email \`teste@sqr.com.br\` for submissions.
EOL
