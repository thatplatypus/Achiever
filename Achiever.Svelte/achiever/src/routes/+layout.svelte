<script lang="ts">
    export const ssr = false;
    
    import { ModeWatcher } from "mode-watcher";
    import Sun from "lucide-svelte/icons/sun";
    import Moon from "lucide-svelte/icons/moon";
    import { mode } from "mode-watcher";

    import { resetMode, setMode } from "mode-watcher";
    import * as DropdownMenu from "$lib/components/ui/dropdown-menu/index.js";
    import { Button } from "$lib/components/ui/button/index.js";
    import { Github, LogIn, LogOut } from "lucide-svelte";
    import  * as Tooltip from "$lib/components/ui/tooltip";
    import * as Avatar from "$lib/components/ui/avatar";
    import {Separator} from "$lib/components/ui/separator";
    import { writable } from "svelte/store";
    import { auth, fetchUser, logout } from "$lib/stores/auth";
    import { onMount } from "svelte";
    import { goto } from "$app/navigation";
    


    let currentMode = "";

mode.subscribe((value) => {
    console.log(value);
    if(value)
        currentMode = value;
});

function handleSetMode(mode) {
  setMode(mode.toLowerCase());
  currentMode = mode;
}

onMount(async () => {
    if(!$auth.isAuthenticated)
    {
        try {
        await fetchUser();
        } catch (error) {
        console.warn("User unathenticated:", error);
        }
    }
});

const avatarColors = [
  'bg-red-500', 'bg-orange-500', 'bg-yellow-500', 'bg-green-500',
  'bg-teal-500', 'bg-blue-500', 'bg-indigo-500', 'bg-purple-500',
  'bg-pink-500', 'bg-gray-500', 'bg-amber-500', 'bg-lime-500',
  'bg-cyan-500', 'bg-fuchsia-500', 'bg-rose-500', 'bg-stone-500',
  'bg-sky-500', 'bg-violet-500', 'bg-zinc-500', 'bg-slate-500',
  'bg-neutral-500', 'bg-emerald-500', 'bg-amber-600', 'bg-green-700',
  'bg-red-700', 'bg-blue-600',
];

function getAvatarColor(email: string): string {
  const firstLetter = email.charAt(0).toLowerCase();
  const index = firstLetter.charCodeAt(0) - 'a'.charCodeAt(0);
  return avatarColors[index % avatarColors.length]; // Handles letters outside 'a-z'
}

function logoutUser() {
    logout();
    goto("/login");
}
  </script>
  
  <ModeWatcher />
  
  <header class="p-4 bg-background border-b border-border">
    <div class="container flex items-center justify-between gap-3">
      <!-- App Logo -->
      <h1 class="text-xl font-bold text-foreground">Achiever</h1>
      <div class="flex-1"></div>
      <!-- Dark Mode Toggle -->
      <DropdownMenu.Root>
        <DropdownMenu.Trigger asChild let:builder>
          <Button builders={[builder]} variant="outline" size="icon">
            <Sun
              class="h-[1.2rem] w-[1.2rem] rotate-0 scale-100 transition-all dark:-rotate-90 dark:scale-0"
            />
            <Moon
              class="absolute h-[1.2rem] w-[1.2rem] rotate-90 scale-0 transition-all dark:rotate-0 dark:scale-100"
            />
            <span class="sr-only">Toggle theme</span>
          </Button>
        </DropdownMenu.Trigger>
        <DropdownMenu.Content align="end">
            <DropdownMenu.Item
              on:click={() => handleSetMode("light")}
            >
            <span class="flex items-center">
                <span
                  class={`w-2 h-2 rounded-full mr-2 bg-accent ${currentMode === "light" ? "ring-sky-900 ring-2" : ""}`}
                ></span>
                Light
              </span>
            </DropdownMenu.Item>
            <DropdownMenu.Item
              on:click={() => handleSetMode("dark")}
            >
            <span class="flex items-center">
            <span
            class={`w-2 h-2 rounded-full mr-2 bg-accent ${currentMode === "dark" ? "ring-sky-900 ring-2" : ""}`}
          ></span>
              Dark
        </span>
            </DropdownMenu.Item>
            <DropdownMenu.Item
              on:click={() => handleSetMode("system")}
            >
            <span class="flex items-center">
                <span
                class={`w-2 h-2 rounded-full mr-2 bg-accent ${currentMode === "system" ? "ring-sky-900 ring-2" : ""}`}
              ></span>
                  System
            </span>
            </DropdownMenu.Item>
          </DropdownMenu.Content>
      </DropdownMenu.Root>
      <Tooltip.Root>
        <Tooltip.Trigger>
            <Button href="https://github.com/thatplatypus/Achiever/tree/main" variant="outline" size="icon" target="_blanks">
                <Github />
                </Button>
        </Tooltip.Trigger>
        <Tooltip.Content>
          <p>GitHub</p>
        </Tooltip.Content>
      </Tooltip.Root>
     <!-- Account Avatar -->
     <DropdownMenu.Root>
        <DropdownMenu.Trigger asChild let:builder>
          <Avatar.Root class="scale-125 ml-2">
            <Avatar.Fallback class="{getAvatarColor($auth.user?.email ?? "")}">
                <Button builders={[builder]} variant="ghost" size="icon">
                {#if $auth.user?.email}
                    {$auth.user?.email.charAt(0).toUpperCase()}
                    {:else}
                    ?
                    {/if}

                </Button>
            </Avatar.Fallback>
          </Avatar.Root>
        </DropdownMenu.Trigger>
        <DropdownMenu.Content align="end">
            {#if $auth.isAuthenticated}
            <DropdownMenu.Item>
                {$auth.user?.email}
            </DropdownMenu.Item>
            <Separator class="my-2" />
            <DropdownMenu.Item
            >
            <Button variant="link" class="flex items-center" on:click={logoutUser()}>
                 Logout
                 <LogOut class="ml-2 h-4 w-4" />
            </Button>
              
            </DropdownMenu.Item>
            {:else}
            <DropdownMenu.Item
            >
            <Button variant="link" href="/login" class="flex items-center">
                 Login
                 <LogIn class="ml-2 h-4 w-4" />
            </Button>
              
            </DropdownMenu.Item>
            {/if}
          </DropdownMenu.Content>
      </DropdownMenu.Root>
    </div>
  </header>
  
  <main class="container mx-auto p-4">
    <slot />
  </main>