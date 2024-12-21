<script lang="ts">
    import type { SubTask } from "$lib/api/goals";
    import { dndzone } from "svelte-dnd-action";
    import { flip } from "svelte/animate";
    import EditSubTaskDialog from "./EditSubTaskDialog.svelte";
    import TooltipIconButton from "./TooltipIconButton.svelte";
    import Pencil from "lucide-svelte/icons/pencil";
    import { onMount } from "svelte";
    import { statuses } from "$lib/api/subtaskStatus";
  
    export let dropZoneId: string; 
    export let dropZoneTitle: string;
    export let subtasks: SubTask[] = []; // List of subtasks in this dropzone
    export let onUpdateStatus: (subtaskId: string, dropZoneId: string) => void; // Callback when subtasks are updated
    export let onSubtaskUpdated: (subtask: SubTask) => void; // Callback when a subtask is updated

    function handleDndConsider(e) {
      subtasks = e.detail.items;
    }
  
    function handleDndFinalize(e) {
      console.log(e.detail.info.trigger);
      if(e.detail.info.trigger !== "droppedIntoZone") return;

      var subtaskId = e.detail.info.id;

      console.log(subtaskId, dropZoneId);

     onUpdateStatus(subtaskId, dropZoneId);
    }
    function handleSubTaskSave(updatedSubTask: SubTask) {
      onSubtaskUpdated(updatedSubTask);
    }
    function getDropZoneColor(zoneId: string): string {
    switch (zoneId) {
      case "New":
        return "var(--kanban-new-color)";
      case "InProgress":
        return "var(--kanban-in-progress-color)";
      case "Completed":
        return "var(--kanban-completed-color)";
      default:
        return "transparent";
    }
  }
  
  </script>
  <div class="h-full">
  <h4 class="font-bold text-lg mb-2">{dropZoneTitle}</h4>
  <div
  use:dndzone={{ items: subtasks, flipDurationMs: 300 }}
  on:consider={handleDndConsider}
  on:finalize={handleDndFinalize}
  class="max-h-[50dvh] min-h-[50dvh] overflow-y-auto p-2 rounded-lg border-2 border-gray-700 "
  style="border-color: {getDropZoneColor(dropZoneId)}"
>
  {#if subtasks.length > 0}
      {#each subtasks as subtask (subtask.id)}
      <div
      class="p-2 my-2 bg-white dark:bg-gray-900 rounded shadow cursor-pointer flex items-center space-x-4"
    >
      <!-- Status Pill -->
      <div
        class="w-2 h-8 rounded-full"
        style="background-color: {getDropZoneColor(subtask.status)}"
      ></div>
  
      <!-- Subtask Details -->
      <div class="flex-1">
        <p class="font-medium">{subtask.title}</p>
        <p class="text-sm text-gray-500 flex gap-1 items-center"> 
          <svelte:component
          this={statuses.find((status) => status.value === subtask.status)?.icon} 
          class="mr-2 h-4 w-4"
      /> 
      {statuses.find((st) => st.value === subtask.status)?.label}
    </p>
      </div>
  
      <!-- Edit Button -->
      <div class="flex items-center">
        <EditSubTaskDialog subTask={subtask} onSave={handleSubTaskSave}>
          <TooltipIconButton tooltipText="Edit Subtask">
            <Pencil class="h-4 w-4" />
          </TooltipIconButton>
        </EditSubTaskDialog>
      </div>
    </div>
      {/each}
  {:else}
    <div class="text-center mt-4">
      <p class="text-gray-500">No subtasks</p>
    </div>
  {/if}
</div>
</div>