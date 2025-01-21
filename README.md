# TaskManagement
# TaskManagement


[AUTH]

POST
/api/auth/register

Example Value
Schema
{
  "name": "string",
  "email": "string",
  "password": "string"
}

POST
/api/auth/login

Example Value
Schema
{
  "email": "string",
  "password": "string"
}


[TASKS]


GET
/api/tasks

POST
/api/tasks

Example Value
Schema
{
  "id": 0,
  "title": "string",
  "description": "string",
  "status": "string",
  "assignedUserId": 0,
  "assignedUser": {
    "id": 0,
    "name": "string",
    "email": "string",
    "passwordHash": "string",
    "role": "string"
  },
  "createdAt": "2025-01-21T22:47:02.669Z",
  "updatedAt": "2025-01-21T22:47:02.669Z"
}

GET
/api/tasks/{id}

PUT
/api/tasks/{id}

DELETE
/api/tasks/{id}
Users

[USERS]

GET
/api/users
