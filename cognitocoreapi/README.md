## Dotnet core api to demonstrate the AWS Cognito simple use cases

**Purpose**

This is dotnet core 2.1 web api which is created to demonstrate how to use the aws cognito and cover simple scenarios like Login, Registration, Forgot password.

**Prerequisite**

1. Need to have an AWS account
2. Create a User Pool in cognito service

## End Points

**Register**

Register a user in aws cognito

http://localhost:5000/api/account/register

**Input**
```JSON

{
    "email":"someemail@something.com",
    "password":"somepasword",
    "confirmpassword":"somepassword",
    "firstname":"somename",
    "lastname":"lastname",
    "phonenumber":"somenumber"
}
```
An email confirmation link will be sent to the email of the user, once user click on it the user will be confirmed.

**Login**

Login functionality for user

http://localhost:5000/api/account/login

**Input**
```JSON

{
    "email":"someemail@something.com",
    "password":"somepasword"
}
```
**Output**
```JSON

{
    "metadata":{
        "token":"idtokenfromcognito",
        "username":"usernamefromcognito"
    }
}
```

**Reset**

Reset password for user

http://localhost:5000/api/account/reset

**Input**
```JSON

{
    "token":"idtokenfromcognito",
    "oldpassword":"somepasword",
    "newpassword":"somenewpasword"
}
```
User password will be reset to new one

**Forgot**

Forgot password workflow for user

http://localhost:5000/api/account/forgot

**Input**
```JSON

{
    "email":"some@email.com"
}
```
An email confirmation link will be sent to the email of the user






