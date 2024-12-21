<script lang="ts">
    import { getGoalById, updateGoal, type SubTask, type Goal, createSubTask } from "$lib/api/goals";
    import SubTaskDropzone from "$lib/components/ui/SubTaskDropzone.svelte";
    import { onMount } from "svelte";
    import { page } from '$app/state';
    import { Button } from "$lib/components/ui/button";
    import  * as Breadcrumb from "$lib/components/ui/breadcrumb";
    import { Plus } from "lucide-svelte";

  
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
  
    let isUpdating = false;

async function handleUpdateSubtasks(updatedSubtaskId: string, dropZoneId: string) {
  console.log(updatedSubtaskId, dropZoneId);
 goal.subTasks = goal.subTasks.map((st: SubTask) => {
    if (st.id === updatedSubtaskId) {
      st.status = dropZoneId;
    }
    return st;
  });

  try {
    const result = await updateGoal({ ...goal, subTasks: [...goal.subTasks] });

    if (!result.isSuccess) {
      console.error("Failed to update goal:", result.message);
    } else {
      console.log("Goal updated successfully.");
    }
  } catch (error) {
    console.error("Error updating goal:", error);
  } 
}

async function handleSubtaskUpdated(updatedSubtask: SubTask) {
    const subtaskIndex = goal.subTasks.findIndex((st: SubTask) => st.id === updatedSubtask.id);
  if (subtaskIndex !== -1) {
    // Update the specific subtask in the array
    goal.subTasks[subtaskIndex] = {
      ...goal.subTasks[subtaskIndex],
      ...updatedSubtask,
    };

    // Assign a new reference to `goal.subTasks` to trigger reactivity
    goal.subTasks = [...goal.subTasks];

    // Perform the API call
    const result = await updateGoal(goal);

    if (!result.isSuccess) {
      console.error("Failed to update goal:", result.message);
    }
}
}

async function createSubtask() {
    const newSubtask: SubTask = {
      id: crypto.randomUUID(), // Generate a temporary ID
      title: "New Subtask",
      status: "New",
    };

    goal.subTasks = [...goal.subTasks, newSubtask];

    const result = await createSubTask(goal.id, newSubtask);

    if (!result.isSuccess) {
      console.error("Failed to create subtask:", result.message);
      goal.subTasks = goal.subTasks.filter((st) => st.id !== newSubtask.id); // Remove on failure
    } else {
      console.log("Subtask created successfully.");
    }
  }
  </script>

<Breadcrumb.Root class="mb-4">
    <Breadcrumb.List>
      <Breadcrumb.Item>
        <Breadcrumb.Link href="/goals">Goals</Breadcrumb.Link>
      </Breadcrumb.Item>
      <Breadcrumb.Separator />
      <Breadcrumb.Item>
        <Breadcrumb.Page>{goal?.title || "Goal Details"}</Breadcrumb.Page>
      </Breadcrumb.Item>
    </Breadcrumb.List>
  </Breadcrumb.Root>
  
  {#if loading}
    <p>Loading goal details...</p>
  {:else if errorMessage}
    <p>Error: {errorMessage}</p>
  {:else if goal}
    <div class="p-4 bg-gradient-to-b from-slate-100 to-bg-white dark:bg-gradient-to-b dark:from-slate-800 dark:via-slate-800 dark:to-background rounded-lg shadow">
        <div class="flex justify-between items-center">
            <div>
              <h1 class="text-xl font-bold">{goal.title}</h1>
              <p class="text-gray-400">Status: {goal.subTasks.filter(st => st.status === "Completed").length}/{goal.subTasks.length} complete</p>
            </div>
            <Button on:click={createSubtask} variant="outline" class="hover:bg-background/50 hover:text-accent-foreground transition-colors">
              <Plus class="mr-2 h-4 w-4" />
              Create Subtask
            </Button>
            </div>  
      <div class="grid grid-cols-3 gap-4 mt-4" style="height:75dvh">
        <SubTaskDropzone
          dropZoneId="New"
          dropZoneTitle="New"
          subtasks={goal.subTasks.filter((st: SubTask) => st.status === "New")}
          onUpdateStatus={handleUpdateSubtasks}
          onSubtaskUpdated={handleSubtaskUpdated}
        />
        <SubTaskDropzone
          dropZoneId="InProgress"
          dropZoneTitle = "In Progress"
          subtasks={goal.subTasks.filter((st: SubTask) => st.status === "InProgress")}
          onUpdateStatus={handleUpdateSubtasks}
          onSubtaskUpdated={handleSubtaskUpdated}
        />
        <SubTaskDropzone
          dropZoneId="Completed"
          dropZoneTitle="Complete"
          subtasks={goal.subTasks.filter((st: SubTask) => st.status === "Completed")}
          onUpdateStatus={handleUpdateSubtasks}
          onSubtaskUpdated={handleSubtaskUpdated}
        />

      </div>
    </div>
  {:else}
    <p>Goal not found.</p>
  {/if}