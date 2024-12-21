<script lang="ts">
    import { ModeWatcher } from "mode-watcher";
    import Sun from "lucide-svelte/icons/sun";
    import Moon from "lucide-svelte/icons/moon";
    import { mode } from "mode-watcher";

    import { resetMode, setMode } from "mode-watcher";
    import * as DropdownMenu from "$lib/components/ui/dropdown-menu/index.js";
    import { Button } from "$lib/components/ui/button/index.js";
    import { Github } from "lucide-svelte";
    import  * as Tooltip from "$lib/components/ui/tooltip";
    import { writable } from "svelte/store";
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
     
    </div>
  </header>
  
  <main class="container mx-auto p-4">
    <slot />
  </main>