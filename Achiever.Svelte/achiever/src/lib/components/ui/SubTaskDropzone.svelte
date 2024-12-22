<script lang="ts">
    import type { SubTask } from "$lib/api/goals";
    import { dndzone } from "svelte-dnd-action";
    import { flip } from "svelte/animate";
    import EditSubTaskDialog from "./EditSubTaskDialog.svelte";
    import TooltipIconButton from "./TooltipIconButton.svelte";
    import Pencil from "lucide-svelte/icons/pencil";
    import { onMount } from "svelte";
    import { statuses } from "$lib/api/subtaskStatus";
    import { Clock, NotepadText } from "lucide-svelte";
    import * as Tooltip from "./tooltip";
  
    export let dropZoneId: string; 
    export let dropZoneTitle: string;
    export let subtasks: SubTask[] = []; 
    export let onUpdateStatus: (subtaskId: string, dropZoneId: string) => void; 
    export let onSubtaskUpdated: (subtask: SubTask) => void; 

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

  function getStatusTextLabelColor(status: string): string {
    switch (status) {
      case "New":
        return "text-gray-500";
      case "InProgress":
        return "text-gray-700 dark:text-gray-300";
      case "Completed":
        return "text-emerald-500";
      default:
        return "text-gray-500";
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
        <div class="text-sm text-gray-500 flex gap-3 width-full"> 

        <span class="text-sm {getStatusTextLabelColor(subtask.status)} flex gap-0 items-center"> 
          <svelte:component
          this={statuses.find((status) => status.value === subtask.status)?.icon} 
          class="mr-2 h-4 w-4" />
          {statuses.find((st) => st.value === subtask.status)?.label}
        </span> 

        {#if subtask.estimatedHours && subtask.estimatedHours > 0}
            <Tooltip.Root>
              <Tooltip.Trigger>
                <span class="text-sm text-gray-700 dark:text-gray-300 flex gap-1 items-center">
            <Clock class="h-4 w-4" />
            <span>{subtask.estimatedHours}</span>
          </span>

          </Tooltip.Trigger>
          <Tooltip.Content>
            <p>{subtask.estimatedHours} estimated hours</p>
          </Tooltip.Content>
        </Tooltip.Root> 
        {:else}
        <Tooltip.Root>
          <Tooltip.Trigger>
            <span class="text-sm text-gray-500 flex gap-1 items-center">
        <Clock class="h-4 w-4" />
      </span>

      </Tooltip.Trigger>
      <Tooltip.Content>
        <p class="bg-background p-2">No estimated hours</p>
      </Tooltip.Content>
    </Tooltip.Root> 
        {/if}

        {#if subtask.note}
          <span class="text-sm text-gray-700 dark:text-gray-300 flex gap-1 items-center">
            <Tooltip.Root>
              <Tooltip.Trigger>
                <NotepadText class="h-4 w-4" />
              </Tooltip.Trigger>
              <Tooltip.Content>
                <quote class="bg-background p-2">{subtask.note}</quote>
              </Tooltip.Content>
            </Tooltip.Root>    
          </span>
          {:else}
          <span class="text-sm text-gray-500 flex gap-1 items-center">
            <Tooltip.Root>
              <Tooltip.Trigger>
                <NotepadText class="h-4 w-4" />
              </Tooltip.Trigger>
              <Tooltip.Content>
                <p class="bg-background p-2">No notes</p>
              </Tooltip.Content>
            </Tooltip.Root>    
          </span>
        {/if}
        </div>
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