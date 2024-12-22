<script lang="ts">
  import { type SubTask } from "$lib/api/goals";
  import EditSubtaskDialog from "./EditSubtaskDialog.svelte";
  import { Clock, NotepadText } from "lucide-svelte";
  import * as Tooltip from "$lib/components/ui/tooltip/index.js";
  import { statuses } from "$lib/api/subtaskStatus";

  export let subTask: SubTask;
  export let onEdit: (updatedSubTask: SubTask) => void;

  let open = false;

  function handleSave(updatedSubTask: SubTask) {
    onEdit(updatedSubTask);
    open = false;
  }

  function openEditDialog() {
    open = true;
  }

  // Utility to get status-specific color
  function getStatusColor(status: string): string {
    switch (status) {
      case "New":
        return "bg-gray-500";
      case "InProgress":
        return "bg-sky-500";
      case "Completed":
        return "bg-emerald-500";
      default:
        return "bg-gray-500";
    }
  }
</script>



<!-- Edit Dialog -->
<EditSubtaskDialog subTask={subTask} onSave={handleSave} bind:open>
    <div
      class="shrink-0 p-4 bg-white dark:bg-gray-900 rounded-md shadow-md hover:shadow-lg transition-shadow cursor-pointer"
      on:click={openEditDialog}
    >
      <!-- Top Row -->
      <div class="flex gap-2 mb-2">
        <!-- Status Pill -->
        <div
          class="w-2 h-10 rounded-full {getStatusColor(subTask.status)}"
        ></div>
  
        <!-- Title and Status -->
        <div class="flex flex-col align-items-start text-start">
          <h4 class="text-sm font-semibold text-foreground truncate">
            {subTask.title}
          </h4>
          <div class="flex items-center text-sm text-gray-500 dark:text-gray-400 gap-1">
            <svelte:component
              this={statuses.find((status) => status.value === subTask.status)?.icon}
              class="h-4 w-4"
            />
            {statuses.find((st) => st.value === subTask.status)?.label}
          </div>
        </div>
      </div>
  
      <!-- Bottom Row -->
      <div class="flex items-center gap-1 text-sm text-gray-700 dark:text-gray-300">
        <!-- Estimated Hours -->
        <Tooltip.Root>
          <Tooltip.Trigger>
            <div class="flex items-center gap-1">
              <Clock class="h-4 w-4 {subTask?.estimatedHours > 0 ? "text-gray-700 dark:text-gray-300" :"text-gray-500" }" />
              {#if subTask.estimatedHours && subTask.estimatedHours > 0}
                <span>{subTask.estimatedHours}</span>
              {/if}
            </div>
          </Tooltip.Trigger>
          <Tooltip.Content>
            <p class="bg-background p-2">
              {subTask.estimatedHours > 0
                ? `${subTask.estimatedHours} estimated hours`
                : "No estimated hours"}
            </p>
          </Tooltip.Content>
        </Tooltip.Root>
  
        <!-- Notes -->
        <Tooltip.Root>
          <Tooltip.Trigger>
            <div class="flex items-center gap-1">
              <NotepadText
                class="h-4 w-4 {subTask.note ? 'text-gray-700 dark:text-gray-300' : 'text-gray-500'}"
              />
            </div>
          </Tooltip.Trigger>
          <Tooltip.Content>
            <p class="bg-background p-2">
              {subTask.note || "No notes"}
            </p>
          </Tooltip.Content>
        </Tooltip.Root>
      </div>
    </div>
  </EditSubtaskDialog>