# Shiko External Auth API

Microservice responsible for handling Google OAuth2 authentication. Acts as a bridge between the Next.js frontend and the internal Auth API, validating Google ID tokens and returning access and refresh tokens upon successful login.

## 🚀 Features

- **Google Token Validation**: Validates Google ID tokens using the `Google.Apis.Auth` library, ensuring the token is legitimate before proceeding.
- **User Provisioning**: Forwards the verified email to the Auth API, which automatically creates a new account if the user does not already exist — with `EmailConfirmed` set to `true` since Google has already verified the email address.
- **Token Passthrough**: Returns the same `LoginResult` (access token, refresh token, user info) as the standard login flow, allowing the frontend to handle Google login identically to email/password login.


## 🏁 Getting Started

This service is deployed to Azure and runs as part of the Shiko microservices architecture. For local development, follow the steps below.


## 🔗 Related Services

- **Shiko Auth API** – Handles user creation and token generation
- **Shiko Frontend** – Sends the Google ID token to this API after the user authenticates with Google
