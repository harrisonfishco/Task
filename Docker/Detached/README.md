# Detached Docker
This will create an instance of the Blazor app that connects to a given database.\
*Note: you will have to migrate the databse on your own*
### Running
Set environment variable in `./.env`
```
TASK_CONNECTIONSTRING= # Connection string of database
PORT=3030 # Port to listen the Blazor app
BRANCH=master # Branch to pull the blazor app from
```
Run `docker compose up -d`