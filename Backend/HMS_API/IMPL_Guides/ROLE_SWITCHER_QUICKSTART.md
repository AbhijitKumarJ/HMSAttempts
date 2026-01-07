# Role Switcher Quick Start Guide

## Overview

The role switcher allows users with multiple roles to switch between their assigned roles (e.g., Nurse, Receptionist, Doctor) without requiring re-login. This enables context-aware UI that shows only the tools relevant to the current active role.

## Story Reference

- **Story**: `Docs/stories/4-1-clinical-cockpit-shell-role-switcher.md`
- **Status**: Implemented

## API Endpoint

### POST `/api/auth/switch-role`

Switch the active role for the authenticated user.

**Request Body:**
```json
{
  "role": "Nurse"
}
```

**Response (200 OK):**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "issuedAt": "2026-01-07T10:30:00Z",
  "availableRoles": ["Doctor", "Nurse", "Receptionist"]
}
```

**Error Response (400 Bad Request):**
```json
{
  "error": "Invalid role or user not authorized for this role"
}
```

**Error Response (401 Unauthorized):**
```json
{
  "error": "Invalid token"
}
```

## How It Works

1. **Authentication**: User must be logged in with a valid JWT token
2. **Role Validation**: The service checks if the requested role is assigned to the user
3. **Token Generation**: A new JWT is generated with the requested role in the claims
4. **Response**: Returns new access token and list of all available roles for the user

## Usage Flow

### Step 1: Login with Multi-Role User

```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -c cookies.txt \
  -d '{
    "username": "multiruser",
    "password": "password123"
  }'
```

Response:
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "issuedAt": "2026-01-07T10:30:00Z"
}
```

**Note**: The initial role is the first assigned role in the database.

### Step 2: Decode Current Token

Use https://jwt.io/ to decode the current token and see the active role claim:

```json
{
  "sub": "1",
  "name": "multiruser",
  "role": "Doctor",  // Current active role
  "jti": "guid-here",
  "exp": 1736261400,
  "iat": 1736257800
}
```

### Step 3: Switch to Another Role

```bash
# Switch to Nurse role
curl -X POST http://localhost:5000/api/auth/switch-role \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $ACCESS_TOKEN" \
  -d '{
    "role": "Nurse"
  }'
```

Response:
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",  // New token with Nurse role
  "expiresIn": 3600,
  "issuedAt": "2026-01-07T10:31:00Z",
  "availableRoles": ["Doctor", "Nurse", "Receptionist"]
}
```

### Step 4: Verify Role Switch

Decode the new token:

```json
{
  "sub": "1",
  "name": "multiruser",
  "role": "Nurse",  // Switched to Nurse
  "jti": "guid-here",
  "exp": 1736261460,
  "iat": 1736257860
}
```

## Frontend Integration

### Angular Service Example

```typescript
// auth.service.ts
export interface SwitchRoleRequest {
  role: string;
}

export interface SwitchRoleResponse {
  accessToken: string;
  expiresIn: number;
  issuedAt: string;
  availableRoles: string[];
}

switchRole(newRole: string): Observable<SwitchRoleResponse> {
  const request: SwitchRoleRequest = { role: newRole };
  
  return this.http.post<SwitchRoleResponse>(
    `${this.apiUrl}/switch-role`,
    request,
    {
      headers: this.getAuthHeaders(),
      withCredentials: true
    }
  ).pipe(
    tap(response => {
      // Update stored token with new one
      localStorage.setItem('accessToken', response.accessToken);
      localStorage.setItem('expiresIn', response.expiresIn.toString());
      localStorage.setItem('issuedAt', response.issuedAt);
      localStorage.setItem('availableRoles', JSON.stringify(response.availableRoles));
      
      // Emit role change event
      this.activeRole$.next(newRole);
    })
  );
}

// Signal for active role
activeRole$ = signal<string>(localStorage.getItem('activeRole') || 'Doctor');
```

### Angular Component Example

```typescript
// role-switcher.component.ts
@Component({
  selector: 'app-role-switcher',
  template: `
    <select (change)="onRoleChange($event)">
      @for (role of availableRoles; track role) {
        <option [value]="role" [selected]="role === activeRole()">
          {{ role }}
        </option>
      }
    </select>
  `
})
export class RoleSwitcherComponent {
  availableRoles: string[] = [];
  activeRole = inject(AuthService).activeRole;
  
  constructor(private authService: AuthService) {
    this.availableRoles = JSON.parse(
      localStorage.getItem('availableRoles') || '[]'
    );
  }
  
  onRoleChange(event: Event) {
    const newRole = (event.target as HTMLSelectElement).value;
    this.authService.switchRole(newRole).subscribe({
      next: () => console.log('Role switched successfully'),
      error: (err) => console.error('Failed to switch role:', err)
    });
  }
}
```

## Database Setup

### Create Multi-Role Test User

```sql
-- Insert user with BCrypt password hash (password: password123)
INSERT INTO users (username, password_hash, is_active) VALUES
('multiruser', '$2a$11$abcdefghijklmnopqrstuvw', TRUE);

-- Assign multiple roles to user
INSERT INTO user_roles (user_id, role_id)
VALUES
((SELECT id FROM users WHERE username = 'multiruser'), 1),  -- Doctor
((SELECT id FROM users WHERE username = 'multiruser'), 2),  -- Nurse
((SELECT id FROM users WHERE username = 'multiruser'), 3);  -- Receptionist

-- Verify roles
SELECT u.username, r.name
FROM users u
JOIN user_roles ur ON u.id = ur.user_id
JOIN roles r ON ur.role_id = r.id
WHERE u.username = 'multiruser';
```

Expected output:
```
 username   |    name
------------+-------------
 multiruser | Doctor
 multiruser | Nurse
 multiruser | Receptionist
```

## Common Use Cases

### 1. Nurse Switching to Receptionist

A user who is both a Nurse and Receptionist can switch views:
- **Nurse Mode**: Access Triage, Clinical Cockpit, Vitals
- **Receptionist Mode**: Access Billing, Registration, Scheduling

### 2. Doctor Switching Roles

A Doctor with additional roles can access different toolsets based on context:
- Switch to **Nurse** role for triage assistance
- Switch to **Receptionist** role for appointment scheduling

### 3. Frontend UI Adaptation

Based on the active role signal, the UI dynamically shows/hides features:

```typescript
// Show Clinical Cockpit only for Nurse role
showClinicalCockpit = computed(() => 
  this.auth.activeRole() === 'Nurse'
);

// Show Billing only for Receptionist role
showBilling = computed(() => 
  this.auth.activeRole() === 'Receptionist'
);
```

## Security Considerations

### Role Validation

The backend validates that:
1. The user is authenticated (valid JWT token)
2. The requested role is actually assigned to the user
3. The user account is active

### Token Refresh

After switching roles:
- The old access token remains valid until expiration
- The new token has the updated role claim
- Frontend should replace the stored token immediately
- No need to refresh the HttpOnly cookie (not used for role switching)

### Authorization Policies

Role-based authorization can be applied to endpoints:

```csharp
[HttpGet("clinical")]
[Authorize(Roles = "Nurse,Doctor")]
public async Task<IActionResult> GetClinicalData()
{
    // Only users with Nurse or Doctor role
}

[HttpGet("billing")]
[Authorize(Roles = "Receptionist")]
public async Task<IActionResult> GetBillingData()
{
    // Only users with Receptionist role
}
```

## Testing Checklist

Before deploying to production:

- ✅ Users can only switch to roles they are assigned
- ✅ New token contains the correct role claim
- ✅ Available roles list is accurate
- ✅ Invalid role requests return 400 Bad Request
- ✅ Unauthenticated requests return 401 Unauthorized
- ✅ Frontend updates token storage on successful switch
- ✅ UI adapts to the new role immediately

## Troubleshooting

### 1. Role switch returns 400 Bad Request

**Cause**: User not assigned to the requested role

**Solution**:
```bash
# Check user's assigned roles
psql -U postgres -d hms -c "
  SELECT u.username, r.name
  FROM users u
  JOIN user_roles ur ON u.id = ur.user_id
  JOIN roles r ON ur.role_id = r.id
  WHERE u.username = 'multiruser';
"

# Assign missing role if needed
INSERT INTO user_roles (user_id, role_id)
VALUES (
  (SELECT id FROM users WHERE username = 'multiruser'),
  (SELECT id FROM roles WHERE name = 'Nurse')
);
```

### 2. Role switch returns 401 Unauthorized

**Cause**: Invalid or expired JWT token

**Solution**:
```bash
# Refresh token first
curl -X POST http://localhost:5000/api/auth/refresh \
  -b cookies.txt \
  -c cookies.txt

# Then retry role switch with new token
```

### 3. Available roles empty in response

**Cause**: User has no roles assigned

**Solution**:
```bash
# Assign at least one role
INSERT INTO user_roles (user_id, role_id)
VALUES (
  (SELECT id FROM users WHERE username = 'multiruser'),
  (SELECT id FROM roles WHERE name = 'Doctor' LIMIT 1)
);
```

### 4. Frontend UI doesn't update

**Cause**: Token not stored or signal not triggered

**Solution**:
```typescript
// Ensure token is stored
localStorage.setItem('accessToken', response.accessToken);

// Emit role change event
this.activeRole$.next(newRole);

// Or use signal with Angular 17+
this.activeRole.set(newRole);
```

## Next Steps

After role switching is working:

1. **Implement Role-Based Menus** - Create navigation menus filtered by role
2. **Add Permission Checks** - More granular permissions within roles
3. **Implement Role History** - Track when users switch roles (audit log)
4. **Add Role Expiration** - Optionally limit role switching frequency
5. **Implement Role Default** - Remember user's preferred role

## Resources

- **Story Details**: `Docs/stories/4-1-clinical-cockpit-shell-role-switcher.md`
- **Backend Code**: `HMS.Business/Auth/AuthService.cs`
- **Controller**: `HMS.API/Controllers/Auth/AuthController.cs`
- **Entities**: `HMS.Entity/Auth/AuthEntities.cs`
- **Test Client**: `wwwroot/role-switcher.html`

## Support

If you encounter issues:

1. Check logs in console output
2. Verify user has multiple roles in database
3. Ensure JWT token is valid
4. Check availableRoles response matches database
5. Verify role names match exactly (case-sensitive)

The role switcher module is now ready for integration with the clinical cockpit shell!