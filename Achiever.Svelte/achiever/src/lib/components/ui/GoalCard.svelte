<script lang="ts">
  import { Button } from "$lib/components/ui/button/index.js";
  import * as Card from "$lib/components/ui/card/index.js";
  import { Label } from "$lib/components/ui/label/index.js";
  import { goto } from "$app/navigation";
  import TooltipIconButton from "./TooltipIconButton.svelte";
  import EditGoalDialog from "./EditGoalDialog.svelte";
  import Pencil from "lucide-svelte/icons/pencil";
  import Trash from "lucide-svelte/icons/trash";
  import { updateGoal, type Goal } from "$lib/api/goals";

  export let goal: Goal;

  export let onDelete: (goalId: string) => void;

  // Handle deleting the goal
  function handleDelete() {
    onDelete(goal.id);
  }

  // Handle navigating to the goal details page
  function handleCardClick(event: MouseEvent) {
    const isButtonClick = (event.target as HTMLElement).closest("button");
    if (!isButtonClick) {
      handleNavigate(goal.id);
    }
  }

  function handleNavigate(goalId: string) {
    goto(`/goals/${goalId}`);
  }

  // Handle updating the goal
  async function handleGoalUpdated(id: string, title: string, status: number) {
    console.log("Goal updated:", { id, title, status });
    goal.title = title;
    goal.status = status;
    const result = await updateGoal(goal);
    if (result.isSuccess) {
      console.log("Goal successfully updated!");
    } else {
      alert(`Failed to update goal: ${result.message}`);
    }
  }
</script>

<Card.Root
  class="hover:shadow-lg hover:cursor-pointer transition-shadow duration-300 hover:shadow-lg hover:scale-105 transition-transform duration-200"
  onclick={handleCardClick}
>
  <Card.Header>
    <Card.Title>{goal.title}</Card.Title>
    <Card.Description>
      {goal.subTasks.length} Subtasks | Status: {goal.status}
    </Card.Description>
  </Card.Header>
  <Card.Content>
    <div class="grid w-full items-center gap-4">
      {#if goal.subTasks.length > 0}
        <div>
          <Label>Subtasks</Label>
          <ul class="list-disc ml-4">
            {#each goal.subTasks as subTask}
              <li>{subTask.title} - {subTask.status}</li>
            {/each}
          </ul>
        </div>
      {/if}
    </div>
  </Card.Content>
  <Card.Footer class="flex justify-between">
    <EditGoalDialog {goal} onSave={(updated) => handleGoalUpdated(updated.id, updated.title, updated.status)}>
      <TooltipIconButton tooltipText="Edit Goal">
        <Pencil class="h-4 w-4" />
      </TooltipIconButton>
    </EditGoalDialog>
    <Button variant="destructive" on:click={handleDelete}>
      <Trash class="mr-2 h-4 w-4" />
      Delete
    </Button>
  </Card.Footer>
</Card.Root>