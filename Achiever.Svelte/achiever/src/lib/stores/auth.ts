import { writable } from 'svelte/store';

type AuthState = {
  isAuthenticated: boolean;
  user: null | { email: string; id: string }; // Adjust based on your user schema
};

export const auth = writable<AuthState>({
  isAuthenticated: false,
  user: null,
});
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function login(email: string, password: string): Promise<void> {
  const response = await fetch(
    `${API_BASE_URL}/login?useCookies=true`,
    {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      credentials: 'include', 
      referrerPolicy: 'strict-origin-when-cross-origin',
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

export async function fetchUser(): Promise<{ email: string; id: string } | null> {
  const userInfoResponse = await fetch(`${API_BASE_URL}/manage/info`, {
    method: 'GET',
    credentials: 'include',
  });

  if (!userInfoResponse.ok) {
    throw new Error('Failed to fetch user info');
  }

  const userInfo = await userInfoResponse.json();

  auth.set({
    isAuthenticated: true,
    user: {
      email: userInfo.email,
      id: userInfo.id,
    },
  });

  return {
    email: userInfo.email,
    id: userInfo.id,
  };
}

export async function loginAndFetchUser(email: string, password: string): Promise<{ email: string; id: string } | null> {
  const loginResponse = await login(email, password);

  return await fetchUser();
}

export async function register(email: string, password: string): Promise<void> {
  const response = await fetch(`${API_BASE_URL}/register`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ email, password }),
  });

  if (!response.ok) {
    try {
      const errorData = await response.json();
      const errors = errorData.errors
        ? Object.values(errorData.errors).flat()
        : [errorData.message || 'Registration failed'];
      throw new Error(errors.join('\n'));
    } catch {
      throw new Error('Unexpected error occurred during registration');
    }
  }
}

export async function logout(): Promise<void> {
  const response = await fetch(`${API_BASE_URL}/logout`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    credentials: 'include',
    body: JSON.stringify({}),
  });

  if (!response.ok) {
    throw new Error('Logout failed');
  }

  auth.set({
    isAuthenticated: false,
    user: null,
  });
}