# Visingsöbiodlarna - Digital plattform för kommunikation till Visingsöbiodlarna

Visingsöbiodlarna är ett examensarbete där syftet är att skapa en digital plattform för biodlare på Visingsö. Plattformen hjälper medlemmar att registrera och hantera information om sina bigårdar, kupor, kvalsterrapporter, invintringsrapporter och honungsskördar.

Alla endpoints (förutom register och login) kräver inloggning med JWT-token (Authorization: Bearer {token}).
Vissa endpoints kräver även att användaren är admin.

## Tekniker som används
* Backend: ASP.NET Core Web API med Entity Framework Core
* Frontend: React
* Databas: SQL Server
* Hosting: Azure

## Controllers och endpoints

### AuthController (Registrering och Inloggning)
POST	/api/auth/register	                Registrera en ny användare
POST	/api/auth/login	                    Logga in och få JWT-token

### AdminController (Admin-behörighet krävs)
GET     /api/admin/users	                Hämta alla användare
GET	    /api/admin/pending	                Hämta användare som väntar på godkännande
PUT	    /api/admin/approve/{userId}	        Godkänn en användare
PUT	    /api/admin/make-admin/{userId}	    Uppgradera användare till admin
DELETE	/api/admin/delete/{userId}	        Radera en användare

### ApiaryController (Bigårdar)
POST	/api/apiary	                        Skapa en ny bigård
GET	    /api/apiary	                        Hämta alla bigårdar
GET	    /api/apiary/{id}	                Hämta specifik bigård
PUT	    /api/apiary/{id}	                Uppdatera bigård
DELETE	/api/apiary/{id}	                Radera bigård

### HiveController (Kupor)
POST	/api/hive	                        Skapa en kupa
GET	    /api/hive	                        Hämta alla kupor (admin)
GET	    /api/hive/by-apiary/{apiaryId}	    Hämta kupor i en viss bigård
GET	    /api/hive/by-user/{userId}	        Hämta kupor för en viss användare
PUT	    /api/hive/{id}	                    Uppdatera en kupa
DELETE	/api/hive/{id}	                    Radera en kupa

### MitesController (Kvalsterrapporter)
POST	/api/mites	                        Skapa kvalsterrapport
GET	    /api/mites/by-hive/{hiveId}	        Hämta kvalsterrapporter för en kupa
GET	    /api/mites/by-apiary/{apiaryId}	    Hämta kvalsterrapporter för en bigård
PUT	    /api/mites/{id}	                    Uppdatera kvalsterrapport
DELETE	/api/mites/{id}	                    Radera kvalsterrapport

### WinteringController (Invintringsrapporter)
POST	/api/wintering	                    Skapa invintringsrapport
GET	    /api/wintering/by-user/{userId}	    Hämta invintringsrapporter för en användare
PUT	    /api/wintering/{id}	                Uppdatera invintringsrapport
DELETE	/api/wintering/{id}	                Radera invintringsrapport

### HoneyHarvestController (Honungsskörd)
POST	/api/honeyharvest	                Skapa skörderapport
GET	    /api/honeyharvest/by-user/{userId}	Hämta skörderapporter för en användare
PUT	    /api/honeyharvest/{id}	            Uppdatera skörderapport
DELETE	/api/honeyharvest/{id}	            Radera skörderapport

```bash
git clone https://github.com/Anne-Lii/visingsobiodlarna_api.git
```