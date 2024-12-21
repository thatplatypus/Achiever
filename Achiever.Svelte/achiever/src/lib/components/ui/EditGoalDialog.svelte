<script lang="ts">
    import { Button } from "$lib/components/ui/button/index.js";
    import * as Dialog from "$lib/components/ui/dialog/index.js";
    import { Input } from "$lib/components/ui/input/index.js";
    import { Label } from "$lib/components/ui/label/index.js";
  
    export let goal = { id: "", title: "", status: 0 }; // Initial goal data
    export let onSave: (updatedGoal: { id: string; title: string; status: number }) => void;
  
    let updatedGoal = { ...goal };

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
  </script>
  
  <Dialog.Root bind:open>
    <Dialog.Trigger on:click={openDialog}>
      <slot /> 
    </Dialog.Trigger>
    <Dialog.Content class="sm:max-w-[425px]">
      <Dialog.Header>
        <Dialog.Title>Edit Goal</Dialog.Title>
        <Dialog.Description>
          Make changes to your goal. Click save when you're done.
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
          <Label for="status" class="text-right">Status</Label>
          <Input
            id="status"
            type="number"
            bind:value={updatedGoal.status}
            class="col-span-3"
          />
        </div>
      </div>
      <Dialog.Footer>
        <Button variant="ghost" on:click={cancel}>Cancel</Button>
        <Button variant="outline" on:click={handleSave}>Save</Button>
      </Dialog.Footer>
    </Dialog.Content>
  </Dialog.Root>