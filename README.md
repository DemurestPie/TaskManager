# CSCI 3110 Task Manager App

CSCI 3110 project managing tasks, users, and projects built using ASP.NET MVC. This app uses Entity Framework Core with Identity for authentication and includes full CRUD functionality for tasks, users, and projects.

## Features

- User authentication and role management using role based user logic (Admin, Manager, User)
- Create, view, edit, and delete tasks
- Built with .NET 8 / Entity Framework Core / Identity

## Roles and Access Control

- Only Managers can create tasks and projects
- Only Admins can delete tasks and projects

<b>The user view currently has no access control for testing purposes. You can make yourself Admin by editing your user in the User view and selecting Admin from the dropdown.

## Main Pages

All pages require users to be signed in

| Page           | URL Path          | Description                                          |
| -------------- | ----------------- | ---------------------------------------------------- |
| Dashboard      | `/index`          | Summary stats and recent tasks                       |
| Tasks          | `/Task/Index`     | View and manage tasks                                |
| Create Task    | `/Task/Create`    | Create new task (Manager/Admin)                      |
| Projects       | `/Project/Index`  | View and manage projects                             |
| Create Project | `/Project/Create` | Create new project (Manager/Admin)                   |
| Users          | `/User/Index`     | View all users (Not restricted for testing purposes) |

## RESTful API Endpoints

### Task API

| Method | Route                     | Description       |
| ------ | ------------------------- | ----------------- |
| GET    | `/api/taskapi`            | List all tasks    |
| GET    | `/api/taskapi/{id}`       | Get task by ID    |
| POST   | `/api/taskapi/create`     | Create a new task |
| PUT    | `/api/taskapi/update{id}` | Update task       |
| DELETE | `/api/taskapi/delete{id}` | Delete task       |

### Project API

| Method | Route                                  | Description            |
| ------ | -------------------------------------- | ---------------------- |
| GET    | `/api/projectapi`                      | List all projects      |
| GET    | `/api/projectapi/{id}`                 | Get project by ID      |
| POST   | `/api/projectapi/create`               | Create a new project   |
| PUT    | `/api/projectapi/update/{id}`          | Update project         |
| DELETE | `/api/projectapi/delete/{id}`          | Delete project         |
| POST   | `/api/projectapi/assign/{id}?taskId=3` | Assign task to project |

### Dashboard API

| Method | Route               | Description                                    |
| ------ | ------------------- | ---------------------------------------------- |
| GET    | `/api/dashboardapi` | Returns stats + recent tasks for the dashboard |

## Accessibility using WCAG 2.1 AA

This project follows WCAG 2.1 AA accessibility standards to ensure usability for all users.

- **Color contrast** for all text elements and buttons
- **Keyboard navigation**: All interactive elements (buttons, links, form fields) are operable by keyboard.
- **Form accessibility**:
  - Labels are associated with form fields using `asp-for`.
  - Error messages are displayed with semantic elements and ARIA roles (`.text-danger`).
- **Semantic HTML** used proper hierarchy such as `<h1>`, `<h2>`, ect.
- **Page titles and heading structure** provide clear and easy to use navigation

Accessibility has been tested using:

- Keyboard-only navigation
- Chrome Lighthouse Accessibility Audit

## Technologies Used

- ASP.NET Core MVC
- Entity Framework Core
- SQLite
- ASP.NET Identity
- Bootstrap

## AI Disclosure

Some AI was used in the making of this project such as ChatGPT. AI was used to help understand ASP.NET core and idenity and how entity framework works and diagnose runtime errors, compiler errors, and database errors. For example, AI taught me that entity framework has roles built in and doesn't require properties such as `isAdmin` in the user model.
