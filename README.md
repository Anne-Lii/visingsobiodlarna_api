# Visingsö Beekeepers Association – Backend
Backend API for the **Visingsö Beekeepers Association** digital platform.  
Developed as part of my final degree project in Web Development (Mid Sweden University, 2025).  

---

## About
This project provides the backend for a digital platform supporting beekeepers on the island of Visingsö.  
The system allows members to register and manage:  

- Apiaries and hives  
- Mite reports (per hive and week)  
- Wintering reports  
- Honey harvests  

The application uses **role-based authentication**. Regular members can manage their own data, while **admins** can:  
- Approve new users and assign admin roles  
- Add, edit, and delete news posts  
- Create and manage calendar events  
- Manage documents shared within the association  

---

## 🛠️ Tech Stack
- **Backend:** ASP.NET Core Web API with Entity Framework Core  
- **Frontend:** React (TypeScript)  
- **Database:** SQL Server  
- **Hosting:** Azure App Service + Azure SQL + Azure Blob Storage  
- **Auth:** JWT-based authentication, role-based access control  

---

## Authentication
- All endpoints (except register and login) require a valid JWT token  
- Use header: `Authorization: Bearer {token}`  
- Admin endpoints require admin privileges  

---

## API Controllers & Endpoints (selection)

### AuthController (Registration & Login)
- `POST /api/auth/register` – Register a new user  
- `POST /api/auth/login` – Log in and receive a JWT token  

### AdminController (Admin only)
- `GET /api/admin/users` – Get all users  
- `GET /api/admin/pending` – Get users pending approval  
- `PUT /api/admin/approve/{userId}` – Approve a user  
- `PUT /api/admin/make-admin/{userId}` – Promote a user to admin  
- `DELETE /api/admin/delete/{userId}` – Delete a user  

### ApiaryController (Apiaries)
- `POST /api/apiary` – Create a new apiary  
- `GET /api/apiary` – Get all apiaries  
- `GET /api/apiary/{id}` – Get a specific apiary  
- `PUT /api/apiary/{id}` – Update an apiary  
- `DELETE /api/apiary/{id}` – Delete an apiary  

### HiveController (Hives)
- `POST /api/hive` – Create a new hive  
- `GET /api/hive/by-apiary/{apiaryId}` – Get hives in a specific apiary  
- `GET /api/hive/by-user/{userId}` – Get hives for a specific user  
- `PUT /api/hive/{id}` – Update a hive  
- `DELETE /api/hive/{id}` – Delete a hive  

### MitesController (Mite Reports)
- `POST /api/mites` – Create a mite report  
- `GET /api/mites/by-hive/{hiveId}` – Get mite reports for a hive  
- `GET /api/mites/by-apiary/{apiaryId}` – Get mite reports for an apiary  
- `PUT /api/mites/{id}` – Update a mite report  
- `DELETE /api/mites/{id}` – Delete a mite report  

### WinteringController (Wintering Reports)
- `POST /api/wintering` – Create a wintering report  
- `GET /api/wintering/by-user/{userId}` – Get wintering reports for a user  
- `PUT /api/wintering/{id}` – Update a wintering report  
- `DELETE /api/wintering/{id}` – Delete a wintering report  

### HoneyHarvestController (Honey Harvests)
- `POST /api/honeyharvest` – Create a honey harvest report  
- `GET /api/honeyharvest/by-user/{userId}` – Get honey harvests for a user  
- `PUT /api/honeyharvest/{id}` – Update a honey harvest report  
- `DELETE /api/honeyharvest/{id}` – Delete a honey harvest report  

---

## Getting Started
Clone the project:  
```bash
git clone https://github.com/Anne-Lii/visingsobiodlarna_api.git
