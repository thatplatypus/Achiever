import { writable } from 'svelte/store';

type AuthState = {
  isAuthenticated: boolean;
  user: null | { email: string; id: string }; // Adjust based on your user schema
};

export const auth = writable<AuthState>({
  isAuthenticated: false,
  user: null,
});

export async function login(email: string, password: string): Promise<void> {
  const response = await fetch(
    'https://localhost:7211/login?useCookies=true',
    {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      credentials: 'include', 
      body: JSON.stringify({ email, password }),
    }
  );

  if (!response.ok) {
    try {
      const errorData = await response.json(); 
      throw new Error(errorData.message || 'Login failed');
    } catch {
      throw new Error('Unexpected error occurred');
    }
  }
}

export async function loginAndFetchUser(email: string, password: string): Promise<{ email: string; id: string } | null> {
  // Step 1: Login
  const loginResponse = await fetch(
    'https://localhost:7211/login?useCookies=true',
    {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      credentials: 'include', // Ensures cookies are sent/received
      body: JSON.stringify({ email, password }),
    }
  );

  if (!loginResponse.ok) {
    try {
      const errorData = await loginResponse.json();
      throw new Error(errorData.message || 'Login failed');
    } catch {
      throw new Error('Unexpected error occurred during login');
    }
  }

  // Step 2: Fetch User Info
  const userInfoResponse = await fetch('https://localhost:7211/manage/info', {
    method: 'GET',
    credentials: 'include', // Include the cookie in the request
  });

  if (!userInfoResponse.ok) {
    throw new Error('Failed to fetch user info');
  }

  const userInfo = await userInfoResponse.json();
  return {
    email: userInfo.email,
    id: userInfo.id,
  };
}