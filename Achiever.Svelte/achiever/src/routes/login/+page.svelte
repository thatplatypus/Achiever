<script lang="ts">
  import { goto } from '$app/navigation';
  import { auth, loginAndFetchUser, register } from '$lib/stores/auth';
  import { Button } from '$lib/components/ui/button';
  import { Input } from '$lib/components/ui/input';
  import Label from '$lib/components/ui/label/label.svelte';
  import * as Tabs from '$lib/components/ui/tabs/index.js';

  let email = '';
  let password = '';
  let confirmPassword = '';
  let errorMessage = '';
  let activeTab = 'login'; // 'login' or 'register'

  async function handleLogin() {
    try {
      const result = await loginAndFetchUser(email, password);
      auth.set({
        isAuthenticated: true,
        user: {
          email: email,
          id: result?.id ?? '',
        },
      });
      goto('/goals');
    } catch (error: any) {
      errorMessage = error.message;
    }
  }

  async function handleRegister() {
    try {
      if (password !== confirmPassword) {
        throw new Error("Passwords don't match.");
      }

      await register(email, password);
      errorMessage = '';
      activeTab = 'login'; // Redirect to login after registration
    } catch (error: any) {
      errorMessage = error.message;
    }
  }
</script>

<div class="flex min-h-screen items-center justify-center">
  <div class="w-full max-w-md p-8 space-y-6 rounded-md shadow-md">
    <div class="text-center">
      <h1 class="text-2xl font-semibold">Welcome</h1>
      <p class="text-sm text-gray-500">Access your account or register a new one.</p>
    </div>

    <Tabs.Root value={activeTab} on:change={(e) => (activeTab = e.detail.value)}>
      <Tabs.List class="grid grid-cols-2">
        <Tabs.Trigger value="login">Login</Tabs.Trigger>
        <Tabs.Trigger value="register">Register</Tabs.Trigger>
      </Tabs.List>

      <!-- Login Tab -->
      <Tabs.Content value="login">
        <form on:submit|preventDefault={handleLogin} class="space-y-4">
          {#if errorMessage}
            <div class="p-2 text-sm text-red-600 bg-red-100 rounded">{errorMessage}</div>
          {/if}

          <div>
            <Label for="email" class="block text-sm font-medium text-gray-700">Email</Label>
            <Input
              id="email"
              type="email"
              bind:value={email}
              class="w-full p-2 mt-1 border rounded-md focus:ring-primary focus:ring-offset-2"
              required
            />
          </div>

          <div>
            <Label for="password" class="block text-sm font-medium text-gray-700">Password</Label>
            <Input
              id="password"
              type="password"
              bind:value={password}
              class="w-full p-2 mt-1 border rounded-md focus:ring-primary focus:ring-offset-2"
              required
            />
          </div>

          <Button type="submit" class="w-full">Login</Button>
        </form>
      </Tabs.Content>

      <!-- Register Tab -->
      <Tabs.Content value="register">
        <form on:submit|preventDefault={handleRegister} class="space-y-4">
          {#if errorMessage}
            <div class="p-2 text-sm text-red-600 bg-red-100 rounded">{errorMessage}</div>
          {/if}

          <div>
            <Label for="email" class="block text-sm font-medium text-gray-700">Email</Label>
            <Input
              id="email"
              type="email"
              bind:value={email}
              class="w-full p-2 mt-1 border rounded-md focus:ring-primary focus:ring-offset-2"
              required
            />
          </div>

          <div>
            <Label for="password" class="block text-sm font-medium text-gray-700">Password</Label>
            <Input
              id="password"
              type="password"
              bind:value={password}
              class="w-full p-2 mt-1 border rounded-md focus:ring-primary focus:ring-offset-2"
              required
            />
          </div>

          <div>
            <Label for="confirmPassword" class="block text-sm font-medium text-gray-700">Confirm Password</Label>
            <Input
              id="confirmPassword"
              type="password"
              bind:value={confirmPassword}
              class="w-full p-2 mt-1 border rounded-md focus:ring-primary focus:ring-offset-2"
              required
            />
          </div>

          <Button type="submit" class="w-full">Register</Button>
        </form>
      </Tabs.Content>
    </Tabs.Root>
  </div>
</div>