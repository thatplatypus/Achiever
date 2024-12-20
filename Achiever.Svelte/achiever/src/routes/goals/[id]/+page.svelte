<script lang="ts">
    import { getGoalById, updateGoal, type SubTask, type Goal } from "$lib/api/goals";
    import SubTaskDropzone from "$lib/components/ui/SubTaskDropzone.svelte";
    import { onMount } from "svelte";
    import { page } from '$app/state';

  
    let goalId: string = '';
    let goal: Goal
    let loading = true;
    let errorMessage: string | null = null;
  

    goalId = page.params.id;

  
    onMount(async () => {
      try {
        const result = await getGoalById(goalId);
        if (result.isSuccess) {
          goal = result.value;
        } else {
          errorMessage = result.message || 'Failed to load goal details';
        }
      } catch (error) {
        errorMessage = error.message;
      } finally {
        loading = false;
      }
    });
  
    async function handleUpdateSubtasks(updatedSubtasks: SubTask[], dropZoneId: string) {
      updatedSubtasks.forEach((subtask: SubTask) => {
        subtask.status = dropZoneId; // Update the status based on the dropzone
      });
  
      const result = await updateGoal({ ...goal, subTasks: [...goal.subTasks] });
      if (!result.isSuccess) {
        console.error("Failed to update goal:", result.message);
      }
    }
  </script>
  
  {#if loading}
    <p>Loading goal details...</p>
  {:else if errorMessage}
    <p>Error: {errorMessage}</p>
  {:else if goal}
    <div class="p-4 bg-white dark:bg-gray-800 rounded-lg shadow">
      <h1 class="text-xl font-bold">{goal.title}</h1>
      <p>Status: {goal.status}</p>
  
      <div class="grid grid-cols-3 gap-4 mt-4">
        <SubTaskDropzone
          dropZoneId="New"
          subtasks={goal.subTasks.filter((st: SubTask) => st.status === "New")}
          onUpdateStatus={handleUpdateSubtasks}
        />
        <SubTaskDropzone
          dropZoneId="InProgress"
          subtasks={goal.subTasks.filter((st: SubTask) => st.status === "InProgress")}
          onUpdateStatus={handleUpdateSubtasks}
        />
        <SubTaskDropzone
          dropZoneId="Completed"
          subtasks={goal.subTasks.filter((st: SubTask) => st.status === "Completed")}
          onUpdateStatus={handleUpdateSubtasks}
        />
      </div>
    </div>
  {:else}
    <p>Goal not found.</p>
  {/if}