<script lang="ts">
    import * as Tooltip from "$lib/components/ui/tooltip";
    export let tooltipText: string;
    export let variant: string = "outline";
  
    // Event forwarding for click events
    import { createEventDispatcher } from "svelte";
    import Button from "./button/button.svelte";
    const dispatch = createEventDispatcher();
  </script>
  
  <Tooltip.Root>
    <Tooltip.Trigger>
      <Button
         class="p-2 border hover:text-accent-foreground transition-colors"
         variant={variant}  
        on:click={(event) => dispatch('click', event)}
      >
        <slot />
    </Button>
    </Tooltip.Trigger>
    <Tooltip.Content>
      <p>{tooltipText}</p>
    </Tooltip.Content>
  </Tooltip.Root>