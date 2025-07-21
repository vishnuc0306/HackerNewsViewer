# HackerNewsViewer

This is a full-stack web application to display the latest Hacker News stories. The backend is built with ASP.NET Core Web API and the frontend is built using Angular 15.

---

## Features

- Fetches and displays newest stories from Hacker News
- Search functionality
- Pagination support
- Responsive card layout UI
- Story details like author, points, time, comments, and domain

---

## Tech Stack

**Frontend:**
- Angular 15
- TypeScript
- HTML/CSS
- Cypress (for frontend testing)

**Backend:**
- ASP.NET Core Web API
- C#
- HttpClient
- xUnit (for API testing)

---

## How to Run

### Backend (ASP.NET Core)

1. Open the solution in Visual Studio.
2. Build the project.
3. Run the backend using IIS Express or `dotnet run`.
4. Swagger should be available at:
   ```
   https://localhost:7288/swagger/index.html
   ```

---

### Frontend (Angular)

1. Open the `frontend` folder in Visual Studio Code.
2. Run the following commands:

   ```bash
   npm install
   ng serve
   ```

3. App will be available at:
   ```
   http://localhost:4200
   ```

---

### Run Tests

#### Backend Tests

1. Open the solution in Visual Studio.
2. Right-click the `tests` project.
3. Click **Run Tests**.

#### Frontend Tests (Cypress)

1. Navigate to the `frontend` folder.
2. Run the following commands:

   ```bash
   npm install
   npx cypress open
   ```

3. Choose a spec to run in the Cypress UI.

---

## Folder Structure

```
HackerNewsViewer/
├── backend/           // .NET Core Web API
├── frontend/          // Angular 15 application + Cypress tests
├── tests/             // API test cases (xUnit)
```

---

## Notes

- Make sure the backend is running before launching the frontend.
- Backend should be accessible at: `https://localhost:7288`
- Frontend is configured to make API calls to the above URL.