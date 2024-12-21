<script lang="ts">
    import type { SubTask } from "$lib/api/goals";
    import { Button } from "$lib/components/ui/button/index.js";
    import * as Dialog from "$lib/components/ui/dialog/index.js";
    import { Input } from "$lib/components/ui/input/index.js";
    import { Label } from "$lib/components/ui/label/index.js";
    import * as Select from "$lib/components/ui/select/index.js";
    import { Clock, NotepadText } from "lucide-svelte";
    import Circle from "lucide-svelte/icons/circle";
 import CircleArrowUp from "lucide-svelte/icons/circle-arrow-up";
 import CircleCheck from "lucide-svelte/icons/circle-check";
    import { Slider } from "./slider";
    import Textarea from "./textarea/textarea.svelte";
    import { statuses } from "$lib/api/subtaskStatus";
  
    export let subTask = {
      id: "",
      title: "",
      status: "New",
      estimatedHours: 0,
      note: "",
    }; // Initial subtask data
  
    export let onSave: (updatedSubTask: SubTask) => void;
  
    let updatedSubTask = { ...subTask };
    let open = false;
  
    function handleSave() {
      onSave(updatedSubTask);
      open = false;
    }
  
    function openDialog() {
      open = true;
    }
  
    function handleStatusChange(newStatus: {value: string, label: string}) {
      updatedSubTask.status = newStatus.value;
    }
    function handleSetHours(hours: number) {
        updatedSubTask.estimatedHours = hours;
    }

    function cancel(){
        open =false;
    }
  </script>
  
  <Dialog.Root bind:open>
    <Dialog.Trigger on:click={openDialog}>
      <slot />
    </Dialog.Trigger>
    <Dialog.Content class="sm:max-w-[500px]">
      <Dialog.Header>
        <Dialog.Title>Edit Subtask</Dialog.Title>
        <Dialog.Description>
          Make changes to your subtask.
        </Dialog.Description>
      </Dialog.Header>
      <div class="grid gap-6 py-4">
        <div class="grid gap-4">
          <Label for="title">Title</Label>
          <Input id="title" bind:value={updatedSubTask.title} class="col-span-3" />
        </div>
        <div class="grid gap-4">
          <Label for="status">Status</Label>
          <div class="col-span-3">
            <Select.Root
              selected={statuses.find((status) => status.value === updatedSubTask.status)}
              onSelectedChange={handleStatusChange}
              portal={null}
            >
              <Select.Trigger class="w-full">
                <svelte:component
                        this={statuses.find((status) => status.value === updatedSubTask.status)?.icon} 
                        class="mr-2 h-4 w-4"
                    />
                <Select.Value placeholder="Select status"/> 
              </Select.Trigger>
              <Select.Content>
                <Select.Group>
                  {#each statuses as status}
                    <Select.Item value={status.value} label={status.label}>
                        <svelte:component
                        this={status.icon} 
                        class="mr-2 h-4 w-4"
                    />
                      {status.label}
                    </Select.Item>
                  {/each}
                </Select.Group>
              </Select.Content>
              <Select.Input name="status" />
            </Select.Root>
          </div>
        </div>
        <div class="grid gap-4">
            <div class="flex gap-2 items-center">
            <Clock class="h-6 w-6" />
            <Label for="estimatedHours">Estimated Hours</Label>
            <span class="text-sm text-muted-foreground">{updatedSubTask.estimatedHours}</span>
            </div>
            <Slider
                id="estimatedHours"
                value={[updatedSubTask.estimatedHours * 2]} 
                onValueChange={(e) => (updatedSubTask.estimatedHours = e[0] / 2)}
                max={48} 
                step={1}
                on:change={(e) => (updatedSubTask.estimatedHours = e.detail[0] / 2)}
            />
            <div class="flex justify-between text-sm text-muted-foreground">
                <span>0 hrs</span>
                <span>12 hrs</span>
                <span>24 hrs</span>
            </div>
            <div class="flex gap-2 justify-between">
                <!-- Convenience Pills -->
                {#each [0.5, 1, 2, 4, 8] as hour}
                    <Button
                        variant="outline"
                        size="sm"
                        on:click={() => handleSetHours(hour)}
                    >
                        {hour} {hour === 1 ? "hr" : "hrs"}
                    </Button>
                {/each}
            </div>
        </div>
        <div class="grid gap-4">
            <div class="flex items-center gap-2">
            <NotepadText class="h-6 w-6" />
            <Label for="note">
            Notes
        </Label>
        </div>
          <Textarea id="note" bind:value={updatedSubTask.note} class="col-span-3" />
        </div>
      </div>
      <Dialog.Footer>
        <Button variant="ghost" on:click={cancel}>Cancel</Button>
        <Button variant="outline" on:click={handleSave}>Save</Button>
      </Dialog.Footer>
    </Dialog.Content>
  </Dialog.Root>