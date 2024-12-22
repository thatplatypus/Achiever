<script lang="ts">
  import { goto } from '$app/navigation'; // SvelteKit for routing
  import {auth, loginAndFetchUser} from '$lib/stores/auth';
  import {login} from '$lib/stores/auth';
  import { Button } from '$lib/components/ui/button';
    import { Input } from '$lib/components/ui/input';
    import Label from '$lib/components/ui/label/label.svelte';

  let email = '';
  let password = '';
  let errorMessage = '';

  async function handleLogin() {
    try {
      const result = await loginAndFetchUser(email, password);
      auth.set({
        isAuthenticated: true,
        user: {
          email: email,
          id: result?.id ?? "",
        },
      });
      goto('/goals'); 
    } catch (error: any) {
      errorMessage = error.message;
    }
  }
</script>

<div class="flex min-h-screen items-center justify-center">
  <div class="w-full max-w-md p-8 space-y-6 rounded-md shadow-md">
    <div class="text-center">
      <h1 class="text-2xl font-semibold">Login</h1>
      <p class="text-sm text-gray-500">Access your account with your credentials.</p>
    </div>

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

    <div class="text-sm text-center">
      Don't have an account? <a href="/register" class="text-primary hover:underline">Register</a>
    </div>
  </div>
</div>