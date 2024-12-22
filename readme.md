# Achiever

Achiever is a full-stack web application built with .NET 8 and multiple front-end clients. It's designed to showcase the capabilities of minimal APIs, Blazor, and the flexibility of having distinct clients to support an application.

The solution is structured into the following projects:

1. **Api**: This is the backend of the application, built with .NET 8 minimal APIs. It provides a clean, efficient, and flexible way to define the server-side logic of the application. The api is designed to be re-used amongst several standalone client applications.
   - DotNet Minimal API Endpoint structured to the REPR Pattern
   - MultiTenant Account Structure
   - CQRS seperation
   - Entity Framework backed by Postgres

3. **Client**: This is the frontend of the application, built with Blazor WebAssembly. It provides a rich, interactive user interface that runs in the user's web browser using WebAssembly.
   Blazor Client [demo site](https://achiever-app.azurewebsites.net/goals)
 <div style="display: flex; justify-content: center; gap: 10px;">
  <img width="49%" src="https://github.com/user-attachments/assets/ba49fd80-ac0e-4fb4-b0db-5d541d8c66e1" alt="Blazor Client Goal View" />
  <img width="49%" src="https://github.com/user-attachments/assets/e20eb60a-a76a-4f35-942e-857089c020a0" alt="Blazor Client Dark Mode" />
</div>

4. **Shared**: This project contains code that is shared between the Api and Client projects. This might include data models, constants, or utility functions.

5. **Test**: This project contains tests for the Api and Client projects. It helps ensure the reliability and correctness of the application's code.
  
6. **iOS**: This project contains a native SwiftUI iOS app for the Api backend. 
   The iOS client is not published on the apple app store at this time but feel free to build and run it from xcode using the swift source code.
   <img width="443" alt="image" src="https://github.com/user-attachments/assets/db949796-a1fa-495e-9bc6-fb5132aaeced" />
   <img width="443" alt="image" src="https://github.com/user-attachments/assets/b6b5e77b-046b-4f1f-8f8a-fb174007d3f7" />

8. **Svelte**: This project contains a client-side Svelte app built with shadcn-ui for svelte and tailwind.
   Svelete [demo site](https://happy-smoke-04e17cc1e.4.azurestaticapps.net/)
<div style="display: flex; justify-content: center; gap: 10px;">
  <img width="49%" src="https://github.com/user-attachments/assets/62903e71-6461-4085-8d9a-c4c443af76ad" alt="Svelte Client Light Mode" />
  <img width="49%" src="https://github.com/user-attachments/assets/33ce1011-d3fe-4174-a4f4-7d5c49a31a89" alt="Svelte Client Dark Mode" />
</div>

Achiever demonstrates how to build a modern, full-stack web application with .NET and SPA-Clients including Blazor WebAssembly and Svelte. It's a great starting point for anyone interested in a full-stack c# app with newer .Net features like Minimal API Endpoints and the latest identity practices with a front end client.

## Features

A simple app to help track your progress on goals.

 - Create an unlimited number of goals with optional target dates.
 - Create an unlimited number of SubTasks per goal to help breakdown complex goals.
 - Track estimated hours, notes, and status of each subtask.
 - Calculates the percent the goal is complete by looking for completed subtasks.
 - Dark & Drop support for changing subtask status on a board overview.
 - Free account works in a multi-tenant way so you can access your goals from any client or the api securely.
 - Light and Dark Modes

## Technologies

### Backend
 - DotNet 8
 - Minimal API
 - Entity Framework + Postgres SQL
 - XUnit

### Frontend Blazor Client
 - DotNet 8
 - Blazor WebAssembly
 - FluentUI
 - Apex Charts

### Frontend Svelte Client
 - SvelteKit
 - Shadcn-UI for Svelte
 - Tailwind CSS
 - Apex Charts

### iOS Client
 - SwiftUI
