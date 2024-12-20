<script lang="ts">
    import type { SubTask } from "$lib/api/goals";
    import { dndzone } from "svelte-dnd-action";
    import { flip } from "svelte/animate";
  
    export let dropZoneId: string; // "New", "InProgress", or "Completed"
    export let subtasks: SubTask[] = []; // List of subtasks in this dropzone
    export let onUpdateStatus: (subtasks: any[], dropZoneId: string) => void; // Callback when subtasks are updated
  
    function handleDndConsider(e) {
      subtasks = e.detail.items;
    }
  
    function handleDndFinalize(e) {
      subtasks = e.detail.items;
      onUpdateStatus(subtasks, dropZoneId);
    }
  </script>
  
  <div
    use:dndzone="{{items: subtasks, flipDurationMs: 300}}"
    on:consider={handleDndConsider}
    on:finalize={handleDndFinalize}
    class="p-4 border rounded shadow bg-gray-100 dark:bg-gray-700"
  >
    <h3 class="font-bold text-lg">{dropZoneId}</h3>
    {#if subtasks.length > 0}
      <div class="mt-2">
        {#each subtasks as subtask}
          <div
            class="p-2 my-2 bg-white dark:bg-gray-800 rounded shadow cursor-pointer"
            data-id={subtask.id}
          >
            <p>{subtask.title}</p>
            <p class="text-sm text-gray-500">Status: {subtask.status}</p>
          </div>
        {/each}
      </div>
    {:else}
      <p class="text-gray-500">No subtasks</p>
    {/if}
  </div>