# All In One Docker
This will create an instance of both Blazor app and the Database that is migrated for you.
### Running
Set environment variable in `./.env`
```
BRANCH=master # What git branch
PORT=3030 # Port for ASP application
SAPASSWORD=SUPERDUPERSECUR3p@ssword # SA password for database
TASKPASSWORD=SUP3rSecureP@ssword # task password for database
```
*Note: all passwords need at least 8 characters, captial, lower, number, and symbol.*

Run `./create.sh`