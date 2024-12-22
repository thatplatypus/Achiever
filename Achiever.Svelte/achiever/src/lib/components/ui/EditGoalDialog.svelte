<script lang="ts">
    import { Button } from "$lib/components/ui/button/index.js";
    import * as Dialog from "$lib/components/ui/dialog/index.js";
    import { Input } from "$lib/components/ui/input/index.js";
    import { Label } from "$lib/components/ui/label/index.js";
    import { goto } from "$app/navigation"; // SvelteKit for navigation
    import { type Goal } from "$lib/api/goals";
    import { SquareArrowOutUpRight } from "lucide-svelte";
  
    export let goal: Goal = { id: "", title: "", status: 0, subTasks: [] }; 
    export let onSave: (updatedGoal: { id: string; title: string; status: number, targetEndDate?: string | Date | undefined }) => void;
  
    let updatedGoal = { ...goal };
  
    // Format targetEndDate for the input field
    $: updatedGoal.targetEndDate = getFormattedDate(goal.targetEndDate);
  
    function getFormattedDate(date: string | Date | undefined): string {
      if (!date) return ""; // Handle empty date
      const d = date instanceof Date ? date : new Date(date);
      return d.toISOString().split("T")[0]; // Format to "YYYY-MM-DD"
    }
  
    let open = false;
  
    function handleSave() {
      onSave(updatedGoal);
      open = false;
    }
  
    function openDialog() {
      open = true;
    }
  
    function cancel() {
      open = false;
    }
  
    function viewDetails() {
      if (goal.id) {
        goto(`/goals/${goal.id}`);
        open = false;
      }
    }
  </script>
  
  <Dialog.Root bind:open>
    <Dialog.Trigger on:click={openDialog}>
      <slot />
    </Dialog.Trigger>
    <Dialog.Content class="sm:max-w-[425px]">
      <Dialog.Header>
        <Dialog.Title>Edit Goal</Dialog.Title>
        <Dialog.Description>
          Make changes to your goal.
        </Dialog.Description>
      </Dialog.Header>
      <div class="grid gap-4 py-4">
        <div class="grid grid-cols-4 items-center gap-4">
          <Label for="title" class="text-right">Title</Label>
          <Input
            id="title"
            bind:value={updatedGoal.title}
            class="col-span-3"
          />
        </div>
        <div class="grid grid-cols-4 items-center gap-4">
          <Label for="targetDate" class="text-right">Target Date</Label>
          <Input
            id="targetDate"
            type="date"
            bind:value={updatedGoal.targetEndDate}
            class="col-span-3"
          />
        </div>
      </div>
      <Dialog.Footer class="flex justify-between">
        <!-- View Details Button -->
        <Button variant="link" class="text-primary" on:click={viewDetails}>
          View Details
          <SquareArrowOutUpRight class="h-4 w-4 ml-2" />
        </Button>
        <div class="flex flex-1"></div>
        <div class="flex space-x-2">
          <!-- Cancel and Save Buttons -->
          <Button variant="ghost" on:click={cancel}>Cancel</Button>
          <Button variant="outline" on:click={handleSave}>Save</Button>
        </div>
      </Dialog.Footer>
    </Dialog.Content>
  </Dialog.Root>