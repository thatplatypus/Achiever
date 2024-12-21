<script lang="ts">
    import { onMount } from 'svelte';
    import { getGoals, createGoal, deleteGoal } from '$lib/api/goals';
    import GoalCard from '$lib/components/ui/GoalCard.svelte';
    import Button from '$lib/components/ui/button/button.svelte';
    import Plus from 'lucide-svelte/icons/plus';
  
    let goals = [];
    let loading = true;
    let errorMessage = '';
  
    onMount(async () => {
      const result = await getGoals();
      if (result.isSuccess) {
        goals = result.value;
      } else {
        errorMessage = result.message;
      }
      loading = false;
    });
  
    async function handleCreateGoal() {
      const result = await createGoal('New Goal');
      if (result.isSuccess) {
        goals = [...goals, result.value];
      } else {
        alert(`Failed to create goal: ${result.message}`);
      }
    }
  
    async function handleDeleteGoal(goalId: string) {
      if (confirm('Are you sure you want to delete this goal?')) {
        const result = await deleteGoal(goalId);
        if (result.isSuccess) {
          goals = goals.filter((goal) => goal.id !== goalId);
        } else {
          alert(`Failed to delete goal: ${result.message}`);
        }
      }
    }
  </script>
  
 
<div class="flex items-center justify-between mb-4">
    <h1 class="text-lg font-bold">Goals</h1>
    <Button 
      on:click={handleCreateGoal}
      variant="outline"
      class="rounded hover:bg-background/50 hover:text-accent-foreground transition-colors"
    >
    <Plus class="mr-2 h-4 w-4" />
      Create Goal
    </Button>
  </div>
  
  {#if loading}
    <p>Loading...</p>
  {:else if errorMessage}
    <p>Error: {errorMessage}</p>
  {:else}
  <ul class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-5">
    {#each goals as goal}
              <GoalCard {goal} onDelete={handleDeleteGoal} />
            {/each}
          </ul>
  {/if}